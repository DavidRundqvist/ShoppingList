using System;
using System.Reflection.PortableExecutable;

namespace ShoppingList.Models
{
    public class Recipe
    {
        public string[] Items { get; }

        public string Name { get; }

        public Recipe(string name, params string[] items)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name can't be empty", nameof(name));
            Name = name;
            Items = items;
        }
    }
}