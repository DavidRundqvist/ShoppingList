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

        public Models.ShoppingList Create(Store store, DateTime when, string[] itemNames, Guid id) {
            var items = _itemFactory.CreateItems(itemNames);
            var itemOrder = store.SuggestItemOrder(items);

            return new Models.ShoppingList(store, when, id, itemOrder);
        }
    }
}