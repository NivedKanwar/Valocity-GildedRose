using System;
using System.Reflection;
using GildedRose.Console.Helpers;
using GildedRose.Console.Interfaces;
using GildedRose.Console.Models;

namespace GildedRose.Console.Updaters
{
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
            if (_constraint != null)
            {
                // For range constraints (Min/Max) enforce the initial state as well.
                // Exact constraints (e.g. Sulfuras) are only enforced after the updater runs
                // to allow the updater to correct the value to the required exact value.
                if (_constraint.Exact == int.MinValue)
                {
                    if (item.Quality < _constraint.Min || item.Quality > _constraint.Max)
                    {
                        throw new InvalidOperationException(
                            $"Item '{item.Name}' initial quality {item.Quality} is outside allowed range [{_constraint.Min},{_constraint.Max}].");
                    }
                }
            }

            _inner.Update(item);

            if (_constraint == null) return;

            // Exact enforcement takes precedence for post-update validation
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