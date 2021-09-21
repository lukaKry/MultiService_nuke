using lukaKry.Calc.Library.Logic.Calculations;
using lukaKry.Calc.Library.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lukaKry.Calc.Library.Logic.CalculationFactories
{
    public class DivisionCalculationFactory : ICalculationFactory
    {
        public ISettableCalculation Create()
        {
            return new Division();
        }
    }
}
