using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Pages.Admin.Blogs
{
    public class ListModel : PageModel
    {
        private readonly BloggerDbContext db;

        public List<BlogPost> BlogPosts { get; set; }

        public ListModel(BloggerDbContext db)
        {
            this.db = db;
        }
        public async Task OnGet()
        {
            BlogPosts = await db.BlogPosts.ToListAsync();
        }
    }
}
