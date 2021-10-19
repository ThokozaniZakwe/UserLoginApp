using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UserLoginApp.Pages.Roles
{
    public class EditorModel : AdminPageModel
    {
        public EditorModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }
        public UserManager<IdentityUser> UserManager { get; set; }
        public RoleManager<IdentityRole> RoleManager { get; set; }
        public IdentityRole Role { get; set; }

        public async Task<IList<IdentityUser>> Members()
        {
            return await UserManager.GetUsersInRoleAsync(Role.Name);
        }

        public async Task<IEnumerable<IdentityUser>> NonMembers() => UserManager.Users.ToList().Except(await Members());

        public async Task OnGetAsync(string id)
        {
            Role = await RoleManager.FindByIdAsync(id);
        }

        public async Task<IActionResult> OnPostAsync(string userid, string rolename)
        {
            Role = await RoleManager.FindByNameAsync(rolename);
            IdentityUser user = await UserManager.FindByIdAsync(userid);
            IdentityResult result;
            if(await UserManager.IsInRoleAsync(user, rolename))
            {
                result = await UserManager.RemoveFromRoleAsync(user, rolename);
            }
            else
            {
                result = await UserManager.AddToRoleAsync(user, rolename);
            }
            if (result.Succeeded)
            {
                return RedirectToPage();
            }
            else
            {
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return Page();
            }
        }
    }
}
