using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Internal;
using System.Linq;

namespace ShoppingList.Models {
    public class ShoppingList {
        public Store Store { get; }
        public Item[] Items { get; }
        public DateTime Date { get; }
        public Guid ID { get; }


        public ShoppingList(Store store, Item[] items, DateTime date, Guid id) {
            Store = store;
            Items = items;
            Date = date;
            ID = id;
        }


        public string LongDescription => $"{Store.Name} @ {Date.ToString("d")}, {Items.Length} items";
        public string ShortDescription => $"{Store.Name} @ {Date.ToString("d")}";

        public void Save(IEnumerable<string> itemNames) {
            var items = itemNames.Join(Items, name => name, item => item.Name, (name, item) => item);
            Store.CompleteShopping(items);
        }
    }
}