using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
	public interface IBLogPostLikeRepository
	{
		Task<int> GetTotalLikesForBlog(Guid blogId);
		Task AddLikeForBlog(Guid PostBlogId, Guid UserId);
		Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId);

	}
}
