using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ShoppingList.Models;

namespace ShoppingList.Services {
    public static class IRepositoryExtensions {
        public static IEnumerable<string> GetCommonItems(this IRepository self, int previousThreshold) {
            var allLists = self.GetAllShoppingLists();
            var items = allLists.SelectMany(l => l.AllItems.Select(i => i.Name));

            var groups = items.GroupBy(s => s.ToLowerInvariant());
            var commonGroups = groups.OrderByDescending(g => g.Count());
            return commonGroups
                .Where(g => g.Count() >= previousThreshold)
                .Select(g => g.Key);
        }

        public static IEnumerable<string> GetCommonStores(this IRepository self, int previousThreshold)
        {
            var allLists = self.GetAllShoppingLists();
            var allStores = allLists.Select(s => s.Store);
            var commonStores = allStores
                .GroupBy(s => s)
                .Where(g => g.Count() >= previousThreshold)
                .Select(g => g.Key.Name)
                .OrderBy(n => n);
            return commonStores.Append(Store.None.Name).Distinct();
        }
    }
}