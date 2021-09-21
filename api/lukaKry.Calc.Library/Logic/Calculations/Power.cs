using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lukaKry.Calc.Library.Logic.Calculations
{
    public class Power : BaseCalculation, ISettableCalculation
    {
        public int Priority => 3;

        public override decimal GetResult()
        {
            return (decimal)Math.Pow((double)Arg1.GetResult(), (double)Arg2.GetResult());
        }

        public override string ToString()
        {
            return $"{Arg1}^{Arg2}";
        }

        public override string GetSymbol()
        {
            return "^";
        }
    }
}
