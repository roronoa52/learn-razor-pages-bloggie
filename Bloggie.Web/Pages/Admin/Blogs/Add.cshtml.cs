using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Web.Pages.Admin.Blogs
{
    public class AddModel : PageModel
    {
        private readonly BloggerDbContext db;

        public AddModel(BloggerDbContext db)
        {
            this.db = db;
        }

        [BindProperty]
        public AddBlogPost AddBlogPostRequest { get; set; }

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
            
            await db.BlogPosts.AddAsync(blogPost);
            
            await db.SaveChangesAsync();

            return RedirectToPage("/admin/blogs/list");
        }
    }
}
