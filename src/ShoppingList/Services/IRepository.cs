using System;
using System.Collections;
using System.Collections.Generic;
using ShoppingList.Models;

namespace ShoppingList.Services {
    public interface IRepository {
        IEnumerable<string> GetItems();
        void Add(IEnumerable<string> items);
        void Remove(IEnumerable<string> items);

        IEnumerable<Store> GetStores();
        void Save(params Store[] stores);

        IEnumerable<Models.ShoppingList> GetAllShoppingLists();
        Models.ShoppingList GetShoppingList(Guid id);
        void Save(params Models.ShoppingList[] lists);
    }
}