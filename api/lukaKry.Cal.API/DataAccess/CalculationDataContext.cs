using lukaKry.Calc.API.Models;
using lukaKry.Calc.Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lukaKry.Calc.API.DataAccess
{
    public class CalculationDataContext : DbContext
    {
        public CalculationDataContext(DbContextOptions<CalculationDataContext> options) : base(options)
        { }

        public DbSet<EquationRecord> Equations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var splitStringConverter = new ValueConverter<List<string>, string>(v => string.Join(";", v), v => v.Split(new[] { ';' }).ToList());
            builder.Entity<EquationRecord>().Property(nameof(EquationRecord.Symbols)).HasConversion(splitStringConverter);


            var splitString2Converter = new ValueConverter<List<decimal>, string>(v => string.Join(";", v), v => v.Split(new[] { ';' }).Select(decimal.Parse).ToList());
            builder.Entity<EquationRecord>().Property(nameof(EquationRecord.Numbers)).HasConversion(splitString2Converter);
        }
    }
}
