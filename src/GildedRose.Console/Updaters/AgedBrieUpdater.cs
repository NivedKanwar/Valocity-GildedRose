using GildedRose.Console.Helpers;
using GildedRose.Console.Interfaces;
using GildedRose.Console.Models;

namespace GildedRose.Console.Updaters
{
    internal class AgedBrieUpdater : IItemUpdater
    {
        // Aged Brie increases in Quality as it gets older.Logic Verified.
        public void Update(Item item)
        {
            ItemHelpers.IncreaseQuality(item, 1);
            item.SellIn--;
            if (item.SellIn < 0)
            {
                ItemHelpers.IncreaseQuality(item, 1);
            }
        }
    }
}
