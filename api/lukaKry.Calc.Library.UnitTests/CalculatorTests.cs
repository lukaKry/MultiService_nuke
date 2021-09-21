using lukaKry.Calc.Library.Logic;
using lukaKry.Calc.Library.Logic.Calculations;
using lukaKry.Calc.Library.Logic.CalculationsBuilders;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace lukaKry.Calc.Library.UnitTests
{
    class CalculatorTests
    {
        private CalculationsFactoryProvider _provider = new();
        private Calculator _calc;
        private EquationCalculationBuilder _builder = new();

        [SetUp]
        public void SetUp()
        {
            _calc = new Calculator(_builder);
        }

        [Test]
        public void ResetCurrentCalculation_WhenCalled_SetFieldToNull()
        {
            _calc.ResetCurrentCalculation();

            var result = _calc.CurrentCalculation;

            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetResult_CurrentCalculationNotNull_ReturnsDecimalValue()
        {
            var builder = new Mock<ICalculationBuilder>();
            builder.Setup(b => b.Build()).Returns(new Equation(new List<ISettableCalculation>(), new List<decimal>()));

            _calc = new Calculator(builder.Object);

            var result = _calc.GetResult();

            Assert.That(result, Is.TypeOf<decimal>());
        }

    }
}
