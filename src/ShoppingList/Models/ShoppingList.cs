using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Internal;
using System.Linq;

namespace ShoppingList.Models {
    public class ShoppingList {
        public Store Store { get; }
        public Item[] Items { get; private set; }
        public DateTime Date { get; }
        public Guid ID { get; }


        public ShoppingList(Store store, DateTime date, Guid id, params string[] items)
            : this(store, date, id, items.Select(name => new Item(name)).ToArray())
        { } 

        public ShoppingList(Store store, DateTime date, Guid id, params Item[] items) {
            Store = store;
            Items = items;
            Date = date;
            ID = id;
        }

        /// <summary>
        /// Sorted list of bought items
        /// </summary>
        public IEnumerable<Item> BoughtItems => Items.Where(item => item.IsBought); 

        public string LongDescription => $"{Store.Name} @ {Date.ToString("d")}, {Items.Length} items";
        public string ShortDescription => $"{Store.Name} @ {Date.ToString("d")}";

        public void BuyItems(params string[] boughtItemNames) {
            // order is important
            var boughtItems = boughtItemNames.Join(Items, name => name, item => item.Name, (name, item) => item).ToList();
            var nonBoughtItems = Items.Except(boughtItems).ToList();
            foreach (var item in boughtItems) {
                item.IsBought = true;
            }
            foreach (var item in nonBoughtItems) {
                item.IsBought = false;
            }

            Items = nonBoughtItems.Concat(boughtItems).ToArray();
        }
    }
}