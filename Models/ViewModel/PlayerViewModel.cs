using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models.ViewModels
{
    public class PlayerViewModel
    {
        [Required]
        [MaxLength(300)]
        public String FullName { get; set; }

        [MaxLength(300)]
        public String Position { get; set; }

        public Int32 TeamId { get; set; }
    }
}
