using lukaKry.Calc.Library.Logic.CalculationFactories;
using lukaKry.Calc.Library.Logic.Calculations;
using NUnit.Framework;

namespace lukaKry.Calc.Library.UnitTests.CalculationFactoriesTests
{
    public class SubtractionCalculationFactoryTests
    {
        [Test]
        public void Create_WhenCalled_ReturnSubtractionObject()
        {
            var factory = new SubtractionCalculationFactory();

            var result = factory.Create();

            Assert.That(result, Is.TypeOf<Subtraction>());
        }
    }
}