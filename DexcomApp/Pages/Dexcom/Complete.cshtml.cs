// <copyright file="Complete.cshtml.cs" company="Ken Watson">
// Copyright (c) Ken Watson. All rights reserved.
// </copyright>

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
        private IConfiguration configuration;
        private string cookieName;
        private ApiAccess apiAccess;

        public CompleteModel(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.apiAccess = new ApiAccess(this.configuration["CLIENT_ID"], this.configuration["CLIENT_SECRET"], this.configuration);
            this.cookieName = this.configuration.GetValue<bool>("IsSandbox") ? this.configuration["SandboxCookieName"] : this.configuration["CookieName"];
        }

        public DexcomToken Token { get; set; }

        public async Task<IActionResult> OnGet(string code)
        {
            this.Token = await this.apiAccess.GetAccessTokenAsync(code).ConfigureAwait(false);
            this.Response.SaveCookie<DexcomToken>(this.cookieName, this.Token);
            return this.Page();
        }
    }
}