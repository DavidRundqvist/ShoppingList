using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Models {
    public class Store : IEquatable<Store> {
        public string Name { get; set; }

        public Guid ID { get; }

        private readonly List<string> _orderedItems = new List<string>();

        public string[] OrderedItems
        {
            get { return _orderedItems.ToArray(); }
            set
            {
                _orderedItems.Clear();
                _orderedItems.AddRange(value);
            }
        }

    

        public bool IsReal => ID != None.ID;

        public Store(string name, Guid id) {
            Name = name;
            ID = id;
        }

        public string[] SuggestItemOrder(params string[] items)
        {
            var knownItems = _orderedItems.Intersect(items).ToArray();
            var unknownItems = items.Except(knownItems);
            return unknownItems.Concat(knownItems).ToArray();
        }


        public void BuyItems(params string[] boughtItems) {
            if (!boughtItems.Any())
                return;

            var before = new List<string>();
            var after = new List<string>();

            var foundIndex = _orderedItems.IndexOf(boughtItems.First());
            if (foundIndex >= 0) {
                before = _orderedItems.Take(foundIndex).Except(boughtItems).ToList();
                after = _orderedItems.Except(before).Except(boughtItems).ToList();
            }
            else {
                after = _orderedItems.ToList();
            }

            var allItems = before.Concat(boughtItems).Concat(after);

            // make switch:
            _orderedItems.Clear();
            _orderedItems.AddRange(allItems);
        }


        public static Store None { get; } = new Store("<Unknown>", Guid.Empty);


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