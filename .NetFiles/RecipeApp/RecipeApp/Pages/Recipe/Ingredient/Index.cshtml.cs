using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RecipeApp.Data;
using RecipeApp.Models;

namespace RecipeApp.Pages.Recipe.Ingredient
{
    public class IndexModel : PageModel
    {
        private readonly RecipeApp.Data.ApplicationDbContext _context;

        [BindProperty(SupportsGet = true)]
        public int id { get; set; }

        public IndexModel(RecipeApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Models.Ingredient> Ingredient { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public Models.Recipe Recipe { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Ingredient != null)
            {
                Ingredient = await _context.Ingredient.ToListAsync();
            }

            int newId = id;
        }
    }
}
