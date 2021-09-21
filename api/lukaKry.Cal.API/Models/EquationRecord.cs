using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace lukaKry.Calc.API.Models
{
    public class EquationRecord
    {
        public int Id { get; set; }
        public List<decimal> Numbers { get; set; }
        public List<string> Symbols { get; set; }

        [Column(TypeName = "decimal(28,14)")]
        public decimal Result { get; set; }

        [MaxLength(100)]    
        public string Equation { get; set; }
    }
}
