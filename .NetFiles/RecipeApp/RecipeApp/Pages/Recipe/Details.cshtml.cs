using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using RecipeApp.Models;

namespace RecipeApp.Pages.Recipe
{
	[Authorize]
	public class DetailsModel : PageModel
	{
		private readonly RecipeApp.Models.RecipeAppContext _context;

		public DetailsModel(RecipeApp.Models.RecipeAppContext context)
		{
			_context = context;
		}

		public Models.Recipe Recipe { get; set; } = default!;

		[BindProperty(SupportsGet = true)]
		public int id { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null || _context.Recipes == null)
			{
				return NotFound();
			}

			var recipe = await _context.Recipes.FirstOrDefaultAsync(m => m.Id == id);
			if (recipe == null)
			{
				return NotFound();
			}
			else
			{
				Recipe = recipe;
			}
			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int _rate)
		{
			var rating = new Rating();
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var AllRatings = await (from ra in _context.Ratings
									select ra).ToListAsync();
			//bool flag = true;

			foreach (var item in AllRatings) //Checks whether the user has given more than one rating.
			{
				if(item.RecipeId == id)
					if (userId ==  item.UserId)
					{
						item.Value = _rate;
						_context.Ratings.Update(item);
						await _context.SaveChangesAsync();
						return RedirectToPage("Index");
					}
			}

			if (userId != null /*&& flag*/)
			{
				rating.UserId = userId;
				rating.Value = _rate;
				rating.RecipeId = id;
				_context.Ratings.Add(rating);
				await _context.SaveChangesAsync();
			}
			return RedirectToPage("Index");
		}
	}
}
