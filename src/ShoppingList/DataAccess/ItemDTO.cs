using ShoppingList.Models;

namespace ShoppingList.DataAccess
{
    public class ItemDTO
    {
        public string Name { get; set; }
        public bool IsBought { get; set; }
    }

    public static class ItemDTOExtensions
    {
        public static ItemDTO ToDto(this Item item)
        {
            return new ItemDTO()
            {
                IsBought = item.IsBought,
                Name = item.Name
            };
        }

        public static Item ToModel(this ItemDTO item)
        {
            return new Item(item.Name, item.IsBought);
        }
    }

}