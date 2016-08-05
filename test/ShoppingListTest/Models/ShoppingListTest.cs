using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ShoppingListTest.Models
{
    [TestClass]
    public class ShoppingListTest
    {
        ShoppingList.Models.ShoppingList _sut;

        [TestInitialize]
        public void Setup()
        {
            _sut = new ShoppingList.Models.ShoppingList(new ShoppingList.Models.Store("Coop", Guid.NewGuid()), Guid.NewGuid());
        }

        [TestMethod]
        public void Should_be_completed_when_all_items_are_bought()
        {
            // arrange
            _sut.ReplaceItems("Bröd", "Kaffe", "Chips");

            // act
            _sut.BuyItems("Bröd", "Kaffe", "Chips");

            // assert
            Assert.IsTrue(_sut.IsComplete);
        }

    }
}
