using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Shared.Common {
    public static class IEnumerableExtensions {


        public static IEnumerable<T> TakeRandom<T>(this IEnumerable<T> self, int minCount, int maxCount) {
            var rnd = new Random();
            return Shuffle(self, rnd).Take(rnd.Next(minCount, maxCount));
        }

        /// <summary>
        /// Can optimize..
        /// </summary>
        private static IEnumerable<T> Shuffle<T>(IEnumerable<T> self, Random rnd) {
            return self.OrderBy(i => rnd.Next());
        }

        public static T FirstOr<T>(this IEnumerable<T> self, Func<T, bool> predicate, Func<T> defaultValue) {
            foreach (var item in self.Where(predicate)) {
                return item;
            }
            return defaultValue();
        }
    }
}