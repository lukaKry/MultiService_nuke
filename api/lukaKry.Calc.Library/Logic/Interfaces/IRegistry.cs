using lukaKry.Calc.Library.Logic;
using lukaKry.Calc.Library.Logic.Calculations;
using lukaKry.Calc.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace lukaKry.Calc.Library.Interfaces
{
    public interface IRegistry
    {
        ICalculation GetLastCalculation();
        void AddCalculation(ICalculation calculation);
        IEnumerable<ICalculation> GetAll();
    }
}