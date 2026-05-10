using GildedRose.Console.Interfaces;
using GildedRose.Console.Models;
using GildedRose.Console.Updaters;
using System;

namespace GildedRose.Console.Factory
{
    internal static class ItemUpdaterFactory
    {
        public static IItemUpdater Create(Item item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));

            IItemUpdater updater = item.Name switch
            {
                string s when string.Equals(s, "Sulfuras, Hand of Ragnaros", StringComparison.OrdinalIgnoreCase)
                    => new SulfurasUpdater(),

                string s when string.Equals(s, "Aged Brie", StringComparison.OrdinalIgnoreCase)
                    => new AgedBrieUpdater(),

                string s when string.Equals(s, "Backstage passes to a TAFKAL80ETC concert", StringComparison.OrdinalIgnoreCase)
                    => new BackstageUpdater(),

                string s when string.Equals(s, "Conjured Mana Cake", StringComparison.OrdinalIgnoreCase)
                    => new ConjuredUpdater(),

                _ => new NormalUpdater()
            };

            // Wrap with validation so constraints are enforced after each update.
            return new ValidatingUpdater(updater);
        }
    }
}