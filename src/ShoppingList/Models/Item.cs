using System;

namespace ShoppingList.Models {
    public class Item  {
        public string Name { get; }
        public bool IsBought { get; set; }

        public Item(string name, bool isBought = false) {
            Name = name;
            IsBought = isBought;
        }

        public override string ToString() => Name + (IsBought ? ": bought" : ": not bought");
            
        

    }
}