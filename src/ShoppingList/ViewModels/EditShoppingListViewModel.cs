using System;
using ShoppingList.Models;

namespace ShoppingList.ViewModels {
    public class EditShoppingListViewModel {
        public string Header { get; set; }
        public Store[] AvailableStores { get; set; } 
        public Store SelectedStore { get; set; }

        public string[] AvailableItems { get; set; }
        public string[] SelectedItems { get; set; }

        public Guid ShopplingListId { get; set; }

    }
}