using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace PrimesCalcControl
{
    public sealed partial class NumbersCombinator : UserControl
    {
        private List<long> numbersAll;
        private PrimesCalculator primeCalc;

        public NumbersCombinator()
        {
            this.primeCalc = new PrimesCalculator();
            this.InitializeComponent();
        }

        public async void CalculatePrimesClick(object sender, RoutedEventArgs e)
        {
            int startNumber = GetNumber(this.StartNumber);
            int endNumber = GetNumber(this.EndNumber);
            if (endNumber < startNumber)
            {
                this.Numbers.Text = "End number must be larger than start number!";
                return;
            }
            try
            {
                var numbers = await this.primeCalc.CalculatePrimesInRangeAsync(startNumber, endNumber);

                var combinedNumbers = await CombineNumbers(numbers);

                this.numbersAll = combinedNumbers;
                this.NumbersToShow_Toggled(this, null);
            }
            catch (Exception ex)
            {
                this.Numbers.Text = ex.Message;
            }
        }       

        private async void NumbersToShow_Toggled(object sender, RoutedEventArgs e)
        {
            if (this.numbersAll == null)
            {
                return;
            }

            try
            {
                var numbersToShow = new List<long>();
                var showPrimes = this.NumbersToShow.IsOn;
                numbersToShow = await Task.Run(() =>
                {
                    if (showPrimes)
                    {
                        return this.numbersAll
                            .Where(m => this.primeCalc.IsPrime(m))
                            .ToList();
                    }
                    else
                    {
                        return this.numbersAll
                            .Where(m => !this.primeCalc.IsPrime(m))
                            .ToList();
                    }
                });

                this.Numbers.Text = await numbersToShow.JoinAsString<long>("|");
            }
            catch (Exception ex)
            {
                this.Numbers.Text = ex.Message + "!!!!";
            }
        }

        private async Task<List<long>> CombineNumbers(List<int> numbers)
        {
            return await Task.Run(() =>
            {
                var newNumbers = numbers.Select(n =>
                {
                    var lastDigit = n % 10;

                    int numberStartingWithThatDigit =
                        GetNumbersStartingWith(numbers, lastDigit);

                    var num = n.ToString() +
                        numberStartingWithThatDigit.ToString();

                    return long.Parse(num);
                });

                return newNumbers.ToList();
            });
        }

        private int GetNumbersStartingWith(List<int> numbers, int digit)
        {
            var result = 0;

            foreach (var num in numbers)
            {
                var numberText = num.ToString();
                if (numberText.StartsWith(digit.ToString()))
                {
                    result = num;
                    break;
                }
            }

            return result;
        }

        private int GetNumber(TextBox numberText)
        {
            int startNumber = 0;
            if (!int.TryParse(numberText.Text, out startNumber))
            {
                numberText.Text += "This needs to be a NUMBER!";
            }
            return startNumber;
        }
    }
}
