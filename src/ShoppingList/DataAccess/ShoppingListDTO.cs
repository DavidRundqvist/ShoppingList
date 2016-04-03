using System;
using System.Collections.Generic;
using ShoppingList.Models;
using System.Linq;
using ShoppingList.Common;

namespace ShoppingList.DataAccess
{
    public class ShoppingListDTO
    {
        public Guid ID { get; set; }
        public Guid StoreId { get; set; }
        public List<ItemDTO> Items { get; set; }
         
    }

    public static class ShoppingListDTOExtensions
    {
        public static ShoppingListDTO ToDto(this Models.ShoppingList model)
        {
            return new ShoppingListDTO()
            {
                ID = model.ID,
                Items = model.AllItems.Select(i => i.ToDto()).ToList(),
                StoreId = model.Store.ID
            };            
        }

        public static Models.ShoppingList ToModel(this ShoppingListDTO dto, List<Store> stores) {
            var store = stores.FirstOr(s => s.ID == dto.StoreId, () => Store.None);

            var result = new Models.ShoppingList(
                store,
                dto.ID, 
                dto.Items.Select(i => i.ToModel()));
            return result;
        }
    }


}