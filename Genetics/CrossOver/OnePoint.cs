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

        public IEnumerable<ChromosomeBase<T>> CrossOver(IEnumerable<ChromosomeBase<T>> parents)
        {
            if (parents == null)
                throw new ArgumentNullException("parents");
            ChromosomeBase<T>[] chromosomeBases = parents as ChromosomeBase<T>[] ?? parents.ToArray();
            if (chromosomeBases.Length != 2)
                throw new ArgumentOutOfRangeException("parents", "OnePoint crossover must be used with 2 parents");
            if (chromosomeBases.Any(p => p.GeneCount <= From))
                throw new ArgumentException("Every parents must have a number of genes greater than split position (From)", "parents");
            if (chromosomeBases.Any(p => p.GeneCount != chromosomeBases[0].GeneCount))
                throw new ArgumentException("Every parents must have the same number of genes", "parents");

            ChromosomeBase<T> offspring1 = chromosomeBases[0].Clone();
            ChromosomeBase<T> offspring2 = chromosomeBases[1].Clone();

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
