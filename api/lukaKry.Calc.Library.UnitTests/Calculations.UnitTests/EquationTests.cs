using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lukaKry.Calc.Library.Logic;
using lukaKry.Calc.Library.Logic.Calculations;
using NUnit.Framework;

namespace lukaKry.Calc.Library.UnitTests.Calculations.UnitTests
{
    public class EquationTests
    {
        [Test]
        public void Constuctor_CorrectParameters_CreatesObjectAndSetsResult()
        {
            var calcs = new List<ISettableCalculation> { new Sum() };
            var nums = new List<decimal> { 1, 1 };
            Equation equation = new(calcs, nums);

            var result = equation.GetResult();

            Assert.That(result, Is.EqualTo(2));
            Assert.That(equation.ToString(), Is.EqualTo("1 + 1 = 2"));
        }

        [Test]
        public void Constructor_EmptyListsAsArguments_ThrowsArgumentException()
        {
            var calcs = new List<ISettableCalculation>();
            var nums = new List<decimal>();

            Assert.That(() => { Equation equation = new(calcs, nums); }, Throws.ArgumentException);   
        }

        [Test]
        public void ToString_WhenCalled_ReturnsCorrectlyFormatedString()
        {
            var calcs = new List<ISettableCalculation> { new Sum(), new Multiplication(), new Power() };
            var nums = new List<decimal> { 1, 2, 3, 4};
            Equation equation = new(calcs, nums);

            Assert.That(equation.ToString(), Is.EqualTo("1 + 2 * 3 ^ 4 = 163"));
        }

        [Test]
        public void GetSymbol_WhenCalled_ReturnsEqualSign()
        {
            var calcs = new List<ISettableCalculation> { new Sum() };
            var nums = new List<decimal> { 1, 1 };
            Equation equation = new(calcs, nums);

            var result =  equation.GetSymbol();

            Assert.That(result, Is.EqualTo("="));
        }

        [Test]
        public void GetResult_WhenCalled_ReturnsResult()
        {
            var calcs = new List<ISettableCalculation> { new Sum() };
            var nums = new List<decimal> { 1, 1 };
            Equation equation = new(calcs, nums);

            var result = equation.GetResult();

            Assert.That(result, Is.EqualTo(2));
        }
    }
}
