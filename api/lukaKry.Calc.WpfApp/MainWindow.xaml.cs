using System;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using lukaKry.Calc.Library.Logic;
using lukaKry.Calc.Library.Logic.CalculationsBuilders;

namespace lukaKry.Calc.Library
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public Calculator Calculator { get; set; }

        private string _mainDisplay;

        public event PropertyChangedEventHandler PropertyChanged;

        public string MainDisplay
        {
            get { return _mainDisplay; }
            set
            {
                _mainDisplay = value;
                PropertyChanged?.Invoke(this, new(nameof(MainDisplay)));
            }
        }
        public ICalculationBuilder Builder { get; set; } = new EquationCalculationBuilder();
        public CalculationsFactoryProvider Provider { get; set; } = new();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Calculator = new(Builder);
        }

        private void On_MemoryButton_Clicked(object sender, RoutedEventArgs e)
        {
        }

        private void On_ClearButton_Clicked(object sender, RoutedEventArgs e)
        {
            Calculator.ResetCurrentCalculation();
            MainDisplay = "";
        }

        private void On_NumberButton_Clicked(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            try
            {
                Builder.AddNumber(Convert.ToDecimal(button.Uid));
                MainDisplay += button.Uid;
            }
            catch ( DivideByZeroException ex)
            {
               _ = MessageBox.Show(ex.Message);
            }
        }

        private void On_SymbolButton_Clicked(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            MainDisplay += button.Uid;
            Builder.AddCalculation(GetCalculationType(button.Uid));
        }

        private void On_EqualSignButton_Clicked(object sender, RoutedEventArgs e)
        {
            MainDisplay += "=" + Calculator.GetResult();
        }

        private void On_CommaButton_Clicked(object sender, RoutedEventArgs e)
        {
        }

        private static CalculationType GetCalculationType(string calcTypeChoice)
        {
            switch (calcTypeChoice)
            {
                case "-": return CalculationType.Subtraction;
                case "*": return CalculationType.Multiplication;
                case "/": return CalculationType.Division;
                default: return CalculationType.Sum;
            }
        }
    }
}
