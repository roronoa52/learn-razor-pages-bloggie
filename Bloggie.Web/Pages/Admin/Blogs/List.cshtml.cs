using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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

            var messagedescription = (string)TempData["MessageDescription"];


            if (!string.IsNullOrWhiteSpace(messagedescription))
            {
                ViewData["MessageDescription"] = JsonSerializer.Deserialize<Notification>(messagedescription);
            }

            BlogPosts = await db.BlogPosts.ToListAsync();
        }
    }
}
