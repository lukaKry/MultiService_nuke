using lukaKry.Calc.Library.Logic;
using NUnit.Framework;

namespace lukaKry.Calc.Library.UnitTests
{
    class NumberTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetResult__CorrectResult()
        {
            Number number = new(1);

            var result = number.GetResult();

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ToString__CorrectValue()
        {
            Number num = new(1);

            var result = num.ToString();

            Assert.That(result, Is.EqualTo("1"));
        }

        [Test]
        public void GetSymbol__EmptyString()
        {
            Number num = new Number(1);

            var result = num.GetSymbol();

            Assert.That(result, Is.Empty);
        }
    }
}

