// <copyright file="HttpExtensions.cs" company="Ken Watson">
// Copyright (c) Ken Watson. All rights reserved.
// </copyright>

namespace DexcomApp.Code
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;

    public static class HttpExtensions
    {
        public static void SaveCookie<T>(this HttpResponse response, string cookieName, T cookieValue)
        {
            var value = string.Empty;
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(30),
                HttpOnly = true,
                Secure = true
            };

            if (typeof(T).Name == "String")
            {
                value = cookieValue.ToString();
            }
            else
            {
                value = JsonConvert.SerializeObject(cookieValue);
            }

            response.Cookies.Append(cookieName, value, options);
        }

        public static T GetCookie<T>(this HttpRequest request, string cookieName)
        {
            var cookie = request?.Cookies[cookieName];
            if (cookie == null)
            {
                return default;
            }

            if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(cookie, typeof(T), CultureInfo.InvariantCulture);
            }

            return JsonConvert.DeserializeObject<T>(cookie);
        }
    }
}
