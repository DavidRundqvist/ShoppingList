using System.Linq;
using Microsoft.AspNet.Mvc;
using ShoppingList.Services;

namespace ShoppingList.Controllers {
    public class StoresController : Controller {
        private readonly IRepository _repository;
        public StoresController(IRepository repository) {
            _repository = repository;
        }

        public IActionResult ViewStores() {
            var stores = _repository.GetStores().ToList();
            return View(stores);
        }
    }
}