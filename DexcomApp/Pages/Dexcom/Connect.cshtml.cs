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

    public class ConnectModel : PageModel
    {
        private IConfiguration configuration;
        private ApiAccess apiAccess;

        public ConnectModel(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.apiAccess = new ApiAccess(this.configuration["CLIENT_ID"], this.configuration["CLIENT_SECRET"], this.configuration);
            this.DexcomToken = null;
        }

        public DexcomToken DexcomToken { get; set; }

        public IActionResult OnGet()
        {
            var json = this.Request.Cookies["DexcomToken"];

            if (string.IsNullOrWhiteSpace(json))
            {
                this.DexcomToken = null;
            }
            else
            {
                this.DexcomToken = JsonConvert.DeserializeObject<DexcomToken>(json);
            }

            return this.Page();
        }

        public IActionResult OnGetConnectDexcom()
        {
            return this.Redirect(this.apiAccess.OauthUri);
        }
    }
}