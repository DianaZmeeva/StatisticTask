using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Player
    {
        public Int32 Id { get; set; }

        [Required]
        [MaxLength(300)]
        public String FullName { get; set; }

        [MaxLength(300)]
        public String Position{ get; set; }

        public Int32 TeamId { get; set; }

        public Team Team{ get; set; }
    }
}
