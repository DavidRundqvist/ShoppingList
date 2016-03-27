using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Internal;
using System.Linq;

namespace ShoppingList.Models {
    public class ShoppingList {
        private Item[] _items;

        public Store Store { get; set; }
        public Guid ID { get; }

        public IEnumerable<Item> BoughtItems => _items.Where(item => item.IsBought);
        public IEnumerable<Item> ItemsToBuy => _items.Where(item => !item.IsBought);
        public IEnumerable<Item> AllItems => ItemsToBuy.Concat(BoughtItems);

        public bool IsComplete => AllItems.All(item => item.IsBought);
        
        public ShoppingList(Store store,Guid id, params string[] items)
            : this(store, id, items.Select(name => new Item(name)).ToArray())
        { } 

        public ShoppingList(Store store, Guid id, params Item[] items) {
            Store = store;
            _items = items;
            ID = id;
        }

        public string Description => Store.Name;

        public string Text => string.Join(", ", AllItems.Select(i => i.Name));

        public void BuyItems(params string[] boughtItemNames) {
            var boughtItems = boughtItemNames.Join(_items, name => name, item => item.Name, (name, item) => item).ToList();
            var itemsToBuy = _items.Except(boughtItems);
            foreach (var item in boughtItems) {
                item.IsBought = true;
            }
            foreach(var item in itemsToBuy) {
                item.IsBought = false;
            }
        }       
    }
}