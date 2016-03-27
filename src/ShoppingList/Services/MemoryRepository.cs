using System;
using System.Collections.Generic;
using System.Linq;
using ShoppingList.Models;

namespace ShoppingList.Services {
    public class MemoryRepository : IRepository {
        private readonly List<string> _items = new List<string>(); 
        private readonly List<Models.ShoppingList> _shoppingLists = new List<Models.ShoppingList>();
        private readonly List<Store> _stores = new List<Store>();


        public IEnumerable<string> GetItems() => _items.OrderBy(i => i).ToList();
        public void Add(IEnumerable<string> items)
        {
            var newItems = items.Concat(_items).Distinct().ToList();
            _items.Clear();
            _items.AddRange(newItems);
        }
        public void Remove(IEnumerable<string> items)
        {
            foreach (var item in items)
            {
                _items.Remove(item);
            }
        }

        public IEnumerable<Store> GetStores() => _stores.OrderBy(s => s.Name).ToList();
        public void Save(params Store[] stores) {
            var newStores = stores.Except(_stores);
            _stores.AddRange(newStores);
        }

        public IEnumerable<Models.ShoppingList> GetAllShoppingLists() => _shoppingLists.OrderByDescending(s => s.Date).ToList();
        public Models.ShoppingList GetShoppingList(Guid id) => _shoppingLists.FirstOrDefault(l => l.ID == id);
        public void Save(params Models.ShoppingList[] lists) {
            var newLists = lists.Except(_shoppingLists);
            _shoppingLists.AddRange(newLists);
        }
    }
}