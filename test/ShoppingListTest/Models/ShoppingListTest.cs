using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ShoppingListTest.Models
{
    [TestClass]
    public class ShoppingListTest
    {
        ShoppingList.Models.ShoppingList _sut;

        [TestInitialize]
        public void Setup()
        {
            _sut = new ShoppingList.Models.ShoppingList(Guid.NewGuid());
            
        }

        [TestMethod]
        public void Should_reorder_when_buying()
        {
            // arrange
            _sut.Add("A", "B", "C", "D", "E", "F");

            // act
            _sut.Buy("D", "F");
            _sut.Buy("B");

            // assert
            var expectedOrder = new[] {"D", "F", "B", "A", "C", "E"};
            var actualOrder = _sut.AllItemNames.ToArray();
            CollectionAssert.AreEqual(expectedOrder, actualOrder);

        }

        [TestMethod]
        public void Should_reorder_when_unbuying()
        {
            // arrange
            _sut.Add("A", "B", "C", "D", "E", "F");
            _sut.Buy("A", "B", "C", "D");

            // act
            _sut.UnBuy("B", "D");

            // assert
            var expectedOrder = new[] { "A", "C", "B", "D", "E", "F" };
            var actualOrder = _sut.AllItemNames.ToArray();
            CollectionAssert.AreEqual(expectedOrder, actualOrder);

        }

    }
}
