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

		[BindProperty(SupportsGet = true)]
		public bool flag { get; set; }

		[BindProperty(SupportsGet = true)]
		public bool controlVal { get; set; }

		

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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

			var check = await (from c in _context.Favourites
							   select c).ToListAsync();

			foreach (var item in check)
			{
				if (item.UserId == userId && item.RecipeId == id)
					controlVal = true;
			}

			
			


			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int _rate)
		{
			var rating = new Rating();
			var favourite = new Favourite();
			bool check = false;
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var AllRatings = await (from ra in _context.Ratings
									select ra).ToListAsync();

			foreach (var item in AllRatings) //Checks whether the user has given more than one rating.
			{
				if (item.UserId == userId && item.RecipeId == id)
				{
					check = true;
					item.Value = _rate;
					_context.Ratings.Update(item);
					await _context.SaveChangesAsync();
					break;
				}
			}
			if (!check)
			{
				rating.UserId = userId;
				rating.Value = _rate;
				rating.RecipeId = id;

				_context.Ratings.Add(rating);
				await _context.SaveChangesAsync();
			}

			var AllFavourites = await (from fa in _context.Favourites
							   select fa).ToListAsync();

			if (flag)//if checkbox is checked
			{
				favourite.UserId = userId;
				favourite.RecipeId = id;
				_context.Favourites.Add(favourite);
				await _context.SaveChangesAsync();
			}
			else
			{
                foreach (var item in AllFavourites)
                {
					if (userId == item.UserId && id == item.RecipeId)
					{
						_context.Favourites.Remove(item);
						await _context.SaveChangesAsync();
					}
                }
            }
			return RedirectToPage("Index");
		}
	}
}
