using lukaKry.Calc.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lukaKry.Calc.Library.Logic
{
    public class CalculationArchiverProvider
    {
        private readonly IRegistry _archiver;

        public CalculationArchiverProvider(IRegistry archiver = null)
        {
            _archiver = archiver ?? GetDefaultArchiver();
        }

        public IRegistry GetArchiver()
        {
            return _archiver;
        }

        private IRegistry GetDefaultArchiver()
        {
            return new SimpleCalculationArchiver();
        }
    }
}
