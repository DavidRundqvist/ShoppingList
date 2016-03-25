using System;
using System.Linq;
using ShoppingList.Models;

namespace ShoppingList.Services {
    public class ShoppingListFactory {
        public Models.ShoppingList Create(Store store, DateTime when, params Item[] items) {
            return new Models.ShoppingList(store, items.ToArray(), when, Guid.NewGuid());
        } 
    }
}