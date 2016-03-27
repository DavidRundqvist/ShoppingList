using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Internal;
using System.Linq;

namespace ShoppingList.Models {
    public class ShoppingList {
        public Store Store { get; }
        public Item[] Items { get; private set; }
        public Guid ID { get; }


        public ShoppingList(Store store,Guid id, params string[] items)
            : this(store, id, items.Select(name => new Item(name)).ToArray())
        { } 

        public ShoppingList(Store store, Guid id, params Item[] items) {
            Store = store;
            Items = items;
            ID = id;
        }

        /// <summary>
        /// Sorted list of bought items
        /// </summary>
        public IEnumerable<Item> BoughtItems => Items.Where(item => item.IsBought); 

        public string LongDescription => $"{Store.Name}, {Items.Length} items";
        public string ShortDescription => $"{Store.Name}";

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