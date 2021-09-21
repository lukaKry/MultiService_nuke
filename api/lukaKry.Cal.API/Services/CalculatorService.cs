using lukaKry.Calc.API.Models;
using lukaKry.Calc.Library.Logic;
using lukaKry.Calc.Library.Logic.Calculations;
using lukaKry.Calc.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lukaKry.Calc.API.Services
{
    public class CalculatorService
    {
        private readonly CalculationsFactoryProvider _provider;

        public CalculatorService(CalculationsFactoryProvider provider)
        {
            _provider = provider;
        }

        public EquationDTO Evaluate(EquationDTO equationDTO)
        {
            List<ISettableCalculation> calcs = new();

            foreach (var symbol in equationDTO.Symbols)
            {
                calcs.Add(_provider[GetCalculationType(symbol)].Create());
            }

            List<decimal> nums = equationDTO.Numbers.ToList();

            var solvedEquation = new Equation(calcs, nums);
            equationDTO.Equation = solvedEquation.ToString();
            equationDTO.Result = solvedEquation.GetResult();

            return equationDTO;
        }

        private CalculationType GetCalculationType(string symbol)
        {
            return symbol switch
            {
                "-" => CalculationType.Subtraction,
                "*" => CalculationType.Multiplication,
                "/" => CalculationType.Division,
                "+" => CalculationType.Sum,
                _ => throw new ArgumentException("at least one of the calculation symbols is invalid"),
            };
        }
    }
}