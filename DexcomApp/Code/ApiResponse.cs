// <copyright file="ApiResponse.cs" company="Ken Watson">
// Copyright (c) Ken Watson. All rights reserved.
// </copyright>

namespace DexcomApp.Code
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class ApiResponse
    {
        private HttpResponseMessage httpResponse;

        public ApiResponse(HttpResponseMessage httpResponse)
        {
            this.httpResponse = httpResponse ?? throw new ArgumentNullException(nameof(httpResponse));
        }

        public HttpStatusCode StatusCode => this.httpResponse.StatusCode;

        public string Content => Task.Run(() => this.httpResponse.Content.ReadAsStringAsync()).Result;
    }
}
