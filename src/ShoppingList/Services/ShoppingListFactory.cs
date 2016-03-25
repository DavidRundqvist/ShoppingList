using System;
using System.Linq;
using ShoppingList.Models;
using ShoppingList = ShoppingList.Models.ShoppingList;

namespace ShoppingList.Services {
    public class ShoppingListFactory {
        public Models.ShoppingList Create(Store store, DateTime when, params Item[] items) {
            return new Models.ShoppingList(store, items.ToArray(), when, Guid.NewGuid());
        }

        public Models.ShoppingList Create(Store store, DateTime when, Item[] items, Guid id) {
            return new Models.ShoppingList(store, items, when, id);

        }
    }
}