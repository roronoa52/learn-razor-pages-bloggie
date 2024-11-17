using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Web.Pages.Admin.User
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
		private readonly IUserRepository userRepository;

        public List<Models.ViewModels.User> Users { get; set; }

        [BindProperty]
        public AddUser AddUserRequest { get; set; }

        [BindProperty]
        public Guid SelectedUserId { get; set; }

		public IndexModel(IUserRepository userRepository)
        {
			this.userRepository = userRepository;
		}

        public async Task<IActionResult> OnGet()
        {
			await GetUsers();

			return Page();
        }

        public async Task<IActionResult> OnPost()
        {

            if (ModelState.IsValid)
            {
				var identityUser = new IdentityUser()
				{
					UserName = AddUserRequest.Username,
					Email = AddUserRequest.Email,

				};

				var roles = new List<string> { "User" };
				if (AddUserRequest.AdminRole)
				{
					roles.Add("Admin");
				}

				var result = await userRepository.Add(identityUser, AddUserRequest.Password, roles);

				if (result)
				{
					return RedirectToPage("/admin/user/index");
				}

				return Page();
			}

			await GetUsers();


			return Page();
            
        }

        public async Task<IActionResult> OnPostDelete()
        {
            await userRepository.Delete(SelectedUserId);

			return RedirectToPage("/admin/user/index");
		}

        private async Task GetUsers()
        {
			var users = await userRepository.GetAll();

			Users = new List<Models.ViewModels.User>();

			foreach (var user in users)
			{
				Users.Add(new Models.ViewModels.User()
				{
					Id = Guid.Parse(user.Id),
					Username = user.UserName,
					Email = user.Email,
				});
			}
		}
    }
}