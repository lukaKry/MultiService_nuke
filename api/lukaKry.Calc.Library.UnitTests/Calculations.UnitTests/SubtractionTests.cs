using lukaKry.Calc.Library.Logic;
using lukaKry.Calc.Library.Logic.Calculations;
using NUnit.Framework;


namespace lukaKry.Calc.Library.UnitTests
{
    class SubtractionTests
    {
        [SetUp]
        public void Setup()
        {
        }


        /* Test cases:
        
            2 - 2 = 0
            2 - 2 - 2 = -2
            -2 - (-2) - (-2) = 2

            no args set = 0

        */

        [Test]
        [TestCase(0,2,-2)]
        public void GetResult_WithBothArgsSet_CorrectResult(decimal arg1, decimal arg2, decimal outcome)
        {
            Subtraction subtraction = new()
            {
                Arg1 = new Number(arg1),
                Arg2 = new Number(arg2)
            };

            var result = subtraction.GetResult();

            Assert.That(result, Is.EqualTo(outcome));
        }

        [Test]
        [TestCase(2, 2, 2, -2)]
        [TestCase(-2, -2, -2, 2)]
        public void GetResult_WithAllArgsSet_CorrectResult(decimal arg1, decimal arg2, decimal arg3, decimal outcome)
        {
            Subtraction subtraction = new()
            {
                Arg2 = new Number(arg1),
                Arg1 = new Subtraction()
                {
                    Arg2 = new Number(arg2),
                    Arg1 = new Number(arg3)
                }
            };

            var result = subtraction.GetResult();

            Assert.That(result, Is.EqualTo(outcome));
        }

        [Test]
        public void GetResult_NoArgsSet_GetResultsFromDefaultArgsValues()
        {
            Subtraction subtraction = new();
            var result = subtraction.GetResult();
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void GetPriority_WhenCalled_ReturnsTwo()
        {
            var subCalc = new Subtraction();

            var result = subCalc.Priority;

            Assert.AreEqual(result, 1);
        }

        [Test]
        public void GetSymbol_WhenCalled_ReturnForeslashString()
        {
            var subCalc = new Subtraction();

            var result = subCalc.GetSymbol();

            Assert.AreEqual(result, "-");
        }

        [Test]
        public void ToString_WhenCalled_ReturnsCorrectlyFormatedString()
        {
            var subCalc = new Subtraction()
            {
                Arg1 = new Number(1),
                Arg2 = new Number(2)
            };

            var result = subCalc.ToString();

            Assert.AreEqual(result, "1 - 2");
        }
    }
}
