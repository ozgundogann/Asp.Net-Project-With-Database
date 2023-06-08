using System;
using System.Collections.Generic;

namespace RecipeApp.Models;

public partial class Ingredient
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public double? Quantity { get; set; }

    public double? Unit { get; set; }

    public int? RecipeId { get; set; }
}
