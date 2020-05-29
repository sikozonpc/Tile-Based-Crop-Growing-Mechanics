using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public static class DataUtils
    {
        
        public static Dictionary<T, K> MergeDictionaries<T, K>(Dictionary<T, K> d1, Dictionary<T, K> d2)
        {
            return d1.Concat(d2).GroupBy(d => d.Key)
             .ToDictionary(d => d.Key, d => d.First().Value);
        }    
    }
}
