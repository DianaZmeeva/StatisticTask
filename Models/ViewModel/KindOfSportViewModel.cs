using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models.ViewModels
{
    public class KindOfSportViewModel
    {
        [Required]
        [MaxLength(300)]
        public String Name { get; set; }
    }
}
