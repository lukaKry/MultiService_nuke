using NUnit.Framework;
using System;
using lukaKry.Calc.Library.Logic;
using lukaKry.Calc.Library.Logic.Calculations;

namespace lukaKry.Calc.Library.UnitTests
{
    class DivisionTests
    {
        [SetUp]
        public void Setup()
        {
        }

        /* Test cases:
        
            2 / 2 = 1
            2 / 2 / 2 = 0.5
            -2 * (-2) * (-2) = -0.5
            
            2 / 0 = throws exception

            no args set = 0
            
        */

        [Test]
        public void SetGetArg1_SetNumberDifferentThanZero_SetsAndGetsSameValue()
        {
            var divisionCalc = new Division();
            var num = new Number(1);

            divisionCalc.Arg1 = num;
            var result = divisionCalc.Arg1;

            Assert.That(result, Is.EqualTo(num));
        }

        [Test]
        public void SetArg2_SetNumberWithArgZero_ThrowsDivideByZeroException()
        {
            var divisionCalc = new Division();
            var num = new Number(0);

            Assert.That( () =>  divisionCalc.Arg2 = num , Throws.Exception.TypeOf<DivideByZeroException>());
        }

        [Test]
        public void SetGetArg2_SetNumberDifferentThanZero_SetsAndGetsSameValue()
        {
            var divisionCalc = new Division();
            var num = new Number(1);

            divisionCalc.Arg2 = num;
            var result = divisionCalc.Arg2;

            Assert.AreEqual(result, num);
        }

        [Test]
        public void GetPriority_WhenCalled_ReturnsTwo()
        {
            var divisionCalc = new Division();

            var result = divisionCalc.Priority;

            Assert.AreEqual(result, 2);
        }

        [Test]
        [TestCase(2, 2, 1)]
        public void GetResult_WithBothArgsSet_CorrectResult(decimal arg1, decimal arg2, decimal outcome)
        {
            Division division = new()
            {
                Arg1 = new Number(arg1),
                Arg2 = new Number(arg2)
            };

            var result = division.GetResult();

            Assert.That(result, Is.EqualTo(outcome));
        }

        [Test]
        [TestCase(2, 2, 2, 0.5)]
        [TestCase(-2, -2, -2, -0.5)]
        public void GetResult_WithAllArgsSet_CorrectResult(decimal arg1, decimal arg2, decimal arg3, decimal outcome)
        {
            Division division = new()
            {
                Arg2 = new Number(arg1),
                Arg1 = new Division()
                {
                    Arg2 = new Number(arg2),
                    Arg1 = new Number(arg3)
                }
            };

            var result = division.GetResult();

            Assert.That(result, Is.EqualTo(outcome));
        }

        [Test]
        [TestCase(2, 0)]
        public void GetResult_DivisorIsZero_ThrowException(decimal arg1, decimal arg2)
        {

            Assert.That( () =>
            {
                Division division = new()
                {
                    Arg1 = new Number(arg1),
                    Arg2 = new Number(arg2)
                };
            },
            Throws.Exception.TypeOf<DivideByZeroException>() );
        }

        [Test]
        public void GetResult_DivisorIsCalculationWhichResultsInZero_ThrowException()
        {

            Assert.That(() =>
            {
                Division division = new()
                {
                    Arg1 = new Number(2),
                    Arg2 = new Subtraction()
                    {
                        Arg1 = new Number(2),
                        Arg2 = new Number(2)
                    }
                };

                var result = division.GetResult();
            },
            Throws.Exception.TypeOf<DivideByZeroException>());
        }

        [Test]
        public void GetResult_NoArgsSet_GetResultsFromDefaultArgsValues()
        {
            Division division= new();
            var result = division.GetResult();
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void GetSymbol_WhenCalled_ReturnForeslashString()
        {
            var divisionCalc = new Division();

            var result = divisionCalc.GetSymbol();

            Assert.AreEqual(result, "/");
        }

        [Test]
        public void ToString_WhenCalled_ReturnsCorrectlyFormatedString()
        {
            var divisionCalc = new Division()
            {
                Arg1 = new Number(1),
                Arg2 = new Number(2)
            };

            var result = divisionCalc.ToString();

            Assert.AreEqual(result, "1 / 2");
        }
    }
}
