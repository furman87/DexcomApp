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

    public class DataRangeModel : PageModel, IDisposable
    {
        private IConfiguration configuration;
        private ApiAccess apiAccess;

        public DataRangeModel(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.apiAccess = new ApiAccess(this.configuration["CLIENT_ID"], this.configuration["CLIENT_SECRET"], this.configuration);
        }

        ~DataRangeModel()
        {
            this.Dispose(false); // Not calling from Dispose, so it's *not* safe
        }

        public DataRange DataRange { get; set; }

        public string DataRangeJson { get; set; }

        public DexcomToken Token { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var cookieName = this.configuration["CookieName"];
            this.Token = this.Request.GetCookie<DexcomToken>(cookieName);
            if (this.Token == null)
            {
                return this.Redirect(this.apiAccess.OauthUri);
            }

            var json = await this.apiAccess.GetDataRange(this.Token.AccessToken).ConfigureAwait(false);
            if (json.Contains("InvalidAccessToken", StringComparison.InvariantCultureIgnoreCase))
            {
                var newToken = await this.apiAccess.RefreshAccessTokenAsync(this.Token).ConfigureAwait(false);
                this.Response.SaveCookie<DexcomToken>(cookieName, newToken);
                this.Token = newToken;
                json = await this.apiAccess.GetDataRange(this.Token.AccessToken).ConfigureAwait(false);
            }

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