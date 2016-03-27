using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Models {
    public class Store : IEquatable<Store> {
        public string Name { get; private set; }

        public Guid ID { get; }

        private readonly List<string> _orderedItems = new List<string>(); 
        public IReadOnlyList<string> OrderedItems => _orderedItems;


        public Store(string name, Guid id) {
            Name = name;
            ID = id;
        }

        public Item[] SuggestItemOrder(Item[] items) {
            var knownItems = _orderedItems.Join(items, name => name, item => item.Name, (name, item) => item).ToList();
            var unknownItems = items.Except(knownItems);
            return unknownItems.Concat(knownItems).ToArray();
        }


        public void BuyItems(ShoppingList list) {
            var newOrderedItems = list.BoughtItems.Select(item => item.Name).ToList();
            if (!newOrderedItems.Any())
                return;

            var before = new List<string>();
            var after = new List<string>();

            var foundIndex = _orderedItems.IndexOf(newOrderedItems.First());
            if (foundIndex >= 0) {
                before = _orderedItems.Take(foundIndex).Except(newOrderedItems).ToList();
                after = _orderedItems.Except(before).Except(newOrderedItems).ToList();
            }
            else {
                after = _orderedItems.ToList();
            }

            var allItems = before.Concat(newOrderedItems).Concat(after);

            // make switch:
            _orderedItems.Clear();
            _orderedItems.AddRange(allItems);
        }

        public bool Equals(Store other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ID.Equals(other.ID);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Store) obj);
        }

        public override int GetHashCode() {
            return ID.GetHashCode();
        }

    }
}