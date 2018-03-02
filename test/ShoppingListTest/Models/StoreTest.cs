
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingList.Models;

namespace ShoppingListTest.Models {
    [TestClass]
    public class StoreTest {
        private Store _sut;

        [TestInitialize]
        public void Setup() {
            _sut = new Store("Test", Guid.NewGuid());
        }

        [TestCleanup]
        public void TearDown() {}

    }
}