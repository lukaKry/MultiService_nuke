using lukaKry.Calc.API.DataAccess;
using lukaKry.Calc.API.Models;
using lukaKry.Calc.Library.Interfaces;
using lukaKry.Calc.Library.Logic;
using lukaKry.Calc.Library.Logic.Calculations;
using lukaKry.Calc.Library.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lukaKry.Calc.API.Services
{
    

    public class DatabaseArchiver : IEquationsRepository
    {
        private readonly CalculationDataContext _context;
        public DatabaseArchiver(CalculationDataContext context) => _context = context;


        public EquationDTO AddEquation(EquationDTO equation)
        {
            EquationRecord equationRecord = new()
            {
                Numbers = equation.Numbers.ToList(),
                Symbols = equation.Symbols.ToList(),
                Result = equation.Result,
                Equation = equation.Equation
            };

            _context.Equations.Add(equationRecord);
            _context.SaveChanges();

            equation.Id = equationRecord.Id;

            return equation;
        }

        public List<EquationRecord> GetAllEquations() => _context.Equations.ToList();

        public EquationRecord GetLastEquation() => _context.Equations.OrderBy(x => x.Id).Last();
    }
}