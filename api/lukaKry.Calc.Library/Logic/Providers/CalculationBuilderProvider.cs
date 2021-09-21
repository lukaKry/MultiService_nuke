using lukaKry.Calc.Library.Logic.CalculationsBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lukaKry.Calc.Library.Logic
{
    public enum BuilderMode
    {
        simple,
        extended,
        advanced,
        another
    }

    public class CalculationBuilderProvider
    {
        private Dictionary<BuilderMode, Func<ICalculationBuilder>> _builders;

        public CalculationBuilderProvider() : this(GetDefaultBuilders())
        {
        }

        public CalculationBuilderProvider(Dictionary<BuilderMode, Func<ICalculationBuilder>> builders)
        {
            _builders = builders;
        }

        public Func<ICalculationBuilder> this[BuilderMode type] 
        { 
            get { return _builders[type]; }
        }


        private static Dictionary<BuilderMode, Func<ICalculationBuilder>> GetDefaultBuilders()
        {
            return new Dictionary<BuilderMode, Func<ICalculationBuilder>>
            {
                { BuilderMode.simple, () => new EquationCalculationBuilder() },
                { BuilderMode.extended, () => new EquationCalculationBuilder() },
                { BuilderMode.advanced, () => new EquationCalculationBuilder() },
                { BuilderMode.another, () => new EquationCalculationBuilder() }
            };
        }
    }
}
