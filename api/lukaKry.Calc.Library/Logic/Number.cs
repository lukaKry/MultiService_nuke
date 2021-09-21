using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lukaKry.Calc.Library.Logic
{
    internal class Number : ICalculation
    {
        private decimal _arg1;
        public ICalculation Arg1 { get; set; }

        public Number(decimal arg)
        {
            _arg1 = arg;
        }

        public decimal GetResult()
        {
            return _arg1;
        }

        public override string ToString()
        {
            return _arg1.ToString();
        }

        public string GetSymbol()
        {
            return "";
        }
    }
}
