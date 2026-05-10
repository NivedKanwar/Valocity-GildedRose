using GildedRose.Console.Helpers;
using GildedRose.Console.Interfaces;
using GildedRose.Console.Models;

namespace GildedRose.Console.Updaters
{
    [QualityConstraint(Min = QualityConstants.MinQuality, Max = QualityConstants.MaxQuality)]
    internal class ConjuredUpdater : IItemUpdater
    {
        // Conjured items degrade twice as fast as normal items:
        // - before sell date: 2 per day
        // - after sell date: 4 per day
        public void Update(Item item)
        {
            ItemHelpers.DecreaseQuality(item, 2);
            item.SellIn--;
            if (item.SellIn < 0)
            {
                ItemHelpers.DecreaseQuality(item, 2);
            }
        }
    }
}
