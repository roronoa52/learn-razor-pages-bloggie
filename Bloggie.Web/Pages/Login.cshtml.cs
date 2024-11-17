using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Web.Pages
{
    public class LoginModel : PageModel
    {
		private readonly SignInManager<IdentityUser> signInManager;

		public LoginModel(SignInManager<IdentityUser> signInManager)
        {
			this.signInManager = signInManager;
		}

		[BindProperty]
        public Login LoginViewModel { get; set; }

        public void OnGet()
        {
        }

		public async Task<IActionResult> OnPost(string? ReturnUrl)
		{

			if (ModelState.IsValid)
			{
				var signIn = await signInManager.PasswordSignInAsync(LoginViewModel.Username, LoginViewModel.Password, false, false);

				if (signIn.Succeeded)
				{
					if (!string.IsNullOrWhiteSpace(ReturnUrl))
					{
						return RedirectToPage(ReturnUrl);
					}
					return RedirectToPage("Index");
				}
				else
				{
					ViewData["MessageDescription"] = new Notification()
					{
						Type = Enum.NotificationType.Error,
						Massage = "Username or Password is invalid"
					};

					return Page();
				}
			}
			else
			{
				return Page();
			}
		
			
		}
	}
}
