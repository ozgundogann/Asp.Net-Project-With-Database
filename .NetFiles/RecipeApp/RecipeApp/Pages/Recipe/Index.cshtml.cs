using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RecipeApp.Models;

namespace RecipeApp.Pages.Recipe
{
	[Authorize]
	public class IndexModel : PageModel
	{
		private readonly RecipeApp.Models.RecipeAppContext _context;

		public IndexModel(RecipeApp.Models.RecipeAppContext context)
		{
			_context = context;
		}
		public string userId { get; set; }

		[BindProperty(SupportsGet = true)]
		public string? SearchString { get; set; }

		[BindProperty(SupportsGet = true)]
		public List<Models.Recipe> FavouriteRecipeList { get; set; }

		public IList<Models.Recipe> Recipe { get; set; } = default!;
		public IList<Models.Favourite> Favourites { get; set; } = default!;
		public List<double> RecipeAverageRatingsList { get; set; } = default!;
		public async Task CalculateRatings()
		{
			var AllRecipes = await (from re in _context.Recipes
									select re).ToListAsync();

			RecipeAverageRatingsList = new List<Double>();

			var AllRatings = await (from ra in _context.Ratings
									select ra).ToListAsync();

			foreach (var item in AllRecipes)
			{
				List<int> recipeRates = await (from x in _context.Ratings
												where x.RecipeId == item.Id
												select x.Value.GetValueOrDefault()).ToListAsync();

				if (recipeRates.Count >= 1)
				{
					double avg = CalculateRate.GetRating(recipeRates);
					avg = Math.Round(avg, 1);
					RecipeAverageRatingsList.Add(avg);
				}
				else
				{
					RecipeAverageRatingsList.Add(0.0);
				}
			}
		}
		

		public async Task OnGetAsync()
		{
			userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (_context.Favourites != null)
			{
				Favourites = await _context.Favourites.ToListAsync();
			}
			if (_context.Recipes != null)
			{
				Recipe = await _context.Recipes.ToListAsync();
				await CalculateRatings();
			}

			var AllRrecipes = await (from re in _context.Recipes
									select re).ToListAsync();
			var AllFavorutes = await (from fa in _context.Favourites
									  select fa).ToListAsync();

            foreach (var item in AllRrecipes)
            {
                foreach (var favitem in AllFavorutes)
                {
                    if(item.Id == favitem.RecipeId && userId == favitem.UserId)
					{
						FavouriteRecipeList.Add(item);
					}
                }
            }



        }

	}
}
