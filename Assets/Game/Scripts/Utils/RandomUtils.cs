using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public static class RandomUtils
    {
        public static T GetRandomByWeight<T>(this List<T> items, List<T> excludedItems = null)
            where T : IRandomByWeight
        {
            if (items.Count == 0)
                throw new ArgumentException("Collection cannot be empty", nameof(items));

            var eligibleItems = excludedItems != null ? items.Except(excludedItems).ToList() : items;

            var totalWeight = eligibleItems.Sum(x => x.Weight);
            if (totalWeight <= 0)
                throw new ArgumentException("Total weight must be greater than zero");

            var roll = UnityEngine.Random.Range(0, totalWeight);
            var accumulated = 0;

            foreach (var item in eligibleItems)
            {
                accumulated += item.Weight;
                if (roll < accumulated)
                    return item;
            }

            return eligibleItems[^1];
        }
    }
}