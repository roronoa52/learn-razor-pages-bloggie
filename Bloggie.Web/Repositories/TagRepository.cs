using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
	public class TagRepository : ITagRepository
	{
		private readonly BloggerDbContext db;

		public TagRepository(BloggerDbContext db)
        {
			this.db = db;
		}

        public async Task<IEnumerable<Tag>> GetAllAsync()
		{
			var tags = await db.Tags.ToListAsync();

			return tags.DistinctBy(x => x.Name.ToLower());
		}
	}
}
