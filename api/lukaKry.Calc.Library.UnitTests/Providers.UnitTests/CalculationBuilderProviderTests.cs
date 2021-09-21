using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lukaKry.Calc.Library.Logic;
using lukaKry.Calc.Library.Logic.CalculationsBuilders;
using NUnit.Framework;


namespace lukaKry.Calc.Library.UnitTests.Providers.UnitTests
{
    public class CalculationBuilderProviderTests
    {
        [Test]
        [TestCase(BuilderMode.simple, typeof(EquationCalculationBuilder))]
        [TestCase(BuilderMode.extended, typeof(EquationCalculationBuilder))]
        [TestCase(BuilderMode.advanced, typeof(EquationCalculationBuilder))]

        public void BuilderProviderIndexer_WhenCalled_ReturnsDesiredBuilder(BuilderMode mode, Type type)
        {
            var provider = new CalculationBuilderProvider();

            var result = provider[mode]().GetType();

            Assert.That(result, Is.EqualTo(type));
        }
    }
}
