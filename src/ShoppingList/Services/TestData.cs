using System;
using System.Linq;
using ShoppingList.Common;

namespace ShoppingList.Services {
    public class TestData {
        private readonly ItemFactory _itemItemFactory;
        private readonly StoreFactory _storeFactory;
        private readonly ShoppingListFactory _shoppingListFactory;

        public TestData(ItemFactory itemFactory, StoreFactory storeFactory, ShoppingListFactory _shoppingListFactory) {
            _itemItemFactory = itemFactory;
            _storeFactory = storeFactory;
            this._shoppingListFactory = _shoppingListFactory;
        }

        public void InsertTestData(IRepository target) {
            var items = _itemItemFactory.CreateItems("Mjölk", "Bröd", "Ost", "Smör", "Kaffe", "Bacon", "Bananer", "Skinka", "Tandkräm");
            target.Save(items);

            var stores = _storeFactory.CreateStores("Coop Valla", "ICA Maxi", "City Gross", "Systembolaget");
            target.Save(stores);

            var sl1 = _shoppingListFactory.Create(stores[0], DateTime.Today + TimeSpan.FromDays(2), items.TakeRandom(2, 5).ToArray());
            var sl2 = _shoppingListFactory.Create(stores[1], DateTime.Today + TimeSpan.FromDays(1), items.TakeRandom(4, 8).ToArray());
            var sl3 = _shoppingListFactory.Create(stores[2], DateTime.Today + TimeSpan.FromDays(3), items.TakeRandom(3, 10).ToArray());
            target.Save(new[]{sl1, sl2, sl3});
        }
    }
}