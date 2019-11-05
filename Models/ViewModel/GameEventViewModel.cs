using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models.ViewModel
{
    public class GameEventViewModel
    {
        public Int32 EventTypeId { get; set; }
      
        public ICollection<Int32> Players { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Invalid quantity")]
        public int TimeInMinutes { get; set; }

        public Int32 GameProtocolId { get; set; }
    }
}
