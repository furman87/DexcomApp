// <copyright file="DataRange.cshtml.cs" company="Ken Watson">
// Copyright (c) Ken Watson. All rights reserved.
// </copyright>

namespace DexcomApp.Pages.Dexcom
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using DexcomApp.Code;
    using DexcomApp.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;

    public class DataRangeModel : PageModel, IDisposable
    {
        private IConfiguration configuration;
        private ApiAccess apiAccess;
        private string cookieName;

        public DataRangeModel(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.apiAccess = new ApiAccess(this.configuration["CLIENT_ID"], this.configuration["CLIENT_SECRET"], this.configuration);
            this.cookieName = this.configuration.GetValue<bool>("IsSandbox") ? this.configuration["SandboxCookieName"] : this.configuration["CookieName"];
        }

        ~DataRangeModel()
        {
            // Not calling from Dispose, so it's not safe.
            this.Dispose(false);
        }

        public DataRange DataRange { get; set; }

        public string DataRangeJson { get; set; }

        public DexcomToken Token { get; set; }

        public async Task<IActionResult> OnGet()
        {
            this.Token = this.Request.GetCookie<DexcomToken>(this.cookieName);
            if (this.Token == null)
            {
                return this.Redirect(this.apiAccess.OauthUri);
            }

            var response = await this.apiAccess.GetDataRange(this.Token.AccessToken).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newToken = await this.apiAccess.GetAccessTokenAsync(this.Token.RefreshToken, true).ConfigureAwait(false);
                this.Response.SaveCookie<DexcomToken>(this.cookieName, newToken);
                this.Token = newToken;
                response = await this.apiAccess.GetDataRange(this.Token.AccessToken).ConfigureAwait(false);
            }

            var json = response.Content;
            this.DataRangeJson = json;
            this.DataRange = json.ToObject<DataRange>();
            return this.Page();
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
                this.apiAccess.Dispose();
            }
        }
    }
}