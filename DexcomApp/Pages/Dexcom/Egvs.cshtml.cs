namespace DexcomApp.Pages.Dexcom
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

        public EgvsModel(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.apiAccess = new ApiAccess(this.configuration["CLIENT_ID"], this.configuration["CLIENT_SECRET"], this.configuration);
        }

        public EgvList EgvList { get; set; }

        public string EgvJson { get; set; }

        public DexcomToken Token { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var cookieName = this.configuration["CookieName"];
            this.Token = this.Request.GetCookie<DexcomToken>(cookieName);
            if (this.Token == null)
            {
                return this.Redirect(this.apiAccess.OauthUri);
            }

            var json = await this.apiAccess.GetEgvs(this.Token.AccessToken).ConfigureAwait(false);
            if (json.Contains("InvalidAccessToken", StringComparison.InvariantCultureIgnoreCase))
            {
                var newToken = await this.apiAccess.RefreshAccessTokenAsync(this.Token).ConfigureAwait(false);
                this.Response.SaveCookie<DexcomToken>(cookieName, newToken);
                this.Token = newToken;
                json = await this.apiAccess.GetEgvs(this.Token.AccessToken).ConfigureAwait(false);
            }

            this.EgvJson = json;
            this.EgvList = json.ToObject<EgvList>();
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