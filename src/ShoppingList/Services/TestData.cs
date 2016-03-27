using System;
using System.Linq;
using ShoppingList.Common;

namespace ShoppingList.Services {
    public class TestData {
        private readonly StoreFactory _storeFactory;
        private readonly ShoppingListFactory _shoppingListFactory;

        public TestData(StoreFactory storeFactory, ShoppingListFactory shoppingListFactory) {
            _storeFactory = storeFactory;
            _shoppingListFactory = shoppingListFactory;
        }

        public void InsertTestData(IRepository target) {
            var items = new[] {"Mjölk", "Bröd", "Ost", "Smör", "Kaffe", "Bacon", "Bananer", "Skinka", "Tandkräm"};
            target.Save(items);

            var stores = _storeFactory.CreateStores("Coop Valla", "ICA Maxi", "City Gross", "Systembolaget");
            target.Save(stores);

            var sl1 = _shoppingListFactory.Create(stores[0], DateTime.Today + TimeSpan.FromDays(2), items.TakeRandom(2, 5).ToArray(), new Guid("{E753B7F9-152F-4C2C-ACF4-C6D12F3460C3}"));
            var sl2 = _shoppingListFactory.Create(stores[1], DateTime.Today + TimeSpan.FromDays(1), items.TakeRandom(4, 8).ToArray(), new Guid("{E02FE3C4-A742-4054-9A1D-D9904C8375BA}"));
            var sl3 = _shoppingListFactory.Create(stores[2], DateTime.Today + TimeSpan.FromDays(3), items.TakeRandom(3, 10).ToArray(), new Guid("{D7175B1A-FA03-4FA0-A6DF-44DAB79BF1F4}"));
            target.Save(new[]{sl1, sl2, sl3});
        }
    }
}