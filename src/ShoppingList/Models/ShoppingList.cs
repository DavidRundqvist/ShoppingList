using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Internal;
using System.Linq;

namespace ShoppingList.Models {
    public class ShoppingList : IEquatable<ShoppingList>
    {
        private readonly List<Item> _items;

        public Store Store { get; set; }
        public Guid ID { get; }

        public IEnumerable<Item> BoughtItems => _items.Where(item => item.IsBought);
        public IEnumerable<Item> ItemsToBuy => _items.Where(item => !item.IsBought);
        public IEnumerable<Item> AllItems => ItemsToBuy.Concat(BoughtItems);

        public bool IsComplete => AllItems.All(item => item.IsBought);


        /// <summary>
        /// Creates a new shopping list
        /// </summary>
        public ShoppingList(Store store) {
            Store = store;
            ID = Guid.NewGuid();
            _items = new List<Item>();
        }

        /// <summary>
        /// Recreates a shopping list
        /// </summary>
        public ShoppingList(Store store, Guid id, IEnumerable<Item> items)
        {
            _items = items.ToList();
            Store = store;
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

        public bool Equals(ShoppingList other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ID.Equals(other.ID);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ShoppingList) obj);
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public void ReplaceItems(params string[] itemsToBuy)
        {
            var currentItems = _items.ToDictionary(item => item.Name);
            _items.Clear();
            _items.AddRange(itemsToBuy.Select(name => currentItems.ContainsKey(name) ? currentItems[name] : new Item(name)));
        }
    }
}