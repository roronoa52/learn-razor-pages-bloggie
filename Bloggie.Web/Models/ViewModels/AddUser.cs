using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.ViewModels
{
	public class AddUser
	{
		[Required]
		public string Username { get; set; }
		[Required]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
		public bool AdminRole { get; set; }
	}
}
