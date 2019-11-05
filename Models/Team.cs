using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Team
    {
        public Int32 Id { get; set; }

        [Required]
        [MaxLength(300)]
        public String Name { get; set; }

        [Required]
        public Int32 KindOfSportId { get; set; }

        public KindOfSport KindOfSport { get; set; }

        public ICollection<Player> Players{ get; set; }
    }
}
