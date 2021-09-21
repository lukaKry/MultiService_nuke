using lukaKry.Calc.Library.Logic;
using lukaKry.Calc.Library.Logic.Calculations;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace lukaKry.Calc.Library.UnitTests
{
    public class SimpleCalculationArchiverTests
    {
        
        
        [Test]
        public void AddCalculation_WhenCalled_StoresCalculation()
        {
            var archiver = new SimpleCalculationArchiver();

            archiver.AddCalculation(new Equation(new List<ISettableCalculation>(), new List<decimal>()));

            var result = archiver.GetLastCalculation();

            Assert.That(result.GetResult(), Is.EqualTo(1));
        }

        [Test]
        public void GetLastCalculation_EmptyArchive_ThrowsInvalidOperationException()
        {
            var archiver = new SimpleCalculationArchiver();

            Assert.That(() => archiver.GetLastCalculation(), Throws.InvalidOperationException);
        }

        [Test]
        public void GetAllCalculations()
        {
            var archiver = new SimpleCalculationArchiver();

            var result = archiver.GetAll();

            Assert.That(result, Is.InstanceOf<IEnumerable<ICalculation>>());
        }
    }
}
