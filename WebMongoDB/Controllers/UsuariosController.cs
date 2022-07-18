using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using WebMongoDB.Data;
using WebMongoDB.Models;

namespace WebMongoDB.Controllers
{
    public class UsuariosController : Controller
    {
        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            ContextMongoDb dbContext = new ContextMongoDb();
            return View(await dbContext.Usuario.Find(u => true).ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ContextMongoDb dbContext = new ContextMongoDb();
            var usuario = await  dbContext.Usuario.Find(u => u.Id == id).FirstOrDefaultAsync();

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Ean")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                ContextMongoDb dbContext = new ContextMongoDb();
                usuario.Id = Guid.NewGuid();
                await dbContext.Usuario.InsertOneAsync(usuario);

                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ContextMongoDb dbContext = new ContextMongoDb();
            var usuario = await dbContext.Usuario.Find(u => u.Id == id).FirstOrDefaultAsync();
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Nome,Ean")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ContextMongoDb dbContext = new ContextMongoDb();
                    await dbContext.Usuario.ReplaceOneAsync(m => m.Id == usuario.Id, usuario);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
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
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ContextMongoDb dbContext = new ContextMongoDb();
            var usuario = await dbContext.Usuario.Find(u => u.Id == id).FirstOrDefaultAsync();
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            ContextMongoDb dbContext = new ContextMongoDb();

            await dbContext.Usuario.DeleteOneAsync(u => u.Id == id);
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(Guid id)
        {
            ContextMongoDb dbContext = new ContextMongoDb();
            var usuario = dbContext.Usuario.Find(u => u.Id == id).Any();

            return usuario;
        }
    }
}
