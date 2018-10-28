using ShoppingList.Shared.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingList.Server.Services;

namespace ShoppingList.Server.Controllers
{
    [Route("api/[controller]")]
    public class ShoppingListController : Controller
    {
        private readonly IRepository _repository;

        public ShoppingListController(IRepository repository)
        {
            _repository = repository;
        }



        [HttpGet]
        public ShoppingListDTO[] GetShoppingLists()
        {
            var result = _repository.GetShoppingLists().ToArray();
            return result;
        }

        [HttpGet("{id}", Name = "GetShoppingList")]
        public ActionResult<ShoppingListDTO> GetById(Guid id)
        {
            return _repository.GetShoppingList(id);

        }


        [HttpPost]
        public IActionResult Create(ShoppingListDTO item)
        {
            _repository.Save(item);
            return CreatedAtRoute("GetShoppingList", new { id = item.ID }, item);
        }
    }
}
