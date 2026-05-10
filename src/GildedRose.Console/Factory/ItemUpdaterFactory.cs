using System;
using GildedRose.Console.Interfaces;
using GildedRose.Console.Models;
using GildedRose.Console.Updaters;

namespace GildedRose.Console.Factory
{
    internal static class ItemUpdaterFactory
    {
        public static IItemUpdater Create(Item item)
        {
            switch (item.Name)
            {
                case string s when string.Equals(s, "Sulfuras, Hand of Ragnaros", StringComparison.OrdinalIgnoreCase):
                    return new SulfurasUpdater();

                case string s when string.Equals(s, "Aged Brie", StringComparison.OrdinalIgnoreCase):
                    return new AgedBrieUpdater();

                case string s when string.Equals(s, "Backstage passes to a TAFKAL80ETC concert", StringComparison.OrdinalIgnoreCase):
                    return new BackstageUpdater();

                case string s when string.Equals(s, "Conjured Mana Cake", StringComparison.OrdinalIgnoreCase):
                    return new ConjuredUpdater();

                default:
                    return new NormalUpdater();
            }
        }
    }
}
