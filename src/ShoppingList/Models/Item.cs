using System;

namespace ShoppingList.Models {
    public class Item : IEquatable<Item> {
        public string Name { get; }

        public Item(string name) {
            Name = name;
        }

        public override string ToString() => Name;
            
        

        public bool Equals(Item other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Item) obj);
        }

        public override int GetHashCode() {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(Name);
        }
    }
}