using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using ShoppingList.Services;
using ShoppingList.ViewModels;

namespace ShoppingList.Controllers
{
    public class ShoppingListController : Controller {
        private readonly IRepository _repository;
        private readonly StoreFactory _storeFactory;

        public ShoppingListController(IRepository repository, StoreFactory storeFactory) {
            _repository = repository;
            _storeFactory = storeFactory;
        }

        public IActionResult ViewShoppingLists()
        {
            var allLists = _repository.GetAllShoppingLists().OrderBy(l => l.IsComplete);
            return View(allLists.ToList());
        }

        public IActionResult CreateShoppingList()
        {
            var stores = _repository.GetStores().OrderBy(s => s.Name).ToArray();
            var editViewModel = new EditShoppingListViewModel()
            {
                Header = "New List",
                AvailableItems = _repository.GetItems().OrderBy(n => n).ToArray(),
                AvailableStores = stores,
                SelectedItems = new string[0],
                SelectedStore = stores.First(),
                ShopplingListId = Guid.NewGuid()
            };

            return View("EditShoppingList", editViewModel);
        }

        public IActionResult EditShoppingList(Guid id)
        {
            var stores = _repository.GetStores().OrderBy(s => s.Name).ToArray();
            var sl = _repository.GetShoppingList(id);
            var selectedItems = sl.AllItems.Select(i => i.Name).ToArray();
            var availableItems = _repository.GetItems().Except(selectedItems).OrderBy(i => i).ToArray();

            var editViewModel = new EditShoppingListViewModel()
            {
                Header = "Edit List",
                AvailableItems = availableItems,
                AvailableStores = stores,
                SelectedItems = selectedItems,
                SelectedStore = sl.Store,
                ShopplingListId = sl.ID
            };

            return View("EditShoppingList", editViewModel);
        }

        public IActionResult GoShopping(Guid? id) {
            return View(_repository.GetShoppingList(id.Value));
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
                sl = new Models.ShoppingList(store);
            }

            // update shoppinglist
            itemsToBuy = store.SuggestItemOrder(itemsToBuy);
            sl.Store = store;
            sl.ReplaceItems(itemsToBuy);
            _repository.Save(sl);

            // úpdate items
            _repository.Add(itemsToBuy);

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
            _repository.Save(list.Store);

            return new HttpOkResult();
        }
    }
}
