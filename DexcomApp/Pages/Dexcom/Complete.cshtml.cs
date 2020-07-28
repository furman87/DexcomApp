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
    using Newtonsoft.Json.Linq;

    public class CompleteModel : PageModel
    {
        public CompleteModel(IConfiguration configuration)
        {
            this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.ApiAccess = new ApiAccess(this.Configuration["CLIENT_ID"], this.Configuration["CLIENT_SECRET"], this.Configuration);
        }

        public DexcomToken Token { get; set; }

        private IConfiguration Configuration { get; }

        private ApiAccess ApiAccess { get; }

        public async Task<IActionResult> OnGet(string code)
        {
            this.Token = await this.ApiAccess.GetAccessTokenAsync(code).ConfigureAwait(false);
            this.Response.SaveCookie<DexcomToken>(this.Configuration["CookieName"], this.Token);
            return this.Page();
        }
    }
}