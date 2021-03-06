﻿using System;
using System.Collections.Generic;
using System.Linq;
using ShoppingList.Models;

namespace ShoppingList.Services {
    public class MemoryRepository : IRepository {
        private readonly List<string> _items = new List<string>(); 
        private readonly List<Models.ShoppingList> _shoppingLists = new List<Models.ShoppingList>();
        private readonly List<Store> _stores = new List<Store>();


        public IEnumerable<string> GetItems() => _items.OrderBy(i => i).ToList();
        public void AddItem(IEnumerable<string> items)
        {
            var newItems = items.Concat(_items).Distinct().ToList();
            _items.Clear();
            _items.AddRange(newItems);
        }
        public void RemoveItem(IEnumerable<string> items)
        {
            foreach (var item in items)
            {
                _items.Remove(item);
            }
        }

        public IEnumerable<Store> GetStores() => _stores.OrderBy(s => s.Name).ToList();
        public void SaveStore(params Store[] stores) {
            var newStores = stores.Except(_stores).Where(s => s.IsReal);
            _stores.AddRange(newStores);
        }

        public void RemoveStore(params Guid[] storeIDs) {
            var storesToRemove = _stores.Join(storeIDs, s => s.ID, id => id, (s, id) => s).ToList();
            foreach (var store in storesToRemove) {
                _stores.Remove(store);
            }
        }

        public IEnumerable<Models.ShoppingList> GetAllShoppingLists() => _shoppingLists.ToList();
        public Models.ShoppingList GetShoppingList(Guid id) => _shoppingLists.FirstOrDefault(l => l.ID == id);
        public void Save(params Models.ShoppingList[] lists) {
            var newLists = lists.Except(_shoppingLists);
            _shoppingLists.InsertRange(0, newLists);
        }

        public IEnumerable<Recipe> GetRecipes()
        {
            throw new NotImplementedException();
        }

        public void Save(Recipe recipe)
        {
            throw new NotImplementedException();
        }

        public void Delete(Recipe recipe)
        {
            throw new NotImplementedException();
        }

        public void AddItem(Guid shoppingListId, string item)
        {
            throw new NotImplementedException();
        }

        public void RemoveItem(Guid shoppingListId, string item)
        {
            throw new NotImplementedException();
        }

        public void BuyItem(Guid shoppingListId, string item)
        {
            throw new NotImplementedException();
        }

        public void UnbuyItem(Guid shoppingListId, string item)
        {
            throw new NotImplementedException();
        }

        public void SetStore(Guid shoppingListId, string storeName)
        {
            throw new NotImplementedException();
        }

        public void DeleteShoppingList(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}