using lukaKry.Calc.Library.Logic.CalculationFactories;
using lukaKry.Calc.Library.Logic.Calculations;
using NUnit.Framework;

namespace lukaKry.Calc.Library.UnitTests.CalculationFactoriesTests
{
    public class MultiplicationCalculationFactoryTests
    {
        [Test]
        public void Create_WhenCalled_ReturnMultiplicationObject()
        {
            var factory = new MultiplicationCalculationFactory();

            var result = factory.Create();

            Assert.That(result, Is.TypeOf<Multiplication>());
        }
    }
}
