using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            var settings = _settings.Load();
            var stores = _repository.GetStores().OrderBy(s => s.Name).Select(s => s.Name).ToArray();
            var selectedStore = Store.None.Name;

            var editViewModel = new EditShoppingListViewModel()
            {
                PreviousItems = _repository.GetCommonItems(settings.PreviousThreshold).Take(settings.NumberOfPreviousItems).OrderBy(n => n).ToArray(),
                AllStores = stores,
                PreviousStores = _repository.GetCommonStores(settings.PreviousThreshold).ToArray(),
                SelectedItems = new string[0],
                SelectedStore = selectedStore,
                ShopplingListId = Guid.NewGuid(),
                AllItems = _repository.GetCommonItems(0).OrderBy(n => n).ToArray()
            };

            return View("EditShoppingList", editViewModel);
        }

        public IActionResult EditShoppingList(Guid id)
        {
            var settings = _settings.Load();

            var stores = _repository.GetStores().OrderBy(s => s.Name).Select(s => s.Name).ToArray();
            var sl = _repository.GetShoppingList(id);
            var selectedItems = sl.AllItems.Select(i => i.Name).ToArray();
            var availableItems = _repository
                .GetCommonItems(settings.PreviousThreshold)
                .Except(selectedItems)
                .Take(settings.NumberOfPreviousItems)
                .OrderBy(n => n)
                .ToArray();
            var editViewModel = new EditShoppingListViewModel() {
                PreviousItems = availableItems,
                AllStores = stores,
                PreviousStores = _repository.GetCommonStores(settings.PreviousThreshold).ToArray(),
                SelectedItems = selectedItems,
                SelectedStore = sl.Store.Name,
                ShopplingListId = sl.ID,
                AllItems = _repository.GetCommonItems(0).OrderBy(n => n).ToArray()
            };

            return View("EditShoppingList", editViewModel);
        }

        public IActionResult GoShopping(Guid? id) {
            var shoppingList = _repository.GetShoppingList(id.Value);
            if (shoppingList.Store.IsReal) {
                return View("GoShopping", shoppingList);
            }
            else {
                return EditShoppingList(id.Value);
            }
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
                _repository.SaveStore(store);
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

            return new OkResult();
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

            return new OkResult();
        }
    }
}
