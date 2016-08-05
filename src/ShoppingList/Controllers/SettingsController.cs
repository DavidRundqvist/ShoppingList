using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ShoppingList.DataAccess;
using ShoppingList.Models;
using ShoppingList.Services;
using ShoppingList.ViewModels;

namespace ShoppingList.Controllers {
    public class SettingsController : Controller {
        private readonly SettingsPersistence _settingsPersistence;

        public SettingsController(SettingsPersistence settingsPersistence) {
            _settingsPersistence = settingsPersistence;
        }

        public IActionResult EditSettings() {
            return View("EditSettings", _settingsPersistence.Load());
        }


        [HttpPost]
        public IActionResult Save(Settings updatedObject) {
            _settingsPersistence.Save(updatedObject);
            return EditSettings();
        }


    }
}