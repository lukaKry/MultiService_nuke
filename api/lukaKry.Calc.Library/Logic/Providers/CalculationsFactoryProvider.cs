using lukaKry.Calc.Library.Logic.CalculationFactories;
using lukaKry.Calc.Library.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lukaKry.Calc.Library.Logic
{
    public enum CalculationType
    {
        Sum,
        Subtraction,
        Multiplication,
        Division,
        Power
    }

    public class CalculationsFactoryProvider
    {
        private Dictionary<CalculationType, Func<ICalculationFactory>> _factories;

        public CalculationsFactoryProvider() : this(GetPredefinedCalculationsFactories())  // default argument for one-argument constructor
        {
        }

        public CalculationsFactoryProvider(Dictionary<CalculationType, Func<ICalculationFactory>> factories )
        {
            _factories = factories;
        }

        public ICalculationFactory this[CalculationType type]       // indexer
        {
            get { return _factories[type](); }
        }


        private static Dictionary<CalculationType, Func<ICalculationFactory>> GetPredefinedCalculationsFactories()
        {
            return new Dictionary<CalculationType, Func<ICalculationFactory>>()
            {
                { CalculationType.Sum, () => new SumCalculationFactory() },
                { CalculationType.Subtraction, () => new SubtractionCalculationFactory() },
                { CalculationType.Multiplication, () => new MultiplicationCalculationFactory() },
                { CalculationType.Division, () => new DivisionCalculationFactory() },
                { CalculationType.Power, () => new PowerCalculationFactory() }
            };
        }
    }
}
