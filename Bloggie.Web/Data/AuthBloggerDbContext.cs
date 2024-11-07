using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Data
{
	public class AuthBloggerDbContext : IdentityDbContext
	{
        public AuthBloggerDbContext(DbContextOptions<AuthBloggerDbContext> options) : base(options)
        {
            
        }


		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			var superAdminRoleId = "4b0b0ff5-47c4-4094-aec0-78b17bb91781";
			var adminRoleId = "a639de1c-9d67-41da-abab-8da2589f9acb";
			var userRoleId = "d40b42ca-c818-4664-99e9-e91b200e5be2";

			var roles = new List<IdentityRole>
			{
				new IdentityRole()
				{
					Name = "SuperAdmin",
					NormalizedName = "SuperAdmin",
					Id = superAdminRoleId,
					ConcurrencyStamp = superAdminRoleId
				},

				new IdentityRole()
				{
					Name = "Admin",
					NormalizedName = "Admin",
					Id = adminRoleId,
					ConcurrencyStamp = adminRoleId
				},
				new IdentityRole()
				{
					Name = "User",
					NormalizedName = "User",
					Id = userRoleId,
					ConcurrencyStamp = userRoleId
				},
			};

			builder.Entity<IdentityRole>().HasData(roles);

			// seed super admin user
			var superAdminId = "270f5c53-1268-4f01-aec7-bb6626aa602a";
			var superAdminUser = new IdentityUser()
			{
				Id = superAdminId,
				UserName = "superadmin@gmail.com",
				NormalizedUserName = "superadmin@gmail.com",
				NormalizedEmail = "superadmin@gmail.com",
				Email = "superadmin@gmail.com"
			};

			superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(superAdminUser, "12345678");

			builder.Entity<IdentityUser>().HasData(superAdminUser);

			//Add all roles to super admin user
			var superAdminRoles = new List<IdentityUserRole<string>>()
			{
				new IdentityUserRole<string>
				{
					RoleId = superAdminRoleId,
					UserId = superAdminId
				},
				new IdentityUserRole<string>
				{
					RoleId = adminRoleId,
					UserId = superAdminId
				},
				new IdentityUserRole<string>
				{
					RoleId = userRoleId,
					UserId = superAdminId
				},
			};

			builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);

		}
	}

	
}
