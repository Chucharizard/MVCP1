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
    public class ProductoController : Controller
    {
        private readonly ArtesaniasDBContext _context;

        public ProductoController(ArtesaniasDBContext context)
        {
            _context = context;
        }

        // GET: Producto
        public async Task<IActionResult> Index()
        {
            return View(await _context.Productos.ToListAsync());
        }

        // GET: Producto/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productoModel = await _context.Productos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productoModel == null)
            {
                return NotFound();
            }

            return View(productoModel);
        }

        // GET: Producto/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Producto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,Precio,Stock")] ProductoModel productoModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productoModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productoModel);
        }

        // GET: Producto/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productoModel = await _context.Productos.FindAsync(id);
            if (productoModel == null)
            {
                return NotFound();
            }
            return View(productoModel);
        }

        // POST: Producto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Precio,Stock")] ProductoModel productoModel)
        {
            if (id != productoModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productoModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoModelExists(productoModel.Id))
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
            return View(productoModel);
        }

        // GET: Producto/Delete
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productoModel = await _context.Productos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productoModel == null)
            {
                return NotFound();
            }

            return View(productoModel);
        }

        // POST: Producto/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productoModel = await _context.Productos.FindAsync(id);
            if (productoModel != null)
            {
                _context.Productos.Remove(productoModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoModelExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }
    }
}
