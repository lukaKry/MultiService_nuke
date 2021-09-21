using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace lukaKry.Calc.Library.Logic.Calculations
{
    public class Equation : ICalculation
    {
        private readonly List<ISettableCalculation> _calcs;
        private readonly List<decimal> _nums;
        private decimal _result;

        [JsonPropertyName("calcs")]
        public IEnumerable<ISettableCalculation> Calcs => _calcs;

        [JsonPropertyName("nums")]
        public IEnumerable<decimal> Nums => _nums;
        public decimal GetResult() => _result;
        public string GetSymbol() => "=";


        public Equation(List<ISettableCalculation> calcs, List<decimal> nums)
        {
            if (CheckIfInputDataIsCorrect(calcs, nums))
                throw new ArgumentException("Parameters must not be empty and must fullfill condition: calcs + 1 = nums");

            _calcs = calcs;
            _nums = nums;

           if(_result == 0) AssignResult();
        }

        private static bool CheckIfInputDataIsCorrect(List<ISettableCalculation> calcs, List<decimal> nums)
        {
            return calcs.Count + 1 != nums.Count && !calcs.Any();
        }

        private void AssignResult()
        {
            List<decimal> copiedNumbers = _nums.GetRange(0, _nums.Count);
            List<ISettableCalculation> copiedCalculations = _calcs.GetRange(0, _calcs.Count);

            _result = Solve(copiedCalculations, copiedNumbers);
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            for (int i = 0; i < _calcs.Count(); i++)
            {
                sb.Append(_nums[i] + " ");
                sb.Append(_calcs[i].GetSymbol() + " ");
            }

            if (_calcs.Count() < _nums.Count()) sb.Append(_nums[^1]);


            return sb.ToString();
        }

        private decimal Solve(List<ISettableCalculation> calcs, List<decimal> nums)
        {
            // 1. Find the highest priority among all calculations in the equation
            // 2. Choose the first calculation with the found priority and get its index
            // 3. Fill corresponding arguments on suitable spots
            // 4. Remove calculated calculation from the calculations list
            // 5. Remove used arguments from the numbers list
            // 6. Save the result at the first argument spot
            // 7. Call function recursively until only one number is left (the result)

            if (!calcs.Any()) return nums[0];

            var highestPriority = calcs.Max(p => p.Priority);
            var calculationToBeSolved = calcs.FirstOrDefault(p => p.Priority == highestPriority);
            var indexOfCalculationToBeSolved = calcs.IndexOf(calculationToBeSolved);

            calculationToBeSolved.Arg1 = new Number(nums[indexOfCalculationToBeSolved]);
            calculationToBeSolved.Arg2 = new Number(nums[indexOfCalculationToBeSolved + 1]);

            calcs.RemoveAt(indexOfCalculationToBeSolved);
            nums.RemoveAt(indexOfCalculationToBeSolved + 1);
            nums[indexOfCalculationToBeSolved] = calculationToBeSolved.GetResult();

            return Solve(calcs, nums);
        }
    }
}
