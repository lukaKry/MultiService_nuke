using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lukaKry.Calc.Library.Interfaces;
using lukaKry.Calc.Library.Logic;
using NUnit.Framework;


namespace lukaKry.Calc.Library.UnitTests.Providers
{
    public class CalculationArchiverProviderTests
    {
        [Test]
        public void GetArchiver_WhenCalled_ReturnsIRegistryObject()
        {
            var provider  = new CalculationArchiverProvider();

            var result = provider.GetArchiver();

            Assert.That(result, Is.InstanceOf<IRegistry>());
        }
    }
}
