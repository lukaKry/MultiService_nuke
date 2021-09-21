using lukaKry.Calc.Library.Logic.CalculationFactories;
using lukaKry.Calc.Library.Logic.Calculations;
using NUnit.Framework;

namespace lukaKry.Calc.Library.UnitTests.CalculationFactoriesTests
{
    public class SumCalculationFactoryTests
    {
        [Test]
        public void Create_WhenCalled_ReturnSumObject()
        {
            var factory = new SumCalculationFactory();

            var result = factory.Create();

            Assert.That(result, Is.TypeOf<Sum>());
        }
    }
}