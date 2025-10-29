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
    public class PedidoController : Controller
    {
        private readonly ArtesaniasDBContext _context;

        public PedidoController(ArtesaniasDBContext context)
        {
            _context = context;
        }

        // GET: Pedido - Todos pueden ver
        public async Task<IActionResult> Index()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Cliente)
                .ToListAsync();
            return View(pedidos);
        }

        // GET: Pedido/Details - Todos pueden ver detalles
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidoModel = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.DetallePedidos)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pedidoModel == null)
            {
                return NotFound();
            }

            return View(pedidoModel);
        }

        // GET: Pedido/Create - Admin y Empleado pueden crear
        [Authorize(Roles = "Admin,Empleado")]
        public IActionResult Create()
        {
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "Id", "Nombre");
            var pedido = new PedidoModel
            {
                FechaPedido = DateTime.Now
            };
            return View(pedido);
        }

        // POST: Pedido/Create - Admin y Empleado pueden crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Empleado")]
        public async Task<IActionResult> Create([Bind("Id,IdCliente,FechaPedido,Direccion,MontoTotal")] PedidoModel pedidoModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pedidoModel);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Pedido #{pedidoModel.Id} creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "Id", "Nombre", pedidoModel.IdCliente);
            return View(pedidoModel);
        }

        // GET: Pedido/Edit - Admin y Empleado pueden editar
        [Authorize(Roles = "Admin,Empleado")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidoModel = await _context.Pedidos.FindAsync(id);
            if (pedidoModel == null)
            {
                return NotFound();
            }
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "Id", "Nombre", pedidoModel.IdCliente);
            return View(pedidoModel);
        }

        // POST: Pedido/Edit - Admin y Empleado pueden editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Empleado")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdCliente,FechaPedido,Direccion,MontoTotal")] PedidoModel pedidoModel)
        {
            if (id != pedidoModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedidoModel);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Pedido #{pedidoModel.Id} actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoModelExists(pedidoModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "Id", "Nombre", pedidoModel.IdCliente);
            return View(pedidoModel);
        }

        // GET: Pedido/Delete - Admin y Empleado pueden eliminar
        [Authorize(Roles = "Admin,Empleado")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidoModel = await _context.Pedidos
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pedidoModel == null)
            {
                return NotFound();
            }

            return View(pedidoModel);
        }

        // POST: Pedido/Delete - Admin y Empleado pueden eliminar
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Empleado")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedidoModel = await _context.Pedidos.FindAsync(id);
            if (pedidoModel != null)
            {
                _context.Pedidos.Remove(pedidoModel);
                TempData["SuccessMessage"] = $"Pedido #{pedidoModel.Id} eliminado exitosamente.";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PedidoModelExists(int id)
        {
            return _context.Pedidos.Any(e => e.Id == id);
        }
    }
}
