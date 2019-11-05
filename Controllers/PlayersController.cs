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
    public class PlayersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlayersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Players/Create
        public async Task<IActionResult> Create(Int32? teamId)
        {
            if (teamId == null)
            {
                return this.NotFound();
            }

            var team = await _context.Teams
                .SingleOrDefaultAsync(m => m.Id == teamId);

            if (team == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Team = team;
            return this.View(new PlayerViewModel());
        }

        // POST: Players/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? teamId, PlayerViewModel model)
        {
            if (teamId == null)
            {
                return this.NotFound();
            }

            var team = await _context.Teams
                .SingleOrDefaultAsync(m => m.Id == teamId);
            if (team == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                var player = new Player
                {
                    TeamId = team.Id,
                    FullName = model.FullName,
                    Position = model.Position
                };

                _context.Players.Add(player);
                await _context.SaveChangesAsync();
                return this.RedirectToAction("Index", "Teams");
            }

            this.ViewBag.Team = team;
            return this.View(model);
        }

        // GET: Players/Edit/5
        public async Task<IActionResult> Edit(Int32? id)
        {
            if (id == null)
                if (id == null)
                {
                    return this.NotFound();
                }

            var player = await _context.Players
                .Include(x => x.Team)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return this.NotFound();
            }

            var model = new PlayerViewModel
            {
                FullName = player.FullName,
                Position = player.Position
            };

            this.ViewBag.Team = player.Team;
            return this.View(model);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? id, PlayerViewModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var player = await _context.Players
                .Include(x => x.Team)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                player.FullName = model.FullName;
                player.Position = model.Position;

                await _context.SaveChangesAsync();
                return this.RedirectToAction("Index", "Teams");
            }

            this.ViewBag.Team = player.Team;
            return this.View(model);
        }

        // GET: Players/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .Include(p => p.Team)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _context.Players.SingleOrDefaultAsync(m => m.Id == id);
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
            return this.RedirectToAction("Index", "Teams");
        }
    }
}
