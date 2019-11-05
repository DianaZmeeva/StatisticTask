using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = ApplicationRoles.Administrators)]
    public class GameProtocolsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GameProtocolsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GameProtocols
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var items = await _context.GameProtocols
                .Include(p => p.GameEvents)
                .ThenInclude(p => p.EventType)
                .Include(p => p.GameEvents)
                .ThenInclude(p => p.Players)
                .Include(g => g.HomeTeam)
                .Include(g => g.RivalTeam)
                .ToListAsync();
            return this.View(items);
        }

        // GET: GameProtocols/Create
        public IActionResult Create()
        {
            ViewData["HomeTeamId"] = new SelectList(_context.Teams, "Id", "Name");
            ViewData["RivalTeamId"] = new SelectList(_context.Teams, "Id", "Name");
            return View();
        }

        // POST: GameProtocols/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GameProtocolViewModel model )
        {
            if (ModelState.IsValid)
            {

                var gameProtocol = new GameProtocol
                {
                    Time=model.Time,
                    DurationInMinutes=model.DurationInMinutes,
                    HomeTeamId=model.HomeTeamId,
                    RivalTeamId=model.RivalTeamId
                };

                if (model.HomeTeamId == model.RivalTeamId)
                {
                    this.ModelState.AddModelError("RivalTeamId", "Select identical teams");
                    ViewData["HomeTeamId"] = new SelectList(_context.Teams, "Id", "Name", gameProtocol.HomeTeamId);
                    ViewData["RivalTeamId"] = new SelectList(_context.Teams, "Id", "Name", gameProtocol.RivalTeamId);
                    return this.View(model);
                }

                _context.GameProtocols.Add(gameProtocol);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            //ViewData["HomeTeamId"] = new SelectList(_context.Teams, "Id", "Name", gameProtocol.HomeTeamId);
            //ViewData["RivalTeamId"] = new SelectList(_context.Teams, "Id", "Name", gameProtocol.RivalTeamId);
            return this.View(model);
        }

        // GET: GameProtocols/Edit/5
        public async Task<IActionResult> Edit(Int32? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameProtocol = await _context.GameProtocols
                .SingleOrDefaultAsync(m => m.Id == id);
            if (gameProtocol == null)
            {
                return NotFound();
            }
            var model = new GameProtocolViewModel
            {
                Time = gameProtocol.Time,
                DurationInMinutes = gameProtocol.DurationInMinutes,
                HomeTeamId = gameProtocol.HomeTeamId,
                RivalTeamId = gameProtocol.RivalTeamId,
            };

            ViewData["HomeTeamId"] = new SelectList(_context.Teams, "Id", "Name", gameProtocol.HomeTeamId);
            ViewData["RivalTeamId"] = new SelectList(_context.Teams, "Id", "Name", gameProtocol.RivalTeamId);
            return View(model);
        }

        // POST: GameProtocols/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? id, GameProtocolViewModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var gameProtocol = await _context.GameProtocols
                .SingleOrDefaultAsync(m => m.Id == id);
            if (gameProtocol == null)
            {
                return this.NotFound();
            }

            if (model.HomeTeamId == model.RivalTeamId)
            {
                this.ModelState.AddModelError("RivalTeamId", "Select identical teams");
                ViewData["HomeTeamId"] = new SelectList(_context.Teams, "Id", "Name", gameProtocol.HomeTeamId);
                ViewData["RivalTeamId"] = new SelectList(_context.Teams, "Id", "Name", gameProtocol.RivalTeamId);
                return this.View(model);
            }
            if (this.ModelState.IsValid)
            {
                gameProtocol.Time= model.Time;
                gameProtocol.DurationInMinutes = model.DurationInMinutes;
                gameProtocol.HomeTeamId= model.HomeTeamId;
                gameProtocol.RivalTeamId = model.RivalTeamId;

                await _context.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }
            ViewData["HomeTeamId"] = new SelectList(_context.Teams, "Id", "Name", gameProtocol.HomeTeamId);
            ViewData["RivalTeamId"] = new SelectList(_context.Teams, "Id", "Name", gameProtocol.RivalTeamId);
            return this.View(model);
        }

        // GET: GameProtocols/Delete/5
        public async Task<IActionResult> Delete(Int32? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameProtocol = await _context.GameProtocols
                .Include(g => g.HomeTeam)
                .Include(g => g.RivalTeam)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (gameProtocol == null)
            {
                return NotFound();
            }

            return View(gameProtocol);
        }

        // POST: GameProtocols/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32 id)
        {
            var gameProtocol = await _context.GameProtocols
                .SingleOrDefaultAsync(m => m.Id == id);
            _context.GameProtocols.Remove(gameProtocol);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
