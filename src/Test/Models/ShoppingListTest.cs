using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Test.Models
{
    [TestFixture]
    public class ShoppingListTest
    {
        ShoppingList.Models.ShoppingList _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new ShoppingList.Models.ShoppingList(new ShoppingList.Models.Store("Coop", Guid.NewGuid()), Guid.NewGuid());
        }

        [Test]
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
