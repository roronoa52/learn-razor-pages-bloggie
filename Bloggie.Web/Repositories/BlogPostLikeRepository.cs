
using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
	public class BlogPostLikeRepository : IBLogPostLikeRepository
	{
		private readonly BloggerDbContext db;

		public BlogPostLikeRepository(BloggerDbContext db)
        {
			this.db = db;
		}

		public async Task AddLikeForBlog(Guid BlogPostId, Guid UserId)
		{
			var Like = new BlogPostLike
			{
				Id = Guid.NewGuid(),
				BlogPostId = BlogPostId,
				UserId = UserId
			};

			await db.BlogPostLike.AddAsync(Like);
			await db.SaveChangesAsync();
		}

		public async Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId)
		{
			return await db.BlogPostLike.Where(x => x.BlogPostId == blogPostId).ToListAsync();
		}

		public async Task<int> GetTotalLikesForBlog(Guid blogId)
		{
			return await db.BlogPostLike.CountAsync(x => x.BlogPostId == blogId);
		}
	}
}
