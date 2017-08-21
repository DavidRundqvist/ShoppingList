﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingList.DataAccess;
using ShoppingList.Models;
using ShoppingList.Services;
using ShoppingList.ViewModels;
using ShoppingList = ShoppingList.Models.ShoppingList;

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
            var stores = _repository.GetStores().OrderBy(s => s.Name).Select(s => s.Name).ToArray();
            var selectedStore = Store.None.Name;

            var editViewModel = new EditShoppingListViewModel()
            {
                PreviousStores = stores,
                SelectedItems = new string[0],
                SelectedStore = selectedStore,
                ShopplingListId = Guid.NewGuid(),
                PreviousItems = _repository.GetCommonItems(0).OrderBy(n => n).ToArray()
            };

            return View("EditShoppingList", editViewModel);
        }

        public IActionResult EditShoppingList(Guid id)
        {
            var stores = _repository.GetStores().OrderBy(s => s.Name).Select(s => s.Name).ToArray();
            var sl = _repository.GetShoppingList(id);
            var selectedItems = sl.AllItems.Select(i => i.Name).ToArray();
            var editViewModel = new EditShoppingListViewModel() {
                PreviousStores = stores,
                SelectedItems = selectedItems,
                SelectedStore = sl.Store.Name,
                ShopplingListId = sl.ID,
                PreviousItems = _repository.GetCommonItems(0).OrderBy(n => n).ToArray()
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

        public IActionResult SelectStore(Guid shoppingListId, string storeName)
        {
            var sl = _repository.GetShoppingList(shoppingListId);
            var store = _repository.GetStores().FirstOrDefault(s => s.Name == storeName);
            if (store == null)
            {
                store = new Store(storeName, Guid.NewGuid());
                _repository.SaveStore(store);
            }
            sl.Store = store;
            _repository.Save(sl);
            return new OkResult();
        }



        public IActionResult Add(Guid shoppingListId, string itemName)
        {
            var sl = GetOrCreateShoppingList(shoppingListId);
            sl.Add(itemName);
            _repository.Save(sl);
            return new OkResult();
        }
        public IActionResult Remove(Guid shoppingListId, string itemName)
        {
            var sl = GetOrCreateShoppingList(shoppingListId);
            sl.Remove(itemName);
            _repository.Save(sl);
            return new OkResult();
        }



        public IActionResult Buy(Guid shoppingListId, string itemName) {
            var sl = GetOrCreateShoppingList(shoppingListId);
            sl.Buy(itemName);
            _repository.Save(sl);
            return new OkResult();
        }
        public IActionResult UnBuy(Guid shoppingListId, string itemName)
        {
            var sl = GetOrCreateShoppingList(shoppingListId);
            sl.UnBuy(itemName);
            _repository.Save(sl);
            return new OkResult();
        }

        private Models.ShoppingList GetOrCreateShoppingList(Guid shoppingListId)
        {
            var sl = _repository.GetShoppingList(shoppingListId) ?? new Models.ShoppingList(shoppingListId);
            return sl;
        }

    }
}
