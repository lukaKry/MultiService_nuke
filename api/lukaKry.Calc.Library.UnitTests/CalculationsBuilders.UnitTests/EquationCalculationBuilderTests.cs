using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lukaKry.Calc.Library.Logic;
using lukaKry.Calc.Library.Logic.Calculations;
using lukaKry.Calc.Library.Logic.CalculationsBuilders;
using NUnit.Framework;

namespace lukaKry.Calc.Library.UnitTests.CalculationsBuilders.UnitTests
{
    public class EquationCalculationBuilderTests
    {
        [Test]
        public void AddNum_FirstNum_AddsNumberToBuilder()
        {
            var builder = new EquationCalculationBuilder();

            builder.AddNumber(1);

            var result = builder.ToString();

            Assert.That(result, Does.EndWith("1"));
        }

        [Test]
        public void AddNum_NextNumberWithoutNewCalculation_ReplaceLastNumberWithNewOne() 
        {
            var builder = new EquationCalculationBuilder();

            builder.AddNumber(1);
            builder.AddNumber(2);

            var result = builder.ToString();

            Assert.That(result, Does.EndWith("2"));
            Assert.That(result, Does.Not.Contain("1"));
        }

        [Test]
        public void AddCalc_FirstCalc_ReplaceDefaultCalculationToSubtraction()
        {
            var builder = new EquationCalculationBuilder();

            builder.AddCalculation(CalculationType.Subtraction);

            var result = builder.ToString();

            Assert.That(result, Does.Not.Contain("+"));
        }

        [Test]
        public void AddCalc_AddCalculationAfterAddingNumber_ExtendEquationWithDesiredCalculation()
        {
            var builder = new EquationCalculationBuilder();
            builder.AddNumber(1);

            builder.AddCalculation(CalculationType.Subtraction);

            var result = builder.ToString();

            Assert.That(result, Does.Contain("-"));
            Assert.That(result, Does.Contain("1"));
        }

        
        [Test]
        public void Build_NotCompleteEquationToBuild_BuildsWithDuplicatedLastNumberInput() 
        {
            var builder = new EquationCalculationBuilder();

            builder.AddNumber(1);
            builder.AddCalculation(CalculationType.Subtraction);

            var result = builder.Build().GetResult();

            Assert.That(result, Is.EqualTo(0));
        }


        [Test]
        public void ToString_CorrectCalculation_CorrectStringFormat() 
        {
            var builder = new EquationCalculationBuilder();

            builder.AddNumber(1);
            builder.AddCalculation(CalculationType.Sum);
            builder.AddNumber(1);

            var result = builder.ToString();

            Assert.That(result, Is.EqualTo("1 + 1"));
        }

        [Test]
        public void ToString_CalledOnNewBuilderObject_ReturnsZero()
        {
            var builder = new EquationCalculationBuilder();

            var result = builder.ToString();

            Assert.That(result, Is.EqualTo("0"));
        }


        [Test]
        public void Reset_setEverythingToInitialState() 
        {
            var builder = new EquationCalculationBuilder();

            builder.AddNumber(1);
            builder.AddCalculation(CalculationType.Sum);
            builder.AddNumber(1);

            builder.Reset();

            var result = builder.Build().GetResult();

            Assert.That(result, Is.EqualTo(0));
        }


        // Build method tests - check for result - different scenarios

        [Test]
        public void Build_Simple2plus2_ReturnsFour()
        {
            var builder = new EquationCalculationBuilder();

            builder.AddNumber(2);
            builder.AddCalculation(CalculationType.Sum);
            builder.AddNumber(2);

            var result = builder.Build().GetResult();

            Assert.That(result, Is.EqualTo(4));
        }

        [Test]
        public void Build_DoubleSum_ReturnsSix()
        {
            var builder = new EquationCalculationBuilder();

            builder.AddNumber(2);
            builder.AddCalculation(CalculationType.Sum);
            builder.AddNumber(2);
            builder.AddCalculation(CalculationType.Sum);
            builder.AddNumber(2);

            var result = builder.Build().GetResult();

            Assert.That(result, Is.EqualTo(6));
        }

        [Test]
        public void Build_DifferentPriority_ReturnsTwelve()
        {
            var builder = new EquationCalculationBuilder();

            builder.AddNumber(3);
            builder.AddCalculation(CalculationType.Sum);
            builder.AddNumber(3);
            builder.AddCalculation(CalculationType.Multiplication);
            builder.AddNumber(3);

            var result = builder.Build().GetResult();

            Assert.That(result, Is.EqualTo(12));
        }

        [Test]
        public void Build_DifferentPriority_ReturnsTwentyOne()
        {
            var builder = new EquationCalculationBuilder();

            builder.AddNumber(3);
            builder.AddCalculation(CalculationType.Sum);
            builder.AddNumber(3);
            builder.AddCalculation(CalculationType.Multiplication);
            builder.AddNumber(3);
            builder.AddCalculation(CalculationType.Sum);
            builder.AddNumber(3);
            builder.AddCalculation(CalculationType.Multiplication);
            builder.AddNumber(3);

            var result = builder.Build().GetResult();

            Assert.That(result, Is.EqualTo(21));
        }

        [Test]
        public void Build_DifferentPriority_ReturnsFifteen()
        {
            var builder = new EquationCalculationBuilder();

            builder.AddNumber(3);
            builder.AddCalculation(CalculationType.Sum);
            builder.AddNumber(3);
            builder.AddCalculation(CalculationType.Multiplication);
            builder.AddNumber(3);
            builder.AddCalculation(CalculationType.Division);
            builder.AddNumber(3);
            builder.AddCalculation(CalculationType.Sum);
            builder.AddNumber(3);
            builder.AddCalculation(CalculationType.Multiplication);
            builder.AddNumber(3);

            var result = builder.Build().GetResult();

            Assert.That(result, Is.EqualTo(15));
        }

        [Test]
        public void Build_DifferentPriorityWithPower_ReturnsFiftythree()
        {
            var builder = new EquationCalculationBuilder();

            builder.AddNumber(2);
            builder.AddCalculation(CalculationType.Multiplication);
            builder.AddNumber(3);
            builder.AddCalculation(CalculationType.Power);
            builder.AddNumber(3);
            builder.AddCalculation(CalculationType.Subtraction);
            builder.AddNumber(3);
            builder.AddCalculation(CalculationType.Division);
            builder.AddNumber(3);

            var result = builder.Build().GetResult();

            Assert.That(result, Is.EqualTo(53));
        }
    }
}
