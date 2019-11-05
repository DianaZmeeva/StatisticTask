using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = ApplicationRoles.Administrators)]
    public class KindOfSportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KindOfSportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: KindOfSports
        public async Task<IActionResult> Index()
        {
            var items = await _context.KindOfSports
                .Include(p => p.EventTypes)
                .ToListAsync();
            return this.View(items);
        }


        // GET: KindOfSports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KindOfSports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KindOfSportViewModel model)
        {
            if (ModelState.IsValid)
            {
                var kindOfSport = new KindOfSport
                {
                    Name = model.Name
                };


                _context.KindOfSports.Add(kindOfSport);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return this.View(model);
        }

        // GET: KindOfSports/Edit/5
        public async Task<IActionResult> Edit(Int32? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kindOfSport = await _context.KindOfSports
                .SingleOrDefaultAsync(m => m.Id == id);
            if (kindOfSport == null)
            {
                return NotFound();
            }
            var model = new KindOfSportViewModel
            {
                Name = kindOfSport.Name,
            };

            return View(model);

        }

        // POST: KindOfSports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? id, KindOfSportViewModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var kindOfSport = await _context.KindOfSports
                .SingleOrDefaultAsync(m => m.Id == id);
            if (kindOfSport == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                kindOfSport.Name = model.Name;

                await _context.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }

            return this.View(model);
        }

        // GET: KindOfSports/Delete/5
        public async Task<IActionResult> Delete(Int32? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kindOfSport = await _context.KindOfSports
                .SingleOrDefaultAsync(m => m.Id == id);
            if (kindOfSport == null)
            {
                return NotFound();
            }

            return View(kindOfSport);

        }

        // POST: KindOfSports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32 id)
        {
            var kindOfSport = await _context.KindOfSports
                .SingleOrDefaultAsync(m => m.Id == id);
            _context.KindOfSports.Remove(kindOfSport);
            await _context.SaveChangesAsync();
            return this.RedirectToAction("Index");
        }
    }
}
