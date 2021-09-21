using lukaKry.Calc.Library.Logic;
using lukaKry.Calc.Library.Logic.CalculationsBuilders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace lukaKry.Calc.ConsoleApp
{
    class Program
    {
        private static CalculationsFactoryProvider _calculationProvider = new();
        private static SimpleCalculationArchiver _archiver = new();
        private static CalculationBuilderProvider _builderProvider = new();
        private static ICalculationBuilder _builder;

        static void Main(string[] args)
        {
            
            RunCalculationCycle(BuilderMode.another);

            Console.WriteLine("Would You like to see calculations history? (y/n)");
            if (ShouldShowCalculationsHistory(Console.ReadLine()))
            {
                ShowCalculationsHistory(_archiver);
            }
        }

        private static void RunCalculationCycle(BuilderMode builderMode)
        {
            do
            {
                Console.Clear();
                _builder = _builderProvider[builderMode]();

                var num = GetUserInput("number");
                _builder.AddNumber(num);

                do
                {
                    string calcTypeChoice = GetCalcTypeFromUser();
                    _builder.AddCalculation(GetCalculationType(calcTypeChoice));

                    var num2 = GetUserInput("number");
                    _builder.AddNumber(num2);
                } while (IsEndOfEquation(builderMode));

                try
                {
                    _archiver.AddCalculation(_builder.Build());
                    Console.WriteLine( _archiver.GetLastCalculation());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            } while (ShouldRestart());
        }

        private static bool IsEndOfEquation(BuilderMode builderMode)
        {
            Console.WriteLine("Press = to solve equation or anything else to continue");
            // OemPlus is a key with both plus and equal sign
            bool isEnd = !Equals(Console.ReadKey().Key, ConsoleKey.OemPlus);
            Console.WriteLine("\b");
            return isEnd;
        }
        
        private static bool ShouldRestart()
        {
            Console.WriteLine("Restart? (y/n)");
            return Console.ReadLine().ToUpper() == "Y";
        }

        private static bool ShouldShowCalculationsHistory(string answer)
        {
            return answer.ToUpper() == "Y";
        }

        private static void ShowCalculationsHistory(SimpleCalculationArchiver archiver)
        {
            var history = archiver.GetAll();

            foreach (var calc in history)
            {
                Console.WriteLine(calc);
            }
        }

        private static string GetCalcTypeFromUser()
        {
            var correctInput = false;
            string calcTypeChoice;
            do
            {
                Console.WriteLine("Choose type of calculation\n+\tsum\n-\tsubtraction\n*\tmultiplication\n/\tdivision\n^\tpower");
                calcTypeChoice = Console.ReadLine();
                correctInput = CalculationTypeChoiceCheck(calcTypeChoice);
                if (!correctInput) Console.WriteLine("Wrong input.");
            } while (!correctInput);

            return calcTypeChoice;
        }

        private static decimal GetUserInput(string inputType)
        {
            var correctInput = false;
            decimal number;
            do
            {
                Console.WriteLine($"Write {inputType}");
                correctInput = decimal.TryParse(Console.ReadLine(), out number);
                if (!correctInput) Console.WriteLine("Wrong input.");
            } while (!correctInput);

            return number;
        }

        private static CalculationType GetCalculationType(string calcTypeChoice)
        {
            switch (calcTypeChoice)
            {
                case "-": return CalculationType.Subtraction;
                case "*": return CalculationType.Multiplication;
                case "/": return CalculationType.Division;
                case "^": return CalculationType.Power;
                default: return CalculationType.Sum;
            }
        }

        private static bool CalculationTypeChoiceCheck(string calcTypeChoice)
        {
            string[] validKeys = { "+", "-", "*", "/", "^" };
            return Array.IndexOf(validKeys, calcTypeChoice) >= 0;
        }
    }
}
