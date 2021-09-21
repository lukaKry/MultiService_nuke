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
    public class PowerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        /* Test cases:
        
            3 ^ 3 = 27
            2 ^ 2 ^ 2 = 16
            no args set = 1

        */

        [Test]
        [TestCase(3, 3, 27)]
        public void GetResult_WithBothArgsSet_CorrectResult(decimal arg1, decimal arg2, decimal outcome)
        {
            Power pow = new()
            {
                Arg1 = new Number(arg1),
                Arg2 = new Number(arg2)
            };

            var result = pow.GetResult();

            Assert.That(result, Is.EqualTo(outcome));
        }

        [Test]
        [TestCase(2, 2, 2, 16)]
        public void GetResult_WithAllArgsSet_CorrectResult(decimal arg1, decimal arg2, decimal arg3, decimal outcome)
        {
            Power pow = new()
            {
                Arg1 = new Number(arg1),
                Arg2 = new Power()
                {
                    Arg1 = new Number(arg2),
                    Arg2 = new Number(arg3)
                }
            };

            var result = pow.GetResult();

            Assert.That(result, Is.EqualTo(outcome));
        }

        [Test]
        public void GetResult_NoArgsSet_GetResultsFromDefaultArgsValues()
        {
            Power pow = new();
            var result = pow.GetResult();
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void GetPriority_WhenCalled_ReturnsThree()
        {
            var powerCalc = new Power();

            var result = powerCalc.Priority;

            Assert.AreEqual(result, 3);
        }

        [Test]
        public void GetSymbol_WhenCalled_ReturnCircumflexString()
        {
            var powerCalc = new Power();

            var result = powerCalc.GetSymbol();

            Assert.AreEqual(result, "^");
        }

        [Test]
        public void ToString_WhenCalled_ReturnsCorrectlyFormatedString()
        {
            var powerCalc = new Power()
            {
                Arg1 = new Number(1),
                Arg2 = new Number(2)
            };

            var result = powerCalc.ToString();

            Assert.AreEqual(result, "1^2");
        }
    }
}
