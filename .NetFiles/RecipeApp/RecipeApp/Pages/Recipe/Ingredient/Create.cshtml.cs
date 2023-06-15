using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RecipeApp.Data;
using RecipeApp.Models;

namespace RecipeApp.Pages.Recipe.Ingredient
{
    public class CreateModel : PageModel
    {
        private readonly RecipeApp.Data.ApplicationDbContext _context;

        

        public CreateModel(RecipeApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Models.Ingredient Ingredient { get; set; } = default!;

        [BindProperty (SupportsGet = true)]
        public int id { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Ingredient == null || Ingredient == null)
            {
                return Page();
            }

            Ingredient.RecipeId = id;
            _context.Ingredient.Add(Ingredient);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { id });
        }
    }
}
