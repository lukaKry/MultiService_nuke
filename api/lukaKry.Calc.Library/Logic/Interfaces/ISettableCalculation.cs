using System.Text.Json.Serialization;

namespace lukaKry.Calc.Library.Logic
{
    public interface ISettableCalculation : ICalculation
    {
        ICalculation Arg1 { get; set; }
        ICalculation Arg2 { get; set; }
        int Priority => 0;
    }
}