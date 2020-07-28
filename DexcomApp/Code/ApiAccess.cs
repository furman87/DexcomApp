namespace DexcomApp.Code
{
    using System;
    using System.Collections.Generic;
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
            this.baseDexcomUri = this.configuration["BaseDexcomUri"];
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

        public string OauthUri { get; }

        public async Task<string> GetOauthTokenAsync()
        {
            var url = this.OauthUri;
            var authResponse = await this.client.GetAsync(new Uri(url)).ConfigureAwait(false);
            var tokenResponse = await authResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

            return tokenResponse;
        }

        public async Task<DexcomToken> GetAccessTokenAsync(string authorizationCode)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "v2/oauth2/token");
            request.Headers.Add("cache-control", "no-cache");

            var content = new List<KeyValuePair<string, string>>();
            content.Add(new KeyValuePair<string, string>("client_id", this.clientId));
            content.Add(new KeyValuePair<string, string>("client_secret", this.clientSecret));
            content.Add(new KeyValuePair<string, string>("code", authorizationCode));
            content.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));
            content.Add(new KeyValuePair<string, string>("redirect_uri", this.baseRedirectUri + "Dexcom/Complete"));

            request.Content = new FormUrlEncodedContent(content);
            var response = await this.client.SendAsync(request).ConfigureAwait(false);
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var token = json.ToObject<DexcomToken>();
            return token;
        }

        public async Task<DexcomToken> RefreshAccessTokenAsync(DexcomToken currentToken)
        {
            if (currentToken == null)
            {
                return default;
            }

            using var request = new HttpRequestMessage(HttpMethod.Post, "v2/oauth2/token");
            request.Headers.Add("cache-control", "no-cache");

            var content = new List<KeyValuePair<string, string>>();
            content.Add(new KeyValuePair<string, string>("client_id", this.clientId));
            content.Add(new KeyValuePair<string, string>("client_secret", this.clientSecret));
            content.Add(new KeyValuePair<string, string>("refresh_token", currentToken.RefreshToken));
            content.Add(new KeyValuePair<string, string>("grant_type", "refresh_token"));
            content.Add(new KeyValuePair<string, string>("redirect_uri", this.baseRedirectUri + "Dexcom/Complete"));

            request.Content = new FormUrlEncodedContent(content);
            var response = await this.client.SendAsync(request).ConfigureAwait(false);
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var token = json.ToObject<DexcomToken>();
            return token;
        }

        public async Task<string> GetEgvs(string accessToken)
        {
            var endDate = DateTime.UtcNow.AddSeconds(this.configuration.GetValue<int>("TimeAdjustmentSeconds"));
            var startDate = endDate.AddSeconds(this.configuration.GetValue<int>("DefaultEgvPeriodSeconds"));
            var dateFormat = this.configuration["DateTimeQueryFormat"];
            var uri = $"v2/users/self/egvs?startDate={startDate.ToString(dateFormat, CultureInfo.InvariantCulture)}&endDate={endDate.ToString(dateFormat, CultureInfo.InvariantCulture)}";

            using var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Add("cache-control", "no-cache");
            request.Headers.Add("authorization", "Bearer " + accessToken);

            var response = await this.client.SendAsync(request).ConfigureAwait(false);
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return json;
        }

        public async Task<string> GetDataRange(string accessToken)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"v2/users/self/dataRange");
            request.Headers.Add("cache-control", "no-cache");
            request.Headers.Add("authorization", "Bearer " + accessToken);

            var response = await this.client.SendAsync(request).ConfigureAwait(false);
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return json;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            // Free managed resources too, but only if I'm being called from Dispose
            // If I'm being called from Finalize then the objects might not exist anymore.
            if (isDisposing)
            {
                this.client.Dispose();
            }
        }
    }
}
