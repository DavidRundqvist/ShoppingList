using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ShoppingList.Models {
    public class Store : IEquatable<Store> {
        public string Name { get; set; }

        public Guid ID { get; }

        public ShoppingList[] Lists { get; internal set; } = new ShoppingList[0];

        public bool IsReal => ID != None.ID;

        public Store(string name, Guid id) {
            Name = name;
            ID = id;
        }

        public void Order(ShoppingList sl)
        {
            var finishedLists = this.Lists.Where(l => l.IsComplete).ToArray();

            var itemOrder = sl.ItemsToBuy.OrderBy(i => GetWeight(i.Name, finishedLists)).Select(i => i.Name);

            sl.OrderItems(itemOrder);
        }


        private float GetWeight(string item, ShoppingList[] lists)
        {
            var knowns = lists
                .Select(l => GetWeight(item, l))
                .Where(w => w >= 0)
                .ToArray();

            if (knowns.Any())
                return knowns.Average();

            return -1;
        }

        private float GetWeight(string item, ShoppingList list)
        {
            var totalNumber = list.AllItems.Count();
            var index = list.AllItemNames.ToList().IndexOf(item);
            if (index >= 0)
                return (float) index / totalNumber;

            return -1;
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