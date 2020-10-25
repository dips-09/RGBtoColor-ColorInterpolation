using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HW4MVC.Models
{
    public class ColorInterpolation
    {
        [Required]
        public string FirstColor { get; set; }
        [Required]
        public string SecondColor { get; set; }
        [Required]
        public int NumberOfColors { get; set; }
        public List<string> ColorList { get; set; }
    }
}
