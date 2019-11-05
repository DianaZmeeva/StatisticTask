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
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = ApplicationRoles.Administrators)]
    public class GameEventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GameEventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GameEvents/Create
        public async Task<IActionResult> Create(Int32? protocolId)
        {
            if (protocolId == null)
            {
                return this.NotFound();
            }

            var gameProtocol = await _context.GameProtocols
                .SingleOrDefaultAsync(m => m.Id == protocolId);

            if (gameProtocol == null)
            {
                return this.NotFound();
            }

            var homeTeam = await _context.Teams
                .SingleOrDefaultAsync(m => m.Id == gameProtocol.HomeTeamId);

            var rivialTeam = await _context.Teams
                .SingleOrDefaultAsync(m => m.Id == gameProtocol.RivalTeamId);

            if (homeTeam == null || rivialTeam==null)
            {
                return this.NoContent();
            }

            var allPlayers= from m in _context.Players
                            select m;

            var gamePlayers= allPlayers
                .Where(m => m.TeamId == homeTeam.Id || m.TeamId==rivialTeam.Id).ToArray();

            if (gamePlayers == null)
            {
                return this.NoContent();
            }

            var allEvents = from m in _context.EventTypes
                         select m;

            var gameEvents = allEvents
                .Where(m => m.KindOfSportId == homeTeam.KindOfSportId);

            if (gameEvents== null)
            {
                return this.NoContent();
            }

            this.ViewBag.Protocol = gameProtocol;
            ViewData["EventTypeId"] = new SelectList(gameEvents, "Id", "Name");
            ViewBag.PlayersList = gamePlayers;
            //ViewBag.Playerslist = new MultiSelectList(gamePlayers, "ID", "FullName", null);
            return this.View(new GameEventViewModel());
        }

        // POST: GameEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? protocolId, GameEventViewModel model)
        {
            if (protocolId == null)
            {
                return this.NotFound();
            }

            var gameProtocol = await _context.GameProtocols
                .SingleOrDefaultAsync(m => m.Id == protocolId);
            if (gameProtocol == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                
                var gameEvent = new GameEvent
                {
                    GameProtocolId = gameProtocol.Id,
                    TimeInMinutes = model.TimeInMinutes,
                    EventTypeId = model.EventTypeId
                };

                var eventType = await _context.EventTypes
                .SingleOrDefaultAsync(m => m.Id == gameEvent.EventTypeId);

                if(model.Players.Count != eventType.NumberOfPlayers)
                {
                    this.ModelState.AddModelError("Players", "Another number of players");
                    return this.View(model);
                }

                if (model.TimeInMinutes > gameProtocol.DurationInMinutes)
                {
                    this.ModelState.AddModelError("TimeInMinutes", "Required time");
                    return this.View(model);
                }

                gameEvent.Players = new Collection<Player>();
                foreach (Int32 playerId in model.Players)
                {
                    var player = await _context.Players
                    .SingleOrDefaultAsync(m => m.Id == playerId);
                    if (player != null)
                    {
                        gameEvent.Players.Add(player);
                    }
                }

                _context.GameEvens.Add(gameEvent);
                await _context.SaveChangesAsync();
                return this.RedirectToAction("Index", "GameProtocols");
            }

            this.ViewBag.Protocol = gameProtocol;
            return this.View(model);
        }
        // GET: GameEvents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameEvent = await _context.GameEvens
                .Include(g => g.EventType)
                .Include(g => g.GameProtocol)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (gameEvent == null)
            {
                return NotFound();
            }

            return View(gameEvent);
        }

        // POST: GameEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gameEvent = await _context.GameEvens.SingleOrDefaultAsync(m => m.Id == id);
            _context.GameEvens.Remove(gameEvent);
            await _context.SaveChangesAsync();
            return this.RedirectToAction("Index", "GameProtocols");
        }

    }
}
