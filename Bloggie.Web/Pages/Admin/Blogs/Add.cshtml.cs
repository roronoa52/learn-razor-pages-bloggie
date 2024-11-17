using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Bloggie.Web.Pages.Admin.Blogs
{
    [Authorize(Roles = "Admin")]
    public class AddModel : PageModel
    {
        private readonly IBlogPostRepository repository;

        public AddModel(IBlogPostRepository repository)
        {
            this.repository = repository;
        }

        [BindProperty]
        public AddBlogPost AddBlogPostRequest { get; set; }

        [BindProperty]
        public IFormFile FeaturedImage { get; set; }

        [BindProperty]
        [Required]
        public string Tags { get; set; }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost()
        {
            ValidateAddBlog();

			if (ModelState.IsValid)
            {
				BlogPost blogPost = new BlogPost()
				{
					Heading = AddBlogPostRequest.Heading,
					PageTitle = AddBlogPostRequest.PageTitle,
					Content = AddBlogPostRequest.Content,
					ShortDescription = AddBlogPostRequest.ShortDescription,
					FeturedImageUrl = AddBlogPostRequest.FeturedImageUrl,
					UrlHandle = AddBlogPostRequest.UrlHandle,
					PublishedDate = AddBlogPostRequest.PublishedDate,
					Author = AddBlogPostRequest.Author,
					Visible = AddBlogPostRequest.Visible,
					Tags = new List<Tag>(Tags.Split(",").Select(t => new Tag { Name = t.Trim() })),
				};

				await repository.CreateAsync(blogPost);


				var notification = new Notification
				{
					Massage = "The new blog post has post",
					Type = Enum.NotificationType.Success
				};

				TempData["MessageDescription"] = JsonSerializer.Serialize(notification);


				return RedirectToPage("/admin/blogs/list");
			}

            return Page();
        }
    
        private void ValidateAddBlog()
        {
            if(AddBlogPostRequest.PublishedDate.Date < DateTime.Now.Date)
            {
                ModelState.AddModelError("AddBlogPostRequest.PublishedDate",
                    $"PublishedDate can only be today date or future date");
            }
        }
    }
}
