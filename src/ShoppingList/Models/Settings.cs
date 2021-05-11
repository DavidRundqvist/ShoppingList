using System.Reflection;

namespace ShoppingList.Models {
    public class Settings {
        public int NumberOfPreviousItems { get; set; } = 20;
        public int PreviousThreshold { get; set; } = 2;

        public string Password { get; set; } = "DefaultPassword";
    }
}