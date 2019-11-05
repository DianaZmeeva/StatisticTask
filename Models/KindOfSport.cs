using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class KindOfSport
    {
        public Int32 Id { get; set; }

        [Required]
        [MaxLength(300)]
        public String Name { get; set; }

        public ICollection<EventType> EventTypes { get; set; }
    }
}
