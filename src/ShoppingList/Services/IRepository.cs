using System;
using System.Collections;
using System.Collections.Generic;
using ShoppingList.Models;

namespace ShoppingList.Services {
    public interface IRepository {

        IEnumerable<Store> GetStores();
        void SaveStore(params Store[] stores);
        void RemoveStore(params Guid[] storeIDs);

        IEnumerable<Models.ShoppingList> GetAllShoppingLists();

        /// <summary>
        /// Can return null
        /// </summary>
        Models.ShoppingList GetShoppingList(Guid id);
        void Save(params Models.ShoppingList[] lists);
    }
}