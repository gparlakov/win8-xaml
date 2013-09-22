using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrimesCalcControl
{
    public class PrimesCalculator
    {
        public async Task<List<int>> 
            CalculatePrimesInRangeAsync(int start, int end)
        {
            var numbers = await Task.Run(() =>
                {
                    var list = new List<int>();
                    for (int i = start; i <= end; i++)
                    {
                        if (IsPrime(i))
                        {
                            list.Add(i);
                        }
                    }

                    return list;
                });

            return numbers;
        }
  
        public bool IsPrime(long number)
        {
            var isPrime = true;
            var numberSqrt = Math.Sqrt(number);
            for (int i = 2; i <= numberSqrt; i++)
            {
                if (number % i == 0)
                {
                    isPrime = false;
                    break;
                }
            }

            return isPrime;                 
        }
    }
}
