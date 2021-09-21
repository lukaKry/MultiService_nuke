using System.Collections.Generic;
using System;
using lukaKry.Calc.Library.Logic.CalculationsBuilders;

namespace lukaKry.Calc.Library.Logic
{
    public class Calculator
    {
        ICalculation _currentCalculation;
        ICalculationBuilder _builder;

        public ICalculation CurrentCalculation => _currentCalculation;

        public Calculator(ICalculationBuilder builder = null)
        {
            _builder = builder ?? new EquationCalculationBuilder();
        }

        public decimal GetResult()
        {
            _currentCalculation = _builder.Build();
            return _currentCalculation.GetResult();
        }

        public void ResetCurrentCalculation()
        {
            _currentCalculation = null;
        }
    }
}