using System;
using System.Reflection;
using GildedRose.Console.Helpers;
using GildedRose.Console.Interfaces;
using GildedRose.Console.Models;

namespace GildedRose.Console.Updaters
{
    // This is extra check added on top of actual updaters so that we can be sure that the quality constraints are not violated by any of the updaters.
    // This is especially useful for future-proofing against accidental mistakes when new updaters are added.
    internal class ValidatingUpdater : IItemUpdater
    {
        private readonly IItemUpdater _inner;
        private readonly QualityConstraintAttribute? _constraint;

        public ValidatingUpdater(IItemUpdater inner)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
            _constraint = inner.GetType().GetCustomAttribute<QualityConstraintAttribute>(false);
        }

        public void Update(Item item)
        {
            _inner.Update(item);

            if (_constraint == null) return;

            // Exact enforcement takes precedence
            if (_constraint.Exact != int.MinValue)
            {
                if (item.Quality != _constraint.Exact)
                {
                    throw new InvalidOperationException(
                        $"Item '{item.Name}' quality must be exactly {_constraint.Exact} after update but is {item.Quality}.");
                }
            }
            else
            {
                if (item.Quality < _constraint.Min || item.Quality > _constraint.Max)
                {
                    throw new InvalidOperationException(
                        $"Item '{item.Name}' quality must be within [{_constraint.Min},{_constraint.Max}] after update but is {item.Quality}.");
                }
            }
        }
    }
}