using lukaKry.Calc.Library.Logic.Calculations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lukaKry.Calc.Library.Logic.CalculationsBuilders
{
    public class EquationCalculationBuilder : ICalculationBuilder
    {
        private static readonly CalculationsFactoryProvider _provider = new();
        
        private List<decimal> _numbers = new() { 0 };
        private List<ISettableCalculation> _calculations = new() { new Sum() };

        public void AddCalculation(CalculationType calcType)
        {
            var calculation = _provider[calcType].Create();

            if (_numbers.Count > _calculations.Count)
            {
                _calculations.Add(calculation);
            }
            else
            {
                _calculations[^1] = calculation;
            }
        }

        public void AddNumber(decimal number)
        {
            if (_numbers.Count > _calculations.Count)
            {
                _numbers[^1] = number;
            }
            else
            {
                _numbers.Add(number);
            }
        }

        public Equation Build()
        {
            // if there is less numbers than needed duplicate last number 
            if (_numbers.Count == _calculations.Count) _numbers.Add(_numbers[^1]);

            return new Equation(_calculations, _numbers);
        }

        public void Reset()
        {
            _numbers = new List<decimal>() { 0 };
            _calculations = new List<ISettableCalculation>() { new Sum() };
        }

        public override string ToString()
        {
            if (_numbers.Count == 1) return "0";

            StringBuilder sb = new();

            for (int i = 0; i < _calculations.Count ; i++)
            {
                sb.Append(_numbers[i] + " ");
                sb.Append(_calculations[i].GetSymbol() + " ");
            }

            if( _calculations.Count < _numbers.Count ) sb.Append(_numbers[^1]);

            sb.Remove(0, 4);

            return sb.ToString();
        }
    }
}
