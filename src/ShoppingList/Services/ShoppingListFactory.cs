using System;
using System.Linq;
using ShoppingList.Models;

namespace ShoppingList.Services {

    public class ShoppingListFactory {
        private readonly ItemFactory _itemFactory;
        public ShoppingListFactory(ItemFactory itemFactory) {
            _itemFactory = itemFactory;
        }

        public Models.ShoppingList Create(Store store, DateTime when, params string[] items) {
            return Create(store, when, items, Guid.NewGuid());
        }

        public Models.ShoppingList Create(Store store, DateTime when, string[] items, Guid id) {
            return new Models.ShoppingList(store, _itemFactory.CreateItems(items), when, id);
        }
    }
}