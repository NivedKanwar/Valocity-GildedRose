using GildedRose.Console.Helpers;
using GildedRose.Console.Interfaces;
using GildedRose.Console.Models;

namespace GildedRose.Console.Updaters
{
    internal class BackstageUpdater : IItemUpdater
    {
        // Backstage passes increase in Quality as its SellIn value approaches; Quality increases by 2 when there are 10 days or less and by 3 when there are 5 days or less but Quality drops to 0 after the concert.
        //Logic Verified.
        public void Update(Item item)
        {
            if (item.SellIn <= 0)
            {
                // after the concert
                item.Quality = 0;
            }
            else if (item.SellIn <= 5)
            {
                ItemHelpers.IncreaseQuality(item, 3);
            }
            else if (item.SellIn <= 10)
            {
                ItemHelpers.IncreaseQuality(item, 2);
            }
            else
            {
                ItemHelpers.IncreaseQuality(item, 1);
            }

            item.SellIn--;
        }
    }
}
