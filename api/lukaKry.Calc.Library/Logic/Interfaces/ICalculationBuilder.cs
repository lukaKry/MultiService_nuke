using lukaKry.Calc.Library.Logic.Calculations;

namespace lukaKry.Calc.Library.Logic
{
    public interface ICalculationBuilder
    {
        void AddNumber(decimal number);
        void AddCalculation(CalculationType calcType);
        Equation Build();
        void Reset();
    }
}