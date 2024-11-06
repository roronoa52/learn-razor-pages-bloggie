using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Web.Pages.Tags
{
    public class DetailsModel : PageModel
    {
		private readonly IBlogPostRepository blogPostRepository;

        public List<BlogPost> Blogs { get; set; }

		public DetailsModel(IBlogPostRepository blogPostRepository)
        {
			this.blogPostRepository = blogPostRepository;
		}

        public async void OnGet(string TagName)
        {
			Blogs = (await blogPostRepository.GetAllAsync(TagName)).ToList();
        }
    }
}
