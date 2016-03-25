using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace ShoppingList.Controllers
{
    public class ItemController : Controller
    {
        public IActionResult ViewItems()
        {
            return View();
        }
    }
}
