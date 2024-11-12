using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
	public class BlogPostCommentRepository : IBlogPostCommentRepository
	{
		private readonly BloggerDbContext db;

		public BlogPostCommentRepository(BloggerDbContext db)
        {
			this.db = db;
		}
        public async Task<BlogPostComment> AddAsync(BlogPostComment comment)
		{
			await db.Comments.AddAsync(comment);
			await db.SaveChangesAsync();
			return comment;
		}

		public async Task<IEnumerable<BlogPostComment>> GetAllAsync(Guid blogPostId)
		{
			return await db.Comments.Where(x => x.BlogPostId  == blogPostId).ToListAsync();
		}
	}
}
