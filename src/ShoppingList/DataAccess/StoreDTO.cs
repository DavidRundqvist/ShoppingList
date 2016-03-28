﻿using System;
using System.Collections.Generic;
using ShoppingList.Models;
using System.Linq;

namespace ShoppingList.DataAccess
{
    public class StoreDTO
    {
        public string Name { get; set; }
        public Guid ID { get; set; }
        public List<string> OrderedItems { get; set; }
    }

    public static class StoreDTOExtensions
    {
        public static StoreDTO ToDto(this Store store)
        {
            return new StoreDTO()
            {
                ID = store.ID,
                Name = store.Name,
                OrderedItems = store.OrderedItems.ToList()
            };
        }

        public static Store ToModel(this StoreDTO self)
        {
            var result = new Store(self.Name, self.ID);
            result.BuyItems(self.OrderedItems.ToArray());
            return result;
        }
    }
}