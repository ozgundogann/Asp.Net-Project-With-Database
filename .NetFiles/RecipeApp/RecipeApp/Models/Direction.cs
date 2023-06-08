using System;
using System.Collections.Generic;

namespace RecipeApp.Models;

public partial class Direction
{
    public int Id { get; set; }

    public int? StepNumber { get; set; }

    public string? Description { get; set; }

    public int RecipeId { get; set; }
}
