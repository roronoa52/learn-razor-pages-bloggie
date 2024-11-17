using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Bloggie.Web.Pages.Admin.Blogs
{

	[Authorize(Roles = "Admin")]
	public class EditModel : PageModel
    {
        private readonly IBlogPostRepository repository;

        public EditModel(IBlogPostRepository repository)
        {
            this.repository = repository;
        }

        [BindProperty]
        public EditBlogPostRequest BlogPost { get; set; }

		[BindProperty]
		public IFormFile FeaturedImage { get; set; }

        [BindProperty]
        public string Tags { get; set; }

		public async Task OnGet(Guid id)
        {
            var blogPostDomainModel = await repository.GetByIdAsync(id);

            if (blogPostDomainModel != null && blogPostDomainModel.Tags != null)
            {
                BlogPost = new EditBlogPostRequest
                {
                    Id = blogPostDomainModel.Id,
                    Heading = blogPostDomainModel.Heading,
                    PageTitle = blogPostDomainModel.PageTitle,
                    Content = blogPostDomainModel.Content,
                    ShortDescription = blogPostDomainModel.ShortDescription,
                    FeturedImageUrl = blogPostDomainModel.FeturedImageUrl,
                    UrlHandle = blogPostDomainModel.UrlHandle,
                    PublishedDate = blogPostDomainModel.PublishedDate,
                    Author = blogPostDomainModel.Author,
                    Visible = blogPostDomainModel.Visible,
                };
					Tags = string.Join(",", blogPostDomainModel.Tags.Select(x => x.Name));
            }
        }

        public async Task<IActionResult> OnPostEdit()
        {

            if (ModelState.IsValid)
            {
				try
				{
					var blogPostDomainModel = new BlogPost
					{

						Id = BlogPost.Id,
						Heading = BlogPost.Heading,
						PageTitle = BlogPost.PageTitle,
						Content = BlogPost.Content,
						ShortDescription = BlogPost.ShortDescription,
						FeturedImageUrl = BlogPost.FeturedImageUrl,
						UrlHandle = BlogPost.UrlHandle,
						PublishedDate = BlogPost.PublishedDate,
						Author = BlogPost.Author,
						Visible = BlogPost.Visible,
						Tags = new List<Tag>(Tags.Split(',').Select(x => new Tag() { Name = x.Trim() }))
					};

					await repository.UpdateAsync(blogPostDomainModel);

					ViewData["MessageDescription"] = new Notification
					{
						Massage = "Post Has been Update",
						Type = Enum.NotificationType.Success
					};

				}
				catch (Exception ex)
				{

					ViewData["MessageDescription"] = new Notification
					{
						Massage = ex.Message,
						Type = Enum.NotificationType.Error
					};
				}
			}

            return Page();
        }

        public async Task<IActionResult> OnPostDelete()
        {
            var existingPost = await repository.GetByIdAsync(BlogPost.Id);

            if (existingPost != null)
            {
                await repository.DeleteAsync(BlogPost.Id);

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
    }
}
