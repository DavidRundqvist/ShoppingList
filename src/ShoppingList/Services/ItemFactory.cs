using System.Collections.Generic;
using System.Linq;
using ShoppingList.Models;

namespace ShoppingList.Services {
    public class ItemFactory {
        public List<Item> CreateItems(params string[] names) {
            return names.Select(n => new Item(n)).ToList();
        }
    }
}