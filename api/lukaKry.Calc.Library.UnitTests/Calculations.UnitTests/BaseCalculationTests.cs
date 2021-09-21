using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lukaKry.Calc.Library.Logic;
using lukaKry.Calc.Library.Logic.Calculations;
using NUnit.Framework;

namespace lukaKry.Calc.Library.UnitTests.Calculations.UnitTests
{
    public class BaseCalculationTests
    {
        [Test]
        public void SetGet_WhenCalled_SetsAndGetsSameObject()
        {
            BaseCalculation baseCalc = new BaseCalculation();
            var num = new Number(1);

            baseCalc.Arg1 = num;
            var result = baseCalc.Arg1;

            Assert.That(result, Is.EqualTo(num));
        }

        [Test]
        public void Constructor_WhenCalled_CreateNewObjectWithArgumentsSetToNumberWihtArgumentZero()
        {
            var result = new BaseCalculation();

            Assert.That(result.Arg1 is Number && result.Arg1.GetResult() == 0);
            Assert.That(result.Arg2 is Number && result.Arg2.GetResult() == 0);
        }
    }
}
