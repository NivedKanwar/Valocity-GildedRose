using GildedRose.Console.Models;

namespace GildedRose.Console.Helpers
{
    internal static class ItemHelpers
    {
        public static void IncreaseQuality(Item item, int amount = 1)
        {
            item.Quality = Math.Min(QualityConstants.MaxQuality, item.Quality + Math.Max(0, amount));
        }

        public static void DecreaseQuality(Item item, int amount = 1)
        {
            item.Quality = Math.Max(QualityConstants.MinQuality, item.Quality - Math.Max(0, amount));
        }
    }
}
