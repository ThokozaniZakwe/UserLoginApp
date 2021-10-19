using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UserLoginApp.Pages.Users
{
    public class CreateModel : AdminPageModel
    {
        public CreateModel(UserManager<IdentityUser> usrManager)
        {
            UserManager = usrManager;
        }
        public UserManager<IdentityUser> UserManager { get; set; }
        [BindProperty]
        [Required]
        public string UserName { get; set; }
        [Required]
        [BindProperty]
        [EmailAddress]
        public string Email { get; set; }
        [BindProperty]
        [Required]
        public string Password { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser { UserName = UserName, Email = Email };
                IdentityResult result = await UserManager.CreateAsync(user, Password);
                if (result.Succeeded)
                {
                    return RedirectToPage("List");
                }
                foreach(IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
            return Page();
        }
    }
}
