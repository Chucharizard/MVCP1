using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WAMVC.Data;
using WAMVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace WAMVC.Controllers
{
    [Authorize]
    public class DetallePedidoController : Controller
    {
        private readonly ArtesaniasDBContext _context;

        public DetallePedidoController(ArtesaniasDBContext context)
        {
            _context = context;
        }

        // GET: DetallePedido
        public async Task<IActionResult> Index()
        {
            var detallePedidos = await _context.DetallePedidos
                .Include(d => d.Pedido)
                    .ThenInclude(p => p.Cliente)
                .Include(d => d.Producto)
                .ToListAsync();
            return View(detallePedidos);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detallePedidoModel = await _context.DetallePedidos
                .Include(d => d.Pedido)
                    .ThenInclude(p => p!.Cliente)
                .Include(d => d.Producto)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (detallePedidoModel == null)
            {
                return NotFound();
            }

            return View(detallePedidoModel);
        }

        // GET: DetallePedido/Create
        public IActionResult Create(int? pedidoId)
        {
            PopulateDropdowns();
            
            var detalle = new DetallePedidoModel();
            if (pedidoId.HasValue)
            {
                detalle.IdPedido = pedidoId.Value;
                ViewData["PedidoSeleccionado"] = pedidoId.Value;
            }
            
            return View(detalle);
        }

        // POST: DetallePedido/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdPedido,IdProducto,Cantidad,PrecioUnitario")] DetallePedidoModel detallePedidoModel)
        {
            if (ModelState.IsValid)
            {
                // Verificar que el producto existe y tiene stock suficiente
                var producto = await _context.Productos.FindAsync(detallePedidoModel.IdProducto);
                if (producto == null)
                {
                    ModelState.AddModelError("IdProducto", "El producto seleccionado no existe.");
                    PopulateDropdowns(detallePedidoModel);
                    return View(detallePedidoModel);
                }

                if (producto.Stock < detallePedidoModel.Cantidad)
                {
                    ModelState.AddModelError("Cantidad", $"Stock insuficiente. Solo hay {producto.Stock} unidades disponibles.");
                    PopulateDropdowns(detallePedidoModel);
                    return View(detallePedidoModel);
                }

                // Usar el precio actual del producto si no se especificó
                if (detallePedidoModel.PrecioUnitario <= 0)
                {
                    detallePedidoModel.PrecioUnitario = producto.Precio;
                }

                _context.Add(detallePedidoModel);
                
     
                producto.Stock -= detallePedidoModel.Cantidad;
                _context.Update(producto);
                
                await _context.SaveChangesAsync();
                
                await RecalcularMontoPedido(detallePedidoModel.IdPedido);
                
                TempData["SuccessMessage"] = $"Artículo agregado al pedido exitosamente. Stock actualizado: {producto.Stock} unidades restantes.";
                return RedirectToAction("Details", "Pedido", new { id = detallePedidoModel.IdPedido });
            }
            
            PopulateDropdowns(detallePedidoModel);
            return View(detallePedidoModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detallePedidoModel = await _context.DetallePedidos
                .Include(d => d.Producto)
                .FirstOrDefaultAsync(d => d.Id == id);
            
            if (detallePedidoModel == null)
            {
                return NotFound();
            }
            
            ViewData["CantidadOriginal"] = detallePedidoModel.Cantidad;
            
            PopulateDropdowns(detallePedidoModel);
            return View(detallePedidoModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdPedido,IdProducto,Cantidad,PrecioUnitario")] DetallePedidoModel detallePedidoModel, int cantidadOriginal)
        {
            if (id != detallePedidoModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var producto = await _context.Productos.FindAsync(detallePedidoModel.IdProducto);
                    if (producto == null)
                    {
                        ModelState.AddModelError("IdProducto", "El producto seleccionado no existe.");
                        ViewData["CantidadOriginal"] = cantidadOriginal;
                        PopulateDropdowns(detallePedidoModel);
                        return View(detallePedidoModel);
                    }

                    int diferenciaCantidad = detallePedidoModel.Cantidad - cantidadOriginal;
                    
                    if (diferenciaCantidad > 0 && producto.Stock < diferenciaCantidad)
                    {
                        ModelState.AddModelError("Cantidad", $"Stock insuficiente. Solo hay {producto.Stock} unidades adicionales disponibles.");
                        ViewData["CantidadOriginal"] = cantidadOriginal;
                        PopulateDropdowns(detallePedidoModel);
                        return View(detallePedidoModel);
                    }

                    producto.Stock -= diferenciaCantidad;
                    _context.Update(producto);
                 
                    _context.Update(detallePedidoModel);
                    await _context.SaveChangesAsync();
                    
                    await RecalcularMontoPedido(detallePedidoModel.IdPedido);
                    
                    TempData["SuccessMessage"] = "Detalle del pedido actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetallePedidoModelExists(detallePedidoModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Pedido", new { id = detallePedidoModel.IdPedido });
            }
            
            ViewData["CantidadOriginal"] = cantidadOriginal;
            PopulateDropdowns(detallePedidoModel);
            return View(detallePedidoModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detallePedidoModel = await _context.DetallePedidos
                .Include(d => d.Pedido)
                    .ThenInclude(p => p!.Cliente)
                .Include(d => d.Producto)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (detallePedidoModel == null)
            {
                return NotFound();
            }

            return View(detallePedidoModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var detallePedidoModel = await _context.DetallePedidos
                .Include(d => d.Producto)
                .FirstOrDefaultAsync(d => d.Id == id);
                
            if (detallePedidoModel != null)
            {
                if (detallePedidoModel.Producto != null)
                {
                    detallePedidoModel.Producto.Stock += detallePedidoModel.Cantidad;
                    _context.Update(detallePedidoModel.Producto);
                }
                
                int pedidoId = detallePedidoModel.IdPedido;
                
                _context.DetallePedidos.Remove(detallePedidoModel);
                await _context.SaveChangesAsync();
                
                await RecalcularMontoPedido(pedidoId);
                
                TempData["SuccessMessage"] = "Artículo eliminado del pedido y stock restaurado exitosamente.";
                return RedirectToAction("Details", "Pedido", new { id = pedidoId });
            }
            
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<JsonResult> ObtenerPrecioProducto(int idProducto)
        {
            var producto = await _context.Productos.FindAsync(idProducto);
            if (producto != null)
            {
                return Json(new { 
                    precio = producto.Precio, 
                    stock = producto.Stock,
                    nombre = producto.Nombre 
                });
            }
            return Json(new { precio = 0, stock = 0, nombre = "" });
        }

        private bool DetallePedidoModelExists(int id)
        {
            return _context.DetallePedidos.Any(e => e.Id == id);
        }

        private void PopulateDropdowns(DetallePedidoModel? detalle = null)
        {
            ViewData["IdPedido"] = new SelectList(
                _context.Pedidos.Include(p => p.Cliente).Select(p => new { 
                    p.Id, 
                    Display = $"Pedido #{p.Id} - {p.Cliente!.Nombre}" 
                }), 
                "Id", 
                "Display", 
                detalle?.IdPedido);
                
            ViewData["IdProducto"] = new SelectList(
                _context.Productos.Where(p => p.Stock > 0).Select(p => new {
                    p.Id,
                    Display = $"{p.Nombre} (Stock: {p.Stock})"
                }), 
                "Id", 
                "Display", 
                detalle?.IdProducto);
        }

        private async Task RecalcularMontoPedido(int pedidoId)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.DetallePedidos)
                .FirstOrDefaultAsync(p => p.Id == pedidoId);

            if (pedido != null)
            {
                pedido.MontoTotal = pedido.DetallePedidos?.Sum(d => d.Cantidad * d.PrecioUnitario) ?? 0;
                _context.Update(pedido);
                await _context.SaveChangesAsync();
            }
        }
    }
}