using System;
using System.Collections.Generic;

using System.Linq;

namespace ShoppingList.Models {
    public class ShoppingList : IEquatable<ShoppingList>
    {
        private readonly List<Item> _items;

        public Store Store { get; set; }
        public Guid ID { get; }

        public IEnumerable<Item> BoughtItems => _items.Where(item => item.IsBought);
        public IEnumerable<Item> ItemsToBuy => _items.Where(item => !item.IsBought);
        public IEnumerable<Item> AllItems => _items;
        public IEnumerable<string> AllItemNames => _items.Select(i => i.Name);

        public bool IsComplete => AllItems.All(item => item.IsBought);


        /// <summary>
        /// Creates a new shopping list
        /// </summary>
        public ShoppingList(Guid id) {
            Store = Store.None;
            ID = id;
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

        public void Add(params string[] itemNames)
        {
            var itemsToAdd = itemNames.Except(AllItemNames);
            _items.AddRange(itemsToAdd.Select(name => new Item(name)));
        }

        public void Remove(params string[] itemNames)
        {
            var itemsToRemove = itemNames.Join(_items, name => name, item => item.Name, (name, item) => item).ToArray();
            foreach (var item in itemsToRemove) {
                _items.Remove(item);
            }
        }

        public string Description => Store.Name;

        public string Text => string.Join(", ", AllItems.Select(i => i.Name));

        public void Buy(params string[] boughtItemNames)
        {
            var previouslyBought = _items.Where(i => i.IsBought).ToArray();
            var previouslyNotBought = _items.Where(i => !i.IsBought).ToArray();
            var newboughtItems = boughtItemNames.Join(previouslyNotBought, name => name, item => item.Name, (name, item) => item).ToArray();
            foreach (var item in newboughtItems) {
                item.IsBought = true;
            }

            // Reorder:
            var theRest = _items.Except(previouslyBought).Except(newboughtItems).ToArray();
            _items.Clear();
            _items.AddRange(previouslyBought.Concat(newboughtItems).Concat(theRest));
        }

        public void UnBuy(params string[] unboughtItemNames)
        {
            var previouslyBought = _items.Where(i => i.IsBought).ToArray();
            var newunboughtItems = unboughtItemNames.Join(previouslyBought, name => name, item => item.Name, (name, item) => item).ToArray();
            foreach (var item in newunboughtItems)
            {
                item.IsBought = false;
            }

            // Reorder:
            var currentlyBought = _items.Where(i => i.IsBought).ToArray();
            var theRest = _items.Except(currentlyBought).Except(newunboughtItems).ToArray();
            _items.Clear();
            _items.AddRange(currentlyBought.Concat(newunboughtItems).Concat(theRest));


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


        public void OrderItems(IEnumerable<string> itemOrder)
        {
            var knowns = itemOrder.Join(_items, name => name, item => item.Name, (name, item) => item).ToArray();
            var unknowns = _items.Except(knowns).ToArray();

            _items.Clear();
            _items.AddRange(unknowns);
            _items.AddRange(knowns);

        }

    }
}