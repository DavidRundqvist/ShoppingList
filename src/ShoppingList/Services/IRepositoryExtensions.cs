using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Services {
    public static class IRepositoryExtensions {
        public static IEnumerable<string> GetCommonItems(this IRepository self) {
            var allLists = self.GetAllShoppingLists();
            var items = allLists.SelectMany(l => l.AllItems.Select(i => i.Name));

            var groups = items.GroupBy(s => s.ToLowerInvariant());
            var commonGroups = groups.OrderByDescending(g => g.Count());
            return commonGroups.Take(15).Select(g => g.Key);

        }
    }
}