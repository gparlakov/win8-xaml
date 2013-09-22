using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimesCalcControl
{
    public static class IEnumberableExtensions
    {
        public static async Task<string> 
            JoinAsString<T>(this IEnumerable<T> collection, string separator)
        {
            return await Task.Run(() =>
            {
                var result = new StringBuilder();

                foreach (var item in collection)
                {
                    result.Append(item.ToString() + separator);
                }
                result.Length -= separator.Length;

                return result.ToString();
            });
        }
    }
}
