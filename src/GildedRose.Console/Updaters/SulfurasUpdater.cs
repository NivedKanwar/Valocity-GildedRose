using GildedRose.Console.Interfaces;
using GildedRose.Console.Models;
using GildedRose.Console.Helpers;

namespace GildedRose.Console.Updaters
{
    internal class SulfurasUpdater : IItemUpdater
    {
        public void Update(Item item)
        {
            // Ensure legendary item always has authoritative, constant quality and never ages.
            item.Quality = QualityConstants.LegendaryQuality;
            // Do not change SellIn.
        }
    }
}
