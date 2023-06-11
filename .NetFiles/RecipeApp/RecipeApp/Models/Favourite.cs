using System;
using System.Collections.Generic;

namespace RecipeApp.Models;

public partial class Favourite
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public int? RecipeId { get; set; }
}
