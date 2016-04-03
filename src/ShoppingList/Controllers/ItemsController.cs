using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using ShoppingList.Services;

namespace ShoppingList.Controllers
{
    public class ItemsController : Controller {
        private readonly IRepository _repository;
        public ItemsController(IRepository repository) {
            _repository = repository;
        }

        public IActionResult ViewItems() {
            var items = _repository.GetItems().OrderBy(i => i).ToList();
            return View(items);
        }

        public IActionResult Delete(string id) {
            _repository.RemoveItem(new[] {id});
            return RedirectToAction("ViewItems");
        }
    }
}
