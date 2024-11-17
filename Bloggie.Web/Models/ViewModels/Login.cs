﻿using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.ViewModels
{
	public class Login
	{
		[Required]
		public string Username { get; set; }
		[Required]
		public string Password { get; set; }
	}
}
