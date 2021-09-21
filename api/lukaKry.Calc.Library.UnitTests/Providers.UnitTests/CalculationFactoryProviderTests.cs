using lukaKry.Calc.Library.Logic;
using lukaKry.Calc.Library.Logic.CalculationFactories;
using lukaKry.Calc.Library.Logic.Calculations;
using NUnit.Framework;
using System;

namespace lukaKry.Calc.Library.UnitTests.CalculationFactoriesTests
{
    public class CalculationFactoryProviderTests
    {
        [Test]
        [TestCase(CalculationType.Sum, typeof(SumCalculationFactory))]
        [TestCase(CalculationType.Subtraction, typeof(SubtractionCalculationFactory))]
        [TestCase(CalculationType.Multiplication, typeof(MultiplicationCalculationFactory))]
        [TestCase(CalculationType.Division, typeof(DivisionCalculationFactory))]
        public void BuilderProviderIndexer_WhenCalled_ReturnsDesiredBuilder(CalculationType calcType, Type type)
        {
            var provider = new CalculationsFactoryProvider();

            var result = provider[calcType].GetType();

            Assert.That(result, Is.EqualTo(type));
        }
    }
}