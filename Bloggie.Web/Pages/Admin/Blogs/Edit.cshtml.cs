using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Bloggie.Web.Pages.Admin.Blogs
{
    public class EditModel : PageModel
    {
        private readonly IBlogPostRepository repository;

        public EditModel(IBlogPostRepository repository)
        {
            this.repository = repository;
        }

        [BindProperty]
        public BlogPost BlogPost { get; set; }

		[BindProperty]
		public IFormFile FeaturedImage { get; set; }

        [BindProperty]
        public string Tags { get; set; }

		public async Task OnGet(Guid id)
        {
            BlogPost = await repository.GetByIdAsync(id);

            if (BlogPost != null && BlogPost.Tags != null)
            {
                Tags = string.Join(",", BlogPost.Tags.Select(x => x.Name));
            }
        }

        public async Task<IActionResult> OnPostEdit()
        {

            try
            {
                BlogPost.Tags = new List<Tag>(Tags.Split(',').Select(x => new Tag() { Name = x.Trim() }));

				await repository.UpdateAsync(BlogPost);

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
