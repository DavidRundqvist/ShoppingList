using System;
using System.Collections;
using System.Collections.Generic;
using ShoppingList.Models;

namespace ShoppingList.Services {
    public interface IRepository
    {

        IEnumerable<Store> GetStores();
        void SaveStore(params Store[] stores);
        void RemoveStore(params Guid[] storeIDs);

        IEnumerable<Models.ShoppingList> GetAllShoppingLists();

        /// <summary>
        /// Can return null
        /// </summary>
        Models.ShoppingList GetShoppingList(Guid id);

        IEnumerable<Recipe> GetRecipes();
        void Save(Recipe recipe);
        void Delete(Recipe recipe);



        void AddItem(Guid shoppingListId, string item);
        void RemoveItem(Guid shoppingListId, string item);
        void BuyItem(Guid shoppingListId, string item);
        void UnbuyItem(Guid shoppingListId, string item);

        void SetStore(Guid shoppingListId, string storeName);
        void DeleteShoppingList(Guid id);
    }

}