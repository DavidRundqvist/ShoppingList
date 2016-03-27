using System.Collections.Generic;
using System.Linq;
using ShoppingList.Models;

namespace ShoppingList.Services {
    public class ItemFactory {
        public Item[] CreateItems(params string[] names) {
            return names.Select(n => new Item(n, isBought: false)).ToArray();
        }
    }
}