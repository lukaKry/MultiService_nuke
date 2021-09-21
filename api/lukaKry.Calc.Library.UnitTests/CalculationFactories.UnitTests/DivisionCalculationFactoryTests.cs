using lukaKry.Calc.Library.Logic.CalculationFactories;
using lukaKry.Calc.Library.Logic.Calculations;
using NUnit.Framework;

namespace lukaKry.Calc.Library.UnitTests.CalculationFactoriesTests
{
    public class DivisionCalculationFactoryTests
    {
        [Test]
        public void Create_WhenCalled_ReturnDivisionObject()
        {
            var factory = new DivisionCalculationFactory();

            var result = factory.Create();

            Assert.That(result, Is.TypeOf<Division>());
        }
    }
}
