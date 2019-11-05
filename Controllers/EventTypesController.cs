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

namespace WebApplication1.Controllers
{
    public class EventTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EventTypes/Create
        public async Task<IActionResult> Create(Int32? sportId)
        {
            if (sportId == null)
            {
                return this.NotFound();
            }

            var kindOfSport = await _context.KindOfSports
                .SingleOrDefaultAsync(m => m.Id == sportId);

            if (kindOfSport == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Sport = kindOfSport;
            return this.View(new EventTypeViewModel());
        }

        // POST: EventTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? sportId, EventTypeViewModel model)
        {
            if (sportId == null)
            {
                return this.NotFound();
            }

            var kindOfSport = await _context.KindOfSports
                .SingleOrDefaultAsync(m => m.Id == sportId);
            if (kindOfSport == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                var eventType = new EventType
                {
                    KindOfSportId = kindOfSport.Id,
                    Name = model.Name,
                    NumberOfPlayers = model.NumberOfPlayers
                };

                _context.EventTypes.Add(eventType);
                await _context.SaveChangesAsync();
                return this.RedirectToAction("Index", "KindOfSports");
            }

            this.ViewBag.Sport = kindOfSport;
            return this.View(model);
        }

        // GET: EventTypes/Edit/5
        public async Task<IActionResult> Edit(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var eventType = await _context.EventTypes
                .Include(x => x.KindOfSport)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (eventType == null)
            {
                return this.NotFound();
            }

            var model = new EventTypeViewModel
            {
                Name = eventType.Name,
                NumberOfPlayers = eventType.NumberOfPlayers
            };

            this.ViewBag.Sport = eventType.KindOfSport;
            return this.View(model);
        }

        // POST: EventTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? id, EventTypeViewModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var eventType = await _context.EventTypes
                .Include(x => x.KindOfSport)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (eventType == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                eventType.Name = model.Name;
                eventType.NumberOfPlayers = model.NumberOfPlayers;

                await _context.SaveChangesAsync();
                return this.RedirectToAction("Index", "KindOfSports");
            }

            this.ViewBag.Sport = eventType.KindOfSport;
            return this.View(model);
        }

        // GET: EventTypes/Delete/5
        public async Task<IActionResult> Delete(Int32? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventType = await _context.EventTypes
                .Include(f => f.KindOfSport)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (eventType == null)
            {
                return NotFound();
            }

            return View(eventType);
        }

        // POST: EventTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32 id)
        {
            var eventType = await _context.EventTypes
                .SingleOrDefaultAsync(m => m.Id == id);
            _context.EventTypes.Remove(eventType);
            await _context.SaveChangesAsync();
            return this.RedirectToAction("Index", "KindOfSports");
        }
    }
}
