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
            var stores = _repository.GetStores().ToList();
            return View(stores);
        }

        private string[] GetItems(string joinedItemsString)
        {
            return (string.IsNullOrEmpty(joinedItemsString) ? Enumerable.Empty<string>() : joinedItemsString.Split('§')).ToArray();
        }


        [HttpPost]
        public IActionResult SaveStores(string storesJoined)
        {
            var storeNames = GetItems(storesJoined);

            var existingStores = _repository.GetStores().ToList();
            var newStoreNames = storeNames.Except(existingStores.Select(store => store.Name));

            var newStores = _factory.CreateStores(newStoreNames.ToArray());
            _repository.Save(newStores);

            return new HttpOkResult();
        }
    }
}