using System;
using System.Collections.Generic;
using ShoppingList.Shared.Data;

namespace ShoppingList.Server.Services {
    public interface IRepository
    {

        IEnumerable<StoreDTO> GetStores();
        void Save(params StoreDTO[] stores);
        void RemoveStore(params Guid[] storeIDs);

        IEnumerable<ShoppingListDTO> GetShoppingLists();
        /// <summary>
        /// Can return null
        /// </summary>
        ShoppingListDTO GetShoppingList(Guid id);

        void Save(ShoppingListDTO list);
        void DeleteShoppingList(Guid id);



        IEnumerable<RecipeDTO> GetRecipes();
        void Save(RecipeDTO recipe);
        void Delete(RecipeDTO recipe);



        //void AddItem(Guid shoppingListId, string item);
        //void RemoveItem(Guid shoppingListId, string item);
        //void BuyItem(Guid shoppingListId, string item);
        //void UnbuyItem(Guid shoppingListId, string item);

        //void SetStore(Guid shoppingListId, string storeName);
    }

}