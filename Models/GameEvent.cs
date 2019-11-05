using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class GameEvent
    {
        public Int32 Id { get; set; }

        public Int32 EventTypeId { get; set; }

        public EventType EventType { get; set; }

        public ICollection<Player> Players { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Invalid quantity")]
        public int TimeInMinutes { get; set; }

        public Int32 GameProtocolId { get; set; }

        public GameProtocol GameProtocol { get; set; }
    }
}
