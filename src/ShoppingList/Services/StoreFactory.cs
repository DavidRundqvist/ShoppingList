using System;
using System.Collections.Generic;
using System.Linq;
using ShoppingList.Models;

namespace ShoppingList.Services {
    public class StoreFactory {
        public Store[] CreateStores(params string[] names) {
            return names.Select(n => new Store(n, Guid.NewGuid())).ToArray();
        }
    }
}