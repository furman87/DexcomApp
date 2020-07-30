// <copyright file="Egvs.cshtml.cs" company="Ken Watson">
// Copyright (c) Ken Watson. All rights reserved.
// </copyright>

namespace DexcomApp.Pages.Dexcom
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using DexcomApp.Code;
    using DexcomApp.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;

    public class EgvsModel : PageModel
    {
        private IConfiguration configuration;
        private ApiAccess apiAccess;
        private string cookieName;

        public EgvsModel(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.apiAccess = new ApiAccess(this.configuration["CLIENT_ID"], this.configuration["CLIENT_SECRET"], this.configuration);
            this.cookieName = this.configuration.GetValue<bool>("IsSandbox") ? this.configuration["SandboxCookieName"] : this.configuration["CookieName"];
        }

        public EgvList EgvList { get; set; }

        public string EgvJson { get; set; }

        public DexcomToken Token { get; set; }

        public async Task<IActionResult> OnGet()
        {
            this.Token = this.Request.GetCookie<DexcomToken>(this.cookieName);
            if (this.Token == null)
            {
                return this.Redirect(this.apiAccess.OauthUri);
            }

            var response = await this.apiAccess.GetEgvs(this.Token.AccessToken).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newToken = await this.apiAccess.GetAccessTokenAsync(this.Token.RefreshToken, true).ConfigureAwait(false);
                this.Response.SaveCookie<DexcomToken>(this.cookieName, newToken);
                this.Token = newToken;
                response = await this.apiAccess.GetEgvs(this.Token.AccessToken).ConfigureAwait(false);
            }

            var json = response.Content;
            this.EgvList = json.ToObject<EgvList>();
            var dateTimeFormat = this.configuration["DateTimeQueryFormat"];
            var dataPoints = this.EgvList.Egvs.Select(e => new { x = e.DisplayTime.ToString(dateTimeFormat, CultureInfo.InvariantCulture), y = e.Value });
            this.EgvJson = dataPoints.ToJson();
            return this.Page();
        }

        public string TrendClass(string trend)
        {
            switch (trend)
            {
                case "fortyFiveDown":
                case "fortyFiveUp":
                    return "table-warning";
                case "singleDown":
                case "doubleDown":
                case "singleUp":
                case "doubleUp":
                    return "table-danger";
                default:
                    return string.Empty;
            }
        }
    }
}