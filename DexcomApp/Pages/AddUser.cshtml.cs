namespace DexcomApp.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DexcomApp.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class AddUserModel : PageModel
    {
        [BindProperty]
        public User User { get; set; }

        public void OnGet()
        {

        }
    }
}