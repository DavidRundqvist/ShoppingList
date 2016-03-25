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
        private readonly ItemFactory _factory = new ItemFactory();

        [SetUp]
        public void Setup() {
            _sut = new Store("Test", Guid.NewGuid());
        }

        [TearDown]
        public void TearDown() {}

        [Test]
        public void Should_store_sorted_items() {
            // arrange
            var oldItems = _factory.CreateItems("E", "F", "A", "B", "D", "G", "C");
            _sut.CompleteShopping(oldItems);

            // act
            var newItems = _factory.CreateItems("B", "C", "D", "E");
            _sut.CompleteShopping(newItems);

            // assert
            var result = _sut.OrderedItems;
            var expectedItems = _factory.CreateItems("F", "A", "B", "C", "D", "E", "G");
            CollectionAssert.AreEqual(expectedItems, result);

        }
    }
}