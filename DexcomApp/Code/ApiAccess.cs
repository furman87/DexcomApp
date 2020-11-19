// <copyright file="ApiAccess.cs" company="Ken Watson">
// Copyright (c) Ken Watson. All rights reserved.
// </copyright>

namespace DexcomApp.Code
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using DexcomApp.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;

    public class ApiAccess : IDisposable
    {
        private string clientId;
        private string clientSecret;
        private string baseRedirectUri;
        private string baseDexcomUri;
        private HttpClient client;
        private IConfiguration configuration;

        public ApiAccess(string clientId, string clientSecret, IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.baseRedirectUri = this.configuration["BaseRedirectUri"];
            this.baseDexcomUri = this.GetBaseUri(this.configuration.GetValue<bool>("IsSandbox"));
            this.OauthUri = $@"{this.baseDexcomUri}/v2/oauth2/login?client_id={clientId}&redirect_uri={this.baseRedirectUri + "Dexcom/Complete"}&response_type=code&scope=offline_access";
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.client = new HttpClient { BaseAddress = new Uri(this.baseDexcomUri) };
            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        ~ApiAccess()
        {
            this.Dispose(false);
        }

        private string GetBaseUri(bool sandbox)
        {
            var baseUri = this.configuration["BaseDexcomUri"];

            if (sandbox)
            {
                baseUri = baseUri.Insert(baseUri.IndexOf("api.", StringComparison.InvariantCultureIgnoreCase), "sandbox-");
            }

            return baseUri;
        }

        public string OauthUri { get; }

        public async Task<DexcomToken> GetAccessTokenAsync(string authorizationCode, bool refresh = false)
        {
            if (authorizationCode.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(authorizationCode));
            }

            using var request = new HttpRequestMessage(HttpMethod.Post, "v2/oauth2/token");
            request.Headers.Add("cache-control", "no-cache");

            var content = new List<KeyValuePair<string, string>>();
            content.Add(new KeyValuePair<string, string>("client_id", this.clientId));
            content.Add(new KeyValuePair<string, string>("client_secret", this.clientSecret));
            content.Add(new KeyValuePair<string, string>(refresh ? "refresh_token" : "code", authorizationCode));
            content.Add(new KeyValuePair<string, string>("grant_type", refresh ? "refresh_token" : "authorization_code"));
            content.Add(new KeyValuePair<string, string>("redirect_uri", this.baseRedirectUri + "Dexcom/Complete"));

            request.Content = new FormUrlEncodedContent(content);
            var response = new ApiResponse(await this.client.SendAsync(request).ConfigureAwait(false));

            return response.Content.ToObject<DexcomToken>();
        }

        public async Task<ApiResponse> GetEgvs(string accessToken, DateTime startDate = default(DateTime), DateTime endDate = default(DateTime))
        {
            if (endDate == default(DateTime))
            {
                endDate = DateTime.UtcNow.AddSeconds(this.configuration.GetValue<int>("TimeAdjustmentSeconds"));
            }

            if (startDate == default(DateTime) || (startDate > endDate))
            {
                startDate = endDate.AddHours(this.configuration.GetValue<int>("DefaultEgvPeriodHours"));
            }

            var dateFormat = this.configuration["DateTimeQueryFormat"];
            var uri = $"v2/users/self/egvs?startDate={startDate.ToString(dateFormat)}&endDate={endDate.ToString(dateFormat)}";

            using var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Add("cache-control", "no-cache");
            request.Headers.Add("authorization", $"Bearer {accessToken}");

            return new ApiResponse(await this.client.SendAsync(request).ConfigureAwait(false));
        }

        public async Task<ApiResponse> GetDataRange(string accessToken)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"v2/users/self/dataRange");
            request.Headers.Add("cache-control", "no-cache");
            request.Headers.Add("authorization", "Bearer " + accessToken);

            return new ApiResponse(await this.client.SendAsync(request).ConfigureAwait(false));
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                this.client.Dispose();
            }
        }
    }
}
