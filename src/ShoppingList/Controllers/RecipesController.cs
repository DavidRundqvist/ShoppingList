using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ShoppingList.Models;
using ShoppingList.Services;
using ShoppingList.ViewModels;

namespace ShoppingList.Controllers
{
    public class RecipesController : Controller
    {
        private readonly IRepository _repository;

        public RecipesController(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult ViewRecipes()
        {
            return View("ViewRecipes", _repository.GetRecipes().ToList());
        }

        public IActionResult CreateRecipe()
        {
            var newRecipe = new EditRecipeViewModel()
            {
                PreviousItems = _repository.GetCommonItems(0).OrderBy(n => n).ToArray(),
                SelectedItems = new String[0],
                Name = ""
            };
            return View("EditRecipe", newRecipe);
        }

        public IActionResult Edit(string id)
        {
            var recipe = _repository.GetRecipes().FirstOrDefault(r => r.Name.Equals(id, StringComparison.InvariantCultureIgnoreCase));
            var editRecipe = new EditRecipeViewModel()
            {
                PreviousItems = _repository.GetCommonItems(0).OrderBy(n => n).ToArray(),
                SelectedItems = recipe.Items,
                Name = recipe.Name
            };
            return View("EditRecipe", editRecipe);
        }

        [HttpPost]
        public IActionResult Save([FromBody] EditRecipeViewModel editModel)
        {
            var recipe = new Recipe(editModel.Name, editModel.SelectedItems);
            _repository.Save(recipe);
            return new OkResult();
        }

        public IActionResult Delete(string id)
        {
            var recipe = _repository.GetRecipes().FirstOrDefault(r => r.Name.Equals(id, StringComparison.InvariantCultureIgnoreCase));
            if (recipe != null)
            {
                _repository.Delete(recipe);
            }
            return new OkResult();
        }

    }
}