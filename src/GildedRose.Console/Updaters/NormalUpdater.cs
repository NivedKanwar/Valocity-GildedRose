using GildedRose.Console.Helpers;
using GildedRose.Console.Interfaces;
using GildedRose.Console.Models;

namespace GildedRose.Console.Updaters
{
    [QualityConstraint(Min = QualityConstants.MinQuality, Max = QualityConstants.MaxQuality)]
    internal class NormalUpdater : IItemUpdater
    {
        public void Update(Item item)
        {
            ItemHelpers.DecreaseQuality(item, 1);
            item.SellIn--;
            if (item.SellIn < 0)
            {
                ItemHelpers.DecreaseQuality(item, 1);
            }
        }
    }
}
