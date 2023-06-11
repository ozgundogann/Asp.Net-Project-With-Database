using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RecipeApp.Models;
using System.Security.Claims;

namespace RecipeApp.Pages.Recipe
{
	public class CommentModel : PageModel

	{
		private readonly RecipeApp.Models.RecipeAppContext _context;

		public CommentModel(RecipeApp.Models.RecipeAppContext context)
		{
			_context = context;
		}
		public class ApplicationDbContext : DbContext
		{
			public DbSet<Comment> Comments { get; set; }
		}

		[BindProperty]
		public Comment Comments { get; set; }

		public List<Comment> CommentsList { get; set; } = new List<Comment>();

		public Models.Recipe Recipe { get; set; } = new Models.Recipe();

		[BindProperty(SupportsGet = true)]
		public int id { get; set; }

		public async Task<IActionResult> OnPostAsync(string _comment)
		{
			var comment = new Comment();
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId != null)
			{
				comment.UserId = userId;
				comment.Content = _comment;
				comment.RecipeId = id;

				_context.Comments.Add(comment);
				await _context.SaveChangesAsync();

			}

			return RedirectToPage();

		}

		public async Task<IActionResult> OnGetAsync()
		{
			var commentsList = await _context.Comments.Where(cl => cl.RecipeId == id).ToListAsync();
			var recipeList = await _context.Recipes.FirstOrDefaultAsync(m => m.Id == id);

			Recipe = recipeList;

			foreach (var comment in commentsList)
			{
				CommentsList.Add(comment);
			}

			return Page();
		}
	}


}
