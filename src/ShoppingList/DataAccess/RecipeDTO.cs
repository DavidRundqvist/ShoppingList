using System.Collections.Generic;
using System.Linq;
using ShoppingList.Models;

namespace ShoppingList.DataAccess
{
    public class RecipeDTO
    {
        public string Name { get; set; }

        public List<string> Items { get; set; }
    }

    public static class RecipeDTOExtensions {


        public static RecipeDTO ToDTO(this Recipe self)
        {
            return new RecipeDTO {Items = self.Items.ToList(), Name = self.Name};
        }

        public static Recipe ToModel(this RecipeDTO self)
        {
            return new Recipe(self.Name, self.Items.ToArray());
        }

    }
}