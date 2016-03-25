using Microsoft.AspNet.Mvc;

namespace ShoppingList.Controllers {
    public class StoreController : Controller
    {
        public IActionResult ViewStores()
        {
            return View();
        }
    }
}