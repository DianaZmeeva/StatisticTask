using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models.ViewModel
{
    public class GameProtocolViewModel
    {
        [Required]
        public DateTime Time { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Invalid quantity")]
        public int DurationInMinutes { get; set; }

        public Int32 HomeTeamId { get; set; }

        public Int32 RivalTeamId { get; set; }

    }
}
