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

        public ShoppingListController(IRepository repository, ShoppingListFactory factory) {
            _repository = repository;
            _factory = factory;
        }

        public IActionResult ViewShoppingLists()
        {
            return View(_repository.GetAllShoppingLists().ToList());
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


        [HttpPost]
        public IActionResult SaveShoppingList(Guid shoppingListId, Guid storeId, string itemsToBuyJoined)
        {
            var itemsToBuy = string.IsNullOrEmpty(itemsToBuyJoined) ? Enumerable.Empty<string>() : itemsToBuyJoined.Split('§');
            var store = _repository.GetStores().First(s => s.ID == storeId);
            var sl = _repository.GetShoppingList(shoppingListId);
            if (sl == null)             {
                sl = _factory.Create(store, itemsToBuy.ToArray());
            }

            _repository.Save(sl);
            return new HttpOkResult();

        }


        [HttpPost]
        public IActionResult BuyItems(Guid shoppingListId, string boughtItemsJoined) {
            var boughtItemNames = string.IsNullOrEmpty(boughtItemsJoined) ? Enumerable.Empty<string>() : boughtItemsJoined.Split('§');

            // update shopping list
            var list = _repository.GetShoppingList(shoppingListId);
            list.BuyItems(boughtItemNames.ToArray());
            _repository.Save(list);

            // update store
            list.Store.BuyItems(list);
            _repository.Save(list.Store);

            return new HttpOkResult();
        }

        //[HttpPost]
        //public IActionResult BuyItems(Models.ShoppingList list, string boughtItemsJoined) {
        //    var boughtItemNames = string.IsNullOrEmpty(boughtItemsJoined) ? Enumerable.Empty<string>() : boughtItemsJoined.Split('§');
        //    list.BuyItems(boughtItemNames.ToArray());
        //    return new HttpOkResult();
        //}

    }
}
