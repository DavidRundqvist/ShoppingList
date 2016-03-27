using System;
using System.Linq;
using ShoppingList.Models;

namespace ShoppingList.Services {

    public class ShoppingListFactory {
        private readonly ItemFactory _itemFactory;
        public ShoppingListFactory(ItemFactory itemFactory) {
            _itemFactory = itemFactory;
        }

        public Models.ShoppingList Create(Store store, params string[] items) {
            return Create(store, items, Guid.NewGuid());
        }

        public Models.ShoppingList Create(Store store, string[] itemNames, Guid id) {
            var items = _itemFactory.CreateItems(itemNames);
            var itemOrder = store.SuggestItemOrder(items);

            return new Models.ShoppingList(store, id, itemOrder);
        }
    }
}