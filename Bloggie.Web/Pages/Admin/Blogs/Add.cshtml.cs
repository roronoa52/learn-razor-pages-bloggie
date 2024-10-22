using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Bloggie.Web.Pages.Admin.Blogs
{
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

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost()
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
    }
}
