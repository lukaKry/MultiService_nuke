using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lukaKry.Calc.Library.Models
{
    public class EquationDTO
    {
        public int Id { get; set; }
        public decimal[] Numbers { get; set; }
        public string[] Symbols { get; set; }
        public string Equation { get; set; }
        public decimal Result { get; set; }
    }
}
