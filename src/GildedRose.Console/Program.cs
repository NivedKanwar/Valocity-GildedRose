using System.Collections.Generic;
using System;

namespace GildedRose.Console;

public class Program
{
    public IList<Item> Items = new List<Item>();

    static void Main(string[] args)
    {
        System.Console.WriteLine("OMGHAI!");

        var app = new Program()
                      {
                          Items =
                                      [
                                          new Item {Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20},
                                          new Item {Name = "Aged Brie", SellIn = 2, Quality = 0},
                                          new Item {Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7},
                                          new Item {Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80},
                                          new Item
                                              {
                                                  Name = "Backstage passes to a TAFKAL80ETC concert",
                                                  SellIn = 15,
                                                  Quality = 20
                                              },
                                          new Item {Name = "Conjured Mana Cake", SellIn = 3, Quality = 6}
                                      ]

                      };

        app.UpdateQuality();

        System.Console.ReadKey();
    }

    public void UpdateQuality()
    {
        foreach (var item in Items)
        {
            var updater = ItemUpdaterFactory.Create(item);
            updater.Update(item);
        }
    }
}

public class Item
{
    public string Name { get; set; } = "";

    public int SellIn { get; set; }

    public int Quality { get; set; }
}

internal class SulfurasUpdater : IItemUpdater
{
    public void Update(Item item)
    {
        // Legendary: no changes to SellIn or Quality
    }
}

internal class BackstageUpdater : IItemUpdater
{
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

internal class AgedBrieUpdater : IItemUpdater
{
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

internal static class ItemHelpers
{
    private const int MaxQuality = 50;
    private const int MinQuality = 0;

    public static void IncreaseQuality(Item item, int amount = 1)
    {
        item.Quality = Math.Min(MaxQuality, item.Quality + Math.Max(0, amount));
    }

    public static void DecreaseQuality(Item item, int amount = 1)
    {
        item.Quality = Math.Max(MinQuality, item.Quality - Math.Max(0, amount));
    }
}

public interface IItemUpdater
{
    void Update(Item item);
}

public static class ItemUpdaterFactory
{
    public static IItemUpdater Create(Item item)
    {
        if (item.Name == "Sulfuras, Hand of Ragnaros") return new SulfurasUpdater();
        if (item.Name == "Aged Brie") return new AgedBrieUpdater();
        if (item.Name.Contains("Backstage") || item.Name.Contains("Backstage passes")) return new BackstageUpdater();
        if (item.Name.Contains("Conjured")) return new ConjuredUpdater();
        return new NormalUpdater();
    }
}
