using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Web.Pages.Admin.Blogs
{
    public class EditModel : PageModel
    {
        private readonly BloggerDbContext db;

        [BindProperty]
        public BlogPost BlogPost { get; set; }

        public EditModel(BloggerDbContext db)
        {
            this.db = db;
        }

        public async Task OnGet(Guid id)
        {
            BlogPost = await db.BlogPosts.FindAsync(id);
        }

        public async Task<IActionResult> OnPostEdit()
        {
            var existingPost = await db.BlogPosts.FindAsync(BlogPost.Id);

            if (existingPost != null)
            {
                existingPost.Heading = BlogPost.Heading;
                existingPost.PageTitle = BlogPost.PageTitle;
                existingPost.Content = BlogPost.Content;
                existingPost.ShortDescription = BlogPost.ShortDescription;
                existingPost.FeturedImageUrl = BlogPost.FeturedImageUrl;
                existingPost.UrlHandle = BlogPost.UrlHandle;
                existingPost.PublishedDate = BlogPost.PublishedDate;
                existingPost.Author = BlogPost.Author;
                existingPost.Visible = BlogPost.Visible;
            }

            await db.SaveChangesAsync();

            return RedirectToPage("/admin/blogs/list");
        }

        public async Task<IActionResult> OnPostDelete()
        {
            var existingPost = await db.BlogPosts.FindAsync(BlogPost.Id);

            if (existingPost != null)
            {
                db.Remove(existingPost);
                await db.SaveChangesAsync();

                return RedirectToPage("/admin/blogs/list");
            }

            return Page();
        }
    }
}
