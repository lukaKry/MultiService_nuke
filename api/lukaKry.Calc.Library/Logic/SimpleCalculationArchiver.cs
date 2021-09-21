using lukaKry.Calc.Library.Interfaces;
using lukaKry.Calc.Library.Logic.Calculations;
using lukaKry.Calc.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lukaKry.Calc.Library.Logic
{
    public class SimpleCalculationArchiver : IRegistry
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private List<ICalculation> _archive = new();


        public void AddCalculation(ICalculation calculation)
        {
            Logger.Info("Calculation archived successfully.");
            _archive.Add(calculation);
        }

        public ICalculation GetLastCalculation()
        {
            if (!_archive.Any())
            {
                Logger.Error("An attempt to get last item from empty list was made.");
                throw new InvalidOperationException();
            }
            return _archive.Last();
        }

        public IEnumerable<ICalculation> GetAll() =>  _archive;

    }
}
