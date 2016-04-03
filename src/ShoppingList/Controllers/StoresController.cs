using System;
using System.Linq;
using Microsoft.AspNet.Mvc;
using ShoppingList.Services;

namespace ShoppingList.Controllers {
    public class StoresController : Controller {
        private readonly IRepository _repository;
        private readonly StoreFactory _factory;

        public StoresController(IRepository repository, StoreFactory factory) {
            _repository = repository;
            _factory = factory;
        }

        public IActionResult ViewStores() {
            var stores = _repository.GetStores().OrderBy(s => s.Name).ToList();
            return View(stores);
        }


        [HttpPost]
        public IActionResult AddStore(string storeName) {            
            var newStores = _factory.CreateStores(storeName);
            _repository.SaveStore(newStores);

            return new HttpOkResult();
        }

        public IActionResult Edit(Guid id) {
            var store = _repository.GetStores().First(s => s.ID == id);
            return View("EditStore", store);
        }

        [HttpPost]
        public IActionResult UpdateStore(Guid storeId, string storeName, string sortedItems) {
            var items = sortedItems.Split('§');
            var store = _repository.GetStores().First(s => s.ID == storeId);
            store.Name = storeName;
            store.OrderedItems = items;
            _repository.SaveStore(store);
            return new HttpOkResult();
        }

        public IActionResult Delete(Guid id) {
            _repository.RemoveStore(id); 
            return RedirectToAction("ViewStores");
        }
    }
}