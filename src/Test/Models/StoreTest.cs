using System;
using System.Collections.Generic;
using NUnit.Framework;
using ShoppingList.Models;
using System.Linq;
using ShoppingList.Services;
using ShoppingList = ShoppingList.Models.ShoppingList;

namespace Test.Models {
    [TestFixture]
    public class StoreTest {
        private Store _sut;
        private readonly ShoppingListFactory _factory = new ShoppingListFactory(new ItemFactory());

        [SetUp]
        public void Setup() {
            _sut = new Store("Test", Guid.NewGuid());
        }

        [TearDown]
        public void TearDown() {}

        [Test]
        public void Should_store_sorted_items() {
            // arrange
            var oldList = _factory.Create(_sut, DateTime.Today, "E", "F", "A", "B", "D", "G", "C");
            foreach (var item in oldList.Items) { item.IsBought = true; }
            _sut.BuyItems(oldList);

            // act
            var newList = _factory.Create(_sut, DateTime.Today, "B", "C", "D", "E");
            foreach (var item in newList.Items) { item.IsBought = true; }
            _sut.BuyItems(newList);

            // assert
            var result = _sut.OrderedItems;
            var expectedItems = new[] {"F", "A", "B", "C", "D", "E", "G"};
            CollectionAssert.AreEqual(expectedItems, result);

        }
    }
}