using lukaKry.Calc.API.Models;
using lukaKry.Calc.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lukaKry.Calc.API.DataAccess
{
    public interface IEquationsRepository
    {
        EquationDTO AddEquation(EquationDTO equation);
        List<EquationRecord> GetAllEquations();
        EquationRecord GetLastEquation();
    }
}
