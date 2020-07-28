namespace DexcomApp.Code
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public static class StringExtensions
    {
        public static T ToObject<T>(this string s)
        {
            return JsonConvert.DeserializeObject<T>(s);
        }
    }
}
