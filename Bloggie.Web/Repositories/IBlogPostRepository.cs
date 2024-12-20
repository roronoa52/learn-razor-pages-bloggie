﻿using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    public interface IBlogPostRepository
    {
        Task<IEnumerable<BlogPost>> GetAllAsync();

		Task<IEnumerable<BlogPost>> GetAllAsync(string TagName);
		Task<BlogPost> GetByIdAsync(Guid id);
		Task<BlogPost> GetPostAsync(string urlHandle);
		Task<BlogPost> CreateAsync(BlogPost blogPost);
        Task<BlogPost> UpdateAsync(BlogPost blogPost);
        Task<bool> DeleteAsync(Guid id);
    }
}
