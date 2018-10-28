using System;
using System.Collections.Generic;

namespace ShoppingList.Shared.Data
{
    public class ShoppingListDTO
    {
        public Guid ID { get; set; }
        public Guid StoreId { get; set; }
        public List<ItemDTO> Items { get; set; }
         
    }
}