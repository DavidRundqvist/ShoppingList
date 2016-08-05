
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

        [TestMethod]
        public void Should_store_sorted_items() {
            // arrange
            var oldList = new[] { "E", "F", "A", "B", "D", "G", "C" };
            _sut.BuyItems(oldList);

            // act
            var newList = new[] { "B", "C", "D", "E" };
            _sut.BuyItems(newList);

            // assert
            var result = _sut.OrderedItems;
            var expectedItems = new[] {"F", "A", "B", "C", "D", "E", "G"};
            CollectionAssert.AreEqual(expectedItems, result);

        }
    }
}