using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using ShoppingList.DataAccess;
using ShoppingList.Models;
using ShoppingList.Services;
using ShoppingList.ViewModels;

namespace ShoppingList.Controllers
{
    public class ShoppingListController : Controller {
        private readonly IRepository _repository;
        private readonly StoreFactory _storeFactory;
        private readonly SettingsPersistence _settings;

        public ShoppingListController(IRepository repository, StoreFactory storeFactory, SettingsPersistence settings) {
            _repository = repository;
            _storeFactory = storeFactory;
            _settings = settings;
        }


        public IActionResult Start() {

            var allLists = _repository.GetAllShoppingLists().ToList();
            var incompleteList = allLists.FirstOrDefault(s => !s.IsComplete);

            if (incompleteList != null) {
                var hasStore = incompleteList.Store.IsReal;
                return hasStore 
                    ? GoShopping(incompleteList.ID) 
                    : EditShoppingList(incompleteList.ID);
            }
            return CreateShoppingList();

        }


        public IActionResult ViewShoppingLists()
        {
            var allLists = _repository.GetAllShoppingLists().OrderBy(l => l.IsComplete).ToList();
            return View(allLists);
        }

        public IActionResult CreateShoppingList()
        {
            var stores = _repository.GetStores().OrderBy(s => s.Name).ToArray();
            var selectedStore = Store.None;

            var editViewModel = new EditShoppingListViewModel()
            {
                Header = "New List",
                AvailableItems = GetPreviousItems(),
                AvailableStores = stores,
                SelectedItems = new string[0],
                SelectedStore = selectedStore,
                ShopplingListId = Guid.NewGuid()
            };

            return View("EditShoppingList", editViewModel);
        }

        public IActionResult EditShoppingList(Guid id)
        {
            var stores = _repository.GetStores().OrderBy(s => s.Name).ToArray();
            var sl = _repository.GetShoppingList(id);
            var selectedItems = sl.AllItems.Select(i => i.Name).ToArray();

            var editViewModel = new EditShoppingListViewModel() {
                Header = "Edit List",
                AvailableItems = GetPreviousItems(),
                AvailableStores = stores,
                SelectedItems = selectedItems,
                SelectedStore = sl.Store,
                ShopplingListId = sl.ID
            };

            return View("EditShoppingList", editViewModel);
        }

        private string[] GetPreviousItems() {
            return _repository.GetCommonItems().Take(_settings.Load().NumberOfPreviousItems).OrderBy(n => n).ToArray();
        }

        public IActionResult GoShopping(Guid? id) {
            var shoppingList = _repository.GetShoppingList(id.Value);
            return View("GoShopping",shoppingList);
        }

        private string[] GetItems(string joinedItemsString)
        {
            return (string.IsNullOrEmpty(joinedItemsString) ? Enumerable.Empty<string>() : joinedItemsString.Split('§')).ToArray();
        }

        [HttpPost]
        public IActionResult SaveShoppingList(Guid shoppingListId, string storeName, string itemsToBuyJoined)
        {
            var itemsToBuy = GetItems(itemsToBuyJoined);
            var store = _repository.GetStores().FirstOrDefault(s => s.Name == storeName);
            if (store == null) {
                store = _storeFactory.CreateStores(storeName).First();
            }
            var sl = _repository.GetShoppingList(shoppingListId);
            if (sl == null) {
                sl = new Models.ShoppingList(store, shoppingListId);
            }

            // update shoppinglist
            itemsToBuy = store.SuggestItemOrder(itemsToBuy);
            sl.Store = store;
            sl.ReplaceItems(itemsToBuy);
            _repository.Save(sl);

            return new HttpOkResult();
        }

        [HttpPost]
        public IActionResult BuyItems(Guid shoppingListId, string boughtItemsJoined) {
            var boughtItemNames = GetItems(boughtItemsJoined);

            // update shopping list
            var list = _repository.GetShoppingList(shoppingListId);
            list.BuyItems(boughtItemNames);
            _repository.Save(list);

            // update store            
            list.Store.BuyItems(boughtItemNames);
            _repository.SaveStore(list.Store);

            return new HttpOkResult();
        }
    }
}
