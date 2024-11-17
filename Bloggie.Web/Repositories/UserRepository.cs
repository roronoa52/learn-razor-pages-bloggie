using Bloggie.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly AuthBloggerDbContext db;
		private readonly UserManager<IdentityUser> userManager;

		public UserRepository(AuthBloggerDbContext db, UserManager<IdentityUser> userManager)
        {
			this.db = db;
			this.userManager = userManager;
		}

		public async Task<bool> Add(IdentityUser user, string password, List<string> roles)
		{
			var identityResult = await userManager.CreateAsync(user, password);

			if (identityResult.Succeeded)
			{
				identityResult = await userManager.AddToRolesAsync(user, roles);

				if (identityResult.Succeeded)
				{
					return true;
				}
			}

			return false;
		}

		public async Task Delete(Guid id)
		{
			var user = await userManager.FindByIdAsync(id.ToString());

			if (user != null)
			{
				await userManager.DeleteAsync(user);
			}
		}

		public async Task<IEnumerable<IdentityUser>> GetAll()
		{
			var users = await db.Users.ToListAsync();
			var superAdminUser = await db.Users.FirstOrDefaultAsync( x => x.Email == "superadmin@gmail.com");

			if (superAdminUser != null)
			{
				users.Remove(superAdminUser);
			}

			return users;
		}
	}
}
