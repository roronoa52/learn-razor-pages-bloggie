using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Pages.Blog
{
    public class DetailsModel : PageModel
    {
		private readonly IBlogPostRepository blogPostRepository;
		private readonly IBLogPostLikeRepository blogPostLikeRepository;
		private readonly UserManager<IdentityUser> userManager;
		private readonly SignInManager<IdentityUser> signInManager;
		private readonly IBlogPostCommentRepository blogPostCommentRepository;

		public BlogPost BlogPost { get; set; }

		public List<BlogComment> Comments { get; set; }
        public int Likes { get; set; }
		public bool Liked { get; set; }

		[BindProperty]
		[Required]
		public string Description { get; set; }

		[BindProperty]
		public Guid BlogPostId { get; set; }

		public DetailsModel(IBlogPostRepository blogPostRepository, IBLogPostLikeRepository blogPostLikeRepository, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IBlogPostCommentRepository blogPostCommentRepository)
        {
			this.blogPostRepository = blogPostRepository;
			this.blogPostLikeRepository = blogPostLikeRepository;
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.blogPostCommentRepository = blogPostCommentRepository;
		}

        public async Task<IActionResult> OnGet(string urlHandle)
        {

			await GetBlog(urlHandle);

			return Page();
        }

		public async Task<IActionResult> OnPost(string urlHandle)
		{
			if (ModelState.IsValid)
			{
				var userId = userManager.GetUserId(User);

				var comment = new BlogPostComment()
				{
					Description = Description,
					BlogPostId = BlogPostId,
					UserId = Guid.Parse(userId),
					DateAdded = DateTime.Now
				};

				await blogPostCommentRepository.AddAsync(comment);

				return RedirectToPage("/blog/details", new { urlHandle = urlHandle });
			}

			await GetBlog(urlHandle); 

			return Page();

		}

		public async Task GetComment()
		{
			var blogPostComments = await blogPostCommentRepository.GetAllAsync(BlogPost.Id);

			var blogCommentsViewModel = new List<BlogComment>();
            foreach (var blogPostComment in blogPostComments)
            {
				blogCommentsViewModel.Add(new BlogComment()
				{
					Username = (await userManager.FindByIdAsync(blogPostComment.UserId.ToString())).UserName,
					Description = blogPostComment.Description,
					DateAdded = blogPostComment.DateAdded,
				});
            }

			Comments = blogCommentsViewModel;
        }

		private async Task GetBlog(string urlHandle)
		{

			BlogPost = await blogPostRepository.GetPostAsync(urlHandle);

			if (BlogPost != null)
			{
				BlogPostId = BlogPost.Id;

				if (signInManager.IsSignedIn(User))
				{
					var likes = await blogPostLikeRepository.GetLikesForBlog(BlogPost.Id);

					var userId = userManager.GetUserId(User);

					Liked = likes.Any(x => x.UserId == Guid.Parse(userId));

					await GetComment();

				}
				Likes = await blogPostLikeRepository.GetTotalLikesForBlog(BlogPost.Id);
			}
		}
    }
}
