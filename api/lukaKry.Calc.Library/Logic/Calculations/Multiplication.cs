using lukaKry.Calc.Library.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lukaKry.Calc.Library.Logic.Calculations
{
    class Multiplication : BaseCalculation, ISettableCalculation
    {
        public int Priority => 2;

        public override decimal GetResult()
        {
            return Arg1.GetResult() * Arg2.GetResult();
        }

        public override string ToString()
        {
            return $"{Arg1} * {Arg2}";
        }

        public override string GetSymbol()
        {
            return "*";
        }
    }
}
