using System.Collections.Generic;

namespace ShoppingList.Shared.Data
{
    public class RecipeDTO
    {
        public string Name { get; set; }

        public List<string> Items { get; set; }
    }
}