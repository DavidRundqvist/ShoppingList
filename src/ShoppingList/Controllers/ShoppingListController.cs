using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace ShoppingList.Controllers
{
    public class ShoppingListController : Controller
    {
        public IActionResult ViewShoppingLists()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        public IActionResult EditShoppingList()
        {
            return View();
        }

        public IActionResult GoShopping()
        {
            return View();
        }

    }
}
