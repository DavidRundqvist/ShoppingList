using System;
using System.Collections;
using System.Collections.Generic;
using ShoppingList.Models;

namespace ShoppingList.Services {
    public interface IRepository {
        IEnumerable<string> GetItems();

        IEnumerable<Models.ShoppingList> GetAllShoppingLists();

        Models.ShoppingList GetShoppingList(Guid id);

        IEnumerable<Store> GetStores();

        void Save(IEnumerable<string> items);

        void Save(IEnumerable<Store> stores);

        void Save(IEnumerable<Models.ShoppingList> lists);
    }
}