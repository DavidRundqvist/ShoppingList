using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using ShoppingList.Services;

namespace ShoppingList.Controllers
{
    public class ShoppingListController : Controller {
        private readonly IRepository _repository;
        public ShoppingListController(IRepository repository) {
            _repository = repository;
        }

        public IActionResult ViewShoppingLists()
        {
            return View(_repository.GetAllShoppingLists().ToList());
        }

        public IActionResult EditShoppingList()
        {
            return View();
        }

        public IActionResult GoShopping(Guid? id) {
            return View(_repository.GetShoppingList(id.Value));
        }

        [HttpPost]
        public IActionResult BuyItems(Guid shoppingListId, string boughtItemsJoined) {
            var boughtItemNames = string.IsNullOrEmpty(boughtItemsJoined) ? Enumerable.Empty<string>() : boughtItemsJoined.Split('§');
            var list = _repository.GetShoppingList(shoppingListId);
            list.BuyItems(boughtItemNames.ToArray());

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
