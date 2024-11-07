using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;

namespace Bloggie.Web.Pages
{
    public class RegisterModel : PageModel
    {
		private readonly UserManager<IdentityUser> userManager;

		public RegisterModel(UserManager<IdentityUser> userManager)
        {
			this.userManager = userManager;
			this.userManager = userManager;
		}

        [BindProperty]
        public Register RegisterViewModel { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
			var user = new IdentityUser
			{
				UserName = RegisterViewModel.Username,
				Email = RegisterViewModel.Email,
			};

			var identityResult = await userManager.CreateAsync(user, RegisterViewModel.Password);

			if (identityResult.Succeeded)
			{
				var addRolesResult = await userManager.AddToRoleAsync(user, "User");

				if (addRolesResult.Succeeded)
				{
					ViewData["MessageDescription"] = new Notification
					{
						Type = Enum.NotificationType.Success,
						Massage = "User Registered Successfully"
					};

					return Page();
				}
			}

			ViewData["MessageDescription"] = new Notification
			{
				Type = Enum.NotificationType.Error,
				Massage = "Something Went Wrong"
			};

			return Page();
		}
    }
}
