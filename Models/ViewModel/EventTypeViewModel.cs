using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models.ViewModels
{
    public class EventTypeViewModel
    {
        [Required]
        [MaxLength(300)]
        public String Name { get; set; }

        [Required]
        [Range(0, 50, ErrorMessage = "Invalid quantity")]
        public int NumberOfPlayers { get; set; }

        public Int32 KindOfSportId { get; set; }
    }
}
