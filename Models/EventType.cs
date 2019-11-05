using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class EventType
    {
        public Int32 Id { get; set; }

        [Required]
        [MaxLength(300)]
        public String Name { get; set; }

        [Required]
        public int NumberOfPlayers { get; set; }

        public Int32 KindOfSportId { get; set; }

        public KindOfSport KindOfSport { get; set; }

    }
}
