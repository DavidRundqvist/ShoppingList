using System;
using System.Linq;
using ShoppingList.Models;

namespace ShoppingList.ViewModels {
    public class EditShoppingListViewModel {
     
        public string[] PreviousStores { get; set; }
        public string SelectedStore { get; set; }

        public string[] SelectedItems { get; set; }
        public string[] PreviousItems { get; set; }

        public Guid ShopplingListId { get; set; }

        public Recipe[] Recipes { get; set; }

        public string[] CommonItems => PreviousItems.Take(5).ToArray();

    }
}