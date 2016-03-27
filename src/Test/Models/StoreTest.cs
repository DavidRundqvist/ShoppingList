using System;
using System.Collections.Generic;
using NUnit.Framework;
using ShoppingList.Models;
using System.Linq;
using ShoppingList.Services;


namespace Test.Models {
    [TestFixture]
    public class StoreTest {
        private Store _sut;

        [SetUp]
        public void Setup() {
            _sut = new Store("Test", Guid.NewGuid());
        }

        [TearDown]
        public void TearDown() {}

        [Test]
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