﻿namespace Food.App.Core.Entities;
public class Recipe : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImagePath { get; set; }
    public ICollection<RecipeTag> Tags { get; set; } = new List<RecipeTag>();
    public int CategoryId { get; set; }
    public Category Category { get; set; }

}
