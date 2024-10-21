using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BloggerDbContext db;

        public BlogPostRepository(BloggerDbContext db)
        {
            this.db = db;
        }
        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await db.BlogPosts.AddAsync(blogPost);

            await db.SaveChangesAsync();

            return blogPost;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingPost = await db.BlogPosts.FindAsync(id);

            if (existingPost != null)
            {
                db.Remove(existingPost);
                await db.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await db.BlogPosts.ToListAsync();
        }

        public async Task<BlogPost> GetByIdAsync(Guid id)
        {
            return await db.BlogPosts.FindAsync(id);
        }

        public async Task<BlogPost> UpdateAsync(BlogPost blogPost)
        {
            var existingPost = await db.BlogPosts.FindAsync(blogPost.Id);

            if (existingPost != null)
            {
                existingPost.Heading = blogPost.Heading;
                existingPost.PageTitle = blogPost.PageTitle;
                existingPost.Content = blogPost.Content;
                existingPost.ShortDescription = blogPost.ShortDescription;
                existingPost.FeturedImageUrl = blogPost.FeturedImageUrl;
                existingPost.UrlHandle = blogPost.UrlHandle;
                existingPost.PublishedDate = blogPost.PublishedDate;
                existingPost.Author = blogPost.Author;
                existingPost.Visible = blogPost.Visible;
            }

            await db.SaveChangesAsync();

            return existingPost;
        }
    }
}
