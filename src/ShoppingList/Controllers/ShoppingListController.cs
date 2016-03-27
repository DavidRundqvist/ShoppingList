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
        private readonly ShoppingListFactory _factory;
        private readonly StoreFactory _storeFactory;

        public ShoppingListController(IRepository repository, ShoppingListFactory factory, StoreFactory storeFactory) {
            _repository = repository;
            _factory = factory;
            _storeFactory = storeFactory;
        }

        public IActionResult ViewShoppingLists()
        {
            var allLists = _repository.GetAllShoppingLists().OrderBy(l => l.IsComplete);
            return View(allLists.ToList());
        }

        public IActionResult CreateShoppingList() {
            var editViewModel = new EditShoppingListViewModel() {
                                                                    AvailableItems = _repository.GetItems().ToArray(),
                                                                    AvailableStores = _repository.GetStores().ToArray(),
                                                                    SelectedItems = new string[0],
                                                                    SelectedStore = _repository.GetStores().First(),
                                                                    ShoppingDate = DateTime.Today + TimeSpan.FromDays(1),
                                                                    ShopplingListId = Guid.NewGuid()
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
            if (sl == null)             {
                sl = _factory.Create(store, itemsToBuy.ToArray());
            }

            // update shoppinglist (todo: replace items)
            sl.Store = store;
            _repository.Save(sl);

            // úpdate items
            _repository.Add(itemsToBuy);

            // update stores
            _repository.Save(store);
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
