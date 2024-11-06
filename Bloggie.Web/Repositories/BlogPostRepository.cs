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
            return await db.BlogPosts.Include(nameof(BlogPost.Tags)).ToListAsync();
        }

		public async Task<IEnumerable<BlogPost>> GetAllAsync(string TagName)
		{
            return await (db.BlogPosts.Include(nameof(BlogPost.Tags))
                .Where(x => x.Tags.Any(x => x.Name == TagName)))
                .ToListAsync();
		}

		public async Task<BlogPost> GetByIdAsync(Guid id)
        {
            return await db.BlogPosts.Include(nameof(BlogPost.Tags)).FirstOrDefaultAsync(x => x.Id == id);
        }

		public async Task<BlogPost> GetPostAsync(string urlHandle)
		{
			return await db.BlogPosts.Include(nameof(BlogPost.Tags)).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
		}

		public async Task<BlogPost> UpdateAsync(BlogPost blogPost)
        {
            var existingPost = await db.BlogPosts.Include(nameof(BlogPost.Tags)).FirstOrDefaultAsync(x => x.Id == blogPost.Id);

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

                if (existingPost.Tags != null && existingPost.Tags.Any()) 
                {
                    // delete all tags existing
                    db.Tags.RemoveRange(existingPost.Tags);

                    // add new tags
                    blogPost.Tags.ToList().ForEach(x => x.BlogPostId = existingPost.Id);

                    await db.Tags.AddRangeAsync(blogPost.Tags);

                }

            }

            await db.SaveChangesAsync();

            return existingPost;
        }
    }
}
