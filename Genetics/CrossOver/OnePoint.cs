using System;
using System.Collections.Generic;
using System.Linq;
using Genetics.Chromosones;

namespace Genetics.CrossOver
{
    public class OnePoint<T> : ICrossOver<T>
        where T:struct
    {
        public int From { get; private set; }

        public OnePoint(int from)
        {
            From = from;
        }

        public List<ChromosomeBase<T>> CrossOver(List<ChromosomeBase<T>> parents)
        {
            if (parents == null)
                throw new ArgumentNullException("parents");
            if (parents.Count != 2)
                throw new ArgumentOutOfRangeException("parents", "OnePoint crossover must be used with 2 parents");
            if (parents.Any(p => p.GeneCount <= From))
                throw new ArgumentException("Every parents must have a number of genes greater than split position (From)", "parents");
            if (parents.Any(p => p.GeneCount != parents[0].GeneCount))
                throw new ArgumentException("Every parents must have the same number of genes", "parents");

            ChromosomeBase<T> offspring1 = parents[0].Clone();
            ChromosomeBase<T> offspring2 = parents[1].Clone();

            // swap every gene >= from
            for (int gene = From; gene < offspring1.GeneCount; gene++)
                ChromosomeBase<T>.Swap(offspring1, offspring2, gene);

            return new List<ChromosomeBase<T>>
                {
                    offspring1,
                    offspring2
                };
        }
    }
}
