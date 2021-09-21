using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lukaKry.Calc.Library.Logic.Calculations
{
    public class BaseCalculation : ISettableCalculation
    {
        public ICalculation Arg1 { get; set; }
        public ICalculation Arg2 { get; set; }

        public BaseCalculation(decimal arg1 = 0, decimal arg2 = 0)
        {
            Arg1 = new Number(arg1);
            Arg2 = new Number(arg2);
        }

        public virtual decimal GetResult()
        {
            return 0;
        }

        public virtual string GetSymbol()
        {
            return string.Empty;
        }
    }
}
