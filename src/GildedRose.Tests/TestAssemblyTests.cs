using System.Reflection;
using GildedRose.Console;
using GildedRose.Console.Interfaces;
using GildedRose.Console.Models;
using Xunit;

namespace GildedRose.Tests;

public class TestAssemblyTests
{
    [Fact]
    public void TestTheTruth()
    {
        Assert.True(true);
    }

    // Normal item: before sell date quality -1, sellin -1
    [Fact]
    public void Normal_BeforeSell_DecreasesQualityByOne()
    {
        var app = new Program
        {
            Items = new List<Item> { new Item { Name = "+5 Dexterity Vest", SellIn = 5, Quality = 10 } }
        };

        app.UpdateQuality();

        var item = app.Items[0];
        Assert.Equal(9, item.Quality);
        Assert.Equal(4, item.SellIn);
    }

    // Normal item: after sell date quality -2
    [Fact]
    public void Normal_AfterSell_DecreasesQualityByTwo()
    {
        var app = new Program
        {
            Items = new List<Item> { new Item { Name = "+5 Dexterity Vest", SellIn = 0, Quality = 10 } }
        };

        app.UpdateQuality();

        var item = app.Items[0];
        Assert.Equal(8, item.Quality); // 10 -> 9 -> 8
        Assert.Equal(-1, item.SellIn);
    }

    // Quality never negative for normal items
    [Fact]
    public void Normal_QualityNeverNegative()
    {
        var app = new Program
        {
            Items = new List<Item> { new Item { Name = "+5 Dexterity Vest", SellIn = 5, Quality = 0 } }
        };

        app.UpdateQuality();

        var item = app.Items[0];
        Assert.Equal(0, item.Quality);
        Assert.Equal(4, item.SellIn);
    }

    // Conjured: before sell decreases by 2
    [Fact]
    public void Conjured_BeforeSell_DecreasesQualityByTwo()
    {
        var app = new Program
        {
            Items = new List<Item> { new Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 6 } }
        };

        app.UpdateQuality();

        var item = app.Items[0];
        Assert.Equal(4, item.Quality);
        Assert.Equal(2, item.SellIn);
    }

    // Conjured: after sell decreases by 4
    [Fact]
    public void Conjured_AfterSell_DecreasesQualityByFour()
    {
        var app = new Program
        {
            Items = new List<Item> { new Item { Name = "Conjured Mana Cake", SellIn = 0, Quality = 6 } }
        };

        app.UpdateQuality();

        var item = app.Items[0];
        Assert.Equal(2, item.Quality); // 6 -> 4 -> 2
        Assert.Equal(-1, item.SellIn);
    }

    // Conjured: quality never negative
    [Fact]
    public void Conjured_QualityNeverNegative()
    {
        var app = new Program
        {
            Items = new List<Item> { new Item { Name = "Conjured Mana Cake", SellIn = 1, Quality = 1 } }
        };

        app.UpdateQuality();

        var item = app.Items[0];
        Assert.Equal(0, item.Quality); // 1 -> 0 (not negative)
    }

    // Aged Brie: before sell increases by 1
    [Fact]
    public void AgedBrie_BeforeSell_IncreasesQualityByOne()
    {
        var app = new Program
        {
            Items = new List<Item> { new Item { Name = "Aged Brie", SellIn = 2, Quality = 0 } }
        };

        app.UpdateQuality();

        var item = app.Items[0];
        Assert.Equal(1, item.Quality);
        Assert.Equal(1, item.SellIn);
    }

    // Aged Brie: after sell increases by 2
    [Fact]
    public void AgedBrie_AfterSell_IncreasesQualityByTwo()
    {
        var app = new Program
        {
            Items = new List<Item> { new Item { Name = "Aged Brie", SellIn = 0, Quality = 10 } }
        };

        app.UpdateQuality();

        var item = app.Items[0];
        Assert.Equal(12, item.Quality); // 10 -> 11 -> 12
        Assert.Equal(-1, item.SellIn);
    }

    // Aged Brie: quality never exceeds 50
    [Fact]
    public void AgedBrie_QualityCappedAt50()
    {
        var app = new Program
        {
            Items = new List<Item> { new Item { Name = "Aged Brie", SellIn = 1, Quality = 50 } }
        };

        app.UpdateQuality();

        var item = app.Items[0];
        Assert.Equal(50, item.Quality);
    }

    // Backstage: more than 10 days +1
    [Fact]
    public void Backstage_MoreThan10_IncreaseByOne()
    {
        var app = new Program
        {
            Items = new List<Item> { new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 15, Quality = 20 } }
        };

        app.UpdateQuality();

        var item = app.Items[0];
        Assert.Equal(21, item.Quality);
        Assert.Equal(14, item.SellIn);
    }

    // Backstage: 10 days or less +2
    [Fact]
    public void Backstage_TenOrLess_IncreaseByTwo()
    {
        var app = new Program
        {
            Items = new List<Item> { new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 10, Quality = 20 } }
        };

        app.UpdateQuality();

        var item = app.Items[0];
        Assert.Equal(22, item.Quality);
        Assert.Equal(9, item.SellIn);
    }

    // Backstage: 5 days or less +3
    [Fact]
    public void Backstage_FiveOrLess_IncreaseByThree()
    {
        var app = new Program
        {
            Items = new List<Item> { new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 5, Quality = 20 } }
        };

        app.UpdateQuality();

        var item = app.Items[0];
        Assert.Equal(23, item.Quality);
        Assert.Equal(4, item.SellIn);
    }

    // Backstage: after concert quality drops to 0
    [Fact]
    public void Backstage_AfterConcert_QualityDropsToZero()
    {
        var app = new Program
        {
            Items = new List<Item> { new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 0, Quality = 20 } }
        };

        app.UpdateQuality();

        var item = app.Items[0];
        Assert.Equal(0, item.Quality);
        Assert.Equal(-1, item.SellIn);
    }

    // Backstage: quality capped at 50
    [Fact]
    public void Backstage_QualityCappedAt50()
    {
        var app = new Program
        {
            Items = new List<Item> { new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 5, Quality = 49 } }
        };

        app.UpdateQuality();

        var item = app.Items[0];
        Assert.Equal(50, item.Quality); // 49 + 3 capped at 50
    }

    // Sulfuras: quality always 80 and sellin never changes
    [Fact]
    public void Sulfuras_AlwaysLegendaryQualityAndNoSellInChange()
    {
        var app = new Program
        {
            Items = new List<Item> { new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 0 } }
        };

        app.UpdateQuality();

        var item = app.Items[0];
        Assert.Equal(80, item.Quality); // enforced to legendary quality
        Assert.Equal(0, item.SellIn);   // unchanged
    }

    //
    // Validation attribute tests (use reflection to reach internal factory and validate enforcement)
    //

    [Fact]
    public void Validation_Throws_When_UpdaterProducesQualityAboveMax()
    {
        // Arrange: create a normal item with an initial Quality > Max (e.g. 60)
        var item = new Item { Name = "+5 Dexterity Vest", SellIn = 5, Quality = 60 };

        // Act: get internal factory via reflection and create updater (which is wrapped by ValidatingUpdater)
        var factoryType = typeof(Program).Assembly.GetType("GildedRose.Console.Factory.ItemUpdaterFactory");
        Assert.NotNull(factoryType);

        var createMethod = factoryType.GetMethod("Create", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        Assert.NotNull(createMethod);

        var updater = (IItemUpdater)createMethod.Invoke(null, new object[] { item });

        // Assert: updater.Update should throw because final Quality remains > MaxQuality
        Assert.Throws<InvalidOperationException>(() => updater.Update(item));
    }

    [Fact]
    public void Validation_Allows_Sulfuras_ToBeCorrectedToLegendaryQuality()
    {
        var item = new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 0 };

        var factoryType = typeof(Program).Assembly.GetType("GildedRose.Console.Factory.ItemUpdaterFactory");
        Assert.NotNull(factoryType);

        var createMethod = factoryType.GetMethod("Create", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        Assert.NotNull(createMethod);

        var updater = (IItemUpdater)createMethod.Invoke(null, new object[] { item });

        // Should not throw; SulfurasUpdater sets Quality to 80 which satisfies Exact constraint
        updater.Update(item);

        Assert.Equal(80, item.Quality);
        Assert.Equal(0, item.SellIn);
    }

    [Fact]
    public void Validation_Throws_For_AgedBrie_When_InitialQualityAboveMax()
    {
        // Aged Brie starting above the max will remain above max after update -> should throw
        var item = new Item { Name = "Aged Brie", SellIn = 1, Quality = 51 };

        var factoryType = typeof(Program).Assembly.GetType("GildedRose.Console.Factory.ItemUpdaterFactory");
        Assert.NotNull(factoryType);

        var createMethod = factoryType.GetMethod("Create", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        Assert.NotNull(createMethod);

        var updater = (IItemUpdater)createMethod.Invoke(null, new object[] { item });

        Assert.Throws<InvalidOperationException>(() => updater.Update(item));
    }

    [Fact]
    public void Validation_DoesNotThrow_For_NormalItem_When_WithinBounds()
    {
        var item = new Item { Name = "+5 Dexterity Vest", SellIn = 3, Quality = 20 };

        var factoryType = typeof(Program).Assembly.GetType("GildedRose.Console.Factory.ItemUpdaterFactory");
        Assert.NotNull(factoryType);

        var createMethod = factoryType.GetMethod("Create", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        Assert.NotNull(createMethod);

        var updater = (IItemUpdater)createMethod.Invoke(null, new object[] { item });

        // Should not throw for valid initial values
        updater.Update(item);

        Assert.InRange(item.Quality, 0, 50);
    }
}