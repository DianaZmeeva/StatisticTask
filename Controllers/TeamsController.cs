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
    public class TeamsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeamsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Teams
        public async Task<IActionResult> Index()
        {
            var items = await _context.Teams
              .Include(p => p.Players)
              .Include(t => t.KindOfSport)
              .ToListAsync();
            return this.View(items);

            //var applicationDbContext = _context.Teams.Include(t => t.KindOfSport);
            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: Teams/Create
        public IActionResult Create()
        {
            ViewData["KindOfSportId"] = new SelectList(_context.KindOfSports, "Id", "Name");
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamViewModel model)
        {
            if (ModelState.IsValid)
            {
                var team = new Team
                {
                    Name = model.Name,
                    KindOfSportId = model.KindOfSportId
                };

                _context.Teams.Add(team);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return this.View(model);
        }

        // GET: Teams/Edit/5
        public async Task<IActionResult> Edit(Int32? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .SingleOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }
            var model = new TeamViewModel
            {
                Name = team.Name,
                KindOfSportId = team.KindOfSportId
            };
            ViewData["KindOfSportId"] = new SelectList(_context.KindOfSports, "Id", "Name", team.KindOfSportId);
            return View(model);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? id, TeamViewModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var team = await _context.Teams
                .SingleOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                team.Name = model.Name;
                team.KindOfSportId = model.KindOfSportId;

                await _context.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }
            ViewData["KindOfSportId"] = new SelectList(_context.KindOfSports, "Id", "Name", team.KindOfSportId);
            return this.View(model);
        }

        // GET: Teams/Delete/5
        public async Task<IActionResult> Delete(Int32? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .Include(t => t.KindOfSport)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32 id)
        {
            var team = await _context.Teams.SingleOrDefaultAsync(m => m.Id == id);
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
