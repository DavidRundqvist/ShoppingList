using System;
using System.Collections.Generic;
using System.Linq;
using ShoppingList.Models;

namespace ShoppingList.Services {
    public class MemoryRepository : IRepository {
        private readonly List<Item> _items = new List<Item>(); 
        private readonly List<Models.ShoppingList> _shoppingLists = new List<Models.ShoppingList>();
        private readonly List<Store> _stores = new List<Store>();


        public IEnumerable<Item> GetItems() => _items.OrderBy(i => i.Name).ToList();

        public IEnumerable<Models.ShoppingList> GetAllShoppingLists() => _shoppingLists.OrderByDescending(s => s.Date).ToList();

        public Models.ShoppingList GetShoppingList(Guid id) => _shoppingLists.Single(l => l.ID == id);

        public IEnumerable<Store> GetStores() => _stores.OrderBy(s => s.Name).ToList();

        public void Save(IEnumerable<Item> items) {
            var newItems = items.Concat(_items).Distinct().ToList();
            _items.Clear();
            _items.AddRange(newItems);
        }

        public void Save(IEnumerable<Store> stores) {
            var newStores = stores.Concat(_stores).Distinct().ToList();
            _stores.Clear();
            _stores.AddRange(newStores);
        }

        public void Save(IEnumerable<Models.ShoppingList> lists) {
            _shoppingLists.AddRange(lists);
        }
    }
}