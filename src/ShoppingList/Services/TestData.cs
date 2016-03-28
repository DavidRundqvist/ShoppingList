using System;
using System.Linq;
using ShoppingList.Common;
using ShoppingList.Models;

namespace ShoppingList.Services {
    public class TestData {
        private readonly StoreFactory _storeFactory;

        public TestData(StoreFactory storeFactory) {
            _storeFactory = storeFactory;
        }

        public void InsertTestData(IRepository target) {
            var items = new[] {"mjölk", "bröd", "ost", "smör", "kaffe", "bacon", "bananer", "skinka", "tandkräm"};
            target.Add(items);

            var stores = _storeFactory.CreateStores("coop valla", "ica maxi", "city gross", "systembolaget");
            target.Save(stores);

            var sl1 = Create(stores[0], items.TakeRandom(5, 15).ToArray(), new Guid("{E753B7F9-152F-4C2C-ACF4-C6D12F3460C3}"));
            var sl2 = Create(stores[1], items.TakeRandom(4, 8).ToArray(), new Guid("{E02FE3C4-A742-4054-9A1D-D9904C8375BA}"));
            var sl3 = Create(stores[2], items.TakeRandom(3, 10).ToArray(), new Guid("{D7175B1A-FA03-4FA0-A6DF-44DAB79BF1F4}"));
            foreach(var item in sl3.AllItems)             {
                item.IsBought = true;
            }
            target.Save(sl1, sl2, sl3);
        }
        public Models.ShoppingList Create(Store store, string[] itemNames, Guid id)
        {
            return new Models.ShoppingList(store, id, itemNames.Select(s => new Item(s)));
        }

    }
}