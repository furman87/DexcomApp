// <copyright file="GenericExtensions.cs" company="Ken Watson">
// Copyright (c) Ken Watson. All rights reserved.
// </copyright>

namespace DexcomApp.Code
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public static class GenericExtensions
    {
        public static string ToJson<T>(this T value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public static bool In<T>(this T needle, params T[] haystack)
        {
            for (var i = 0; i < haystack.Length; i++)
            {
                if (needle.Equals(haystack[i]))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
