﻿using System;
using System.Collections.Generic;
using System.Linq;
using Genetics.Chromosones;

namespace Genetics.CrossOver
{
    public class TwoPoints<T> : ICrossOver<T>
        where T:struct 
    {
        public int From { get; private set; }
        public int To { get; private set; }

        public TwoPoints(int from, int to)
        {
            if (to < from)
                throw new ArgumentException("From must be greather or equal to To");

            From = from;
            To = to;
        }

        public List<ChromosomeBase<T>> CrossOver(List<ChromosomeBase<T>> parents)
        {
            if (parents == null)
                throw new ArgumentNullException("parents");
            if (parents.Count != 2)
                throw new ArgumentOutOfRangeException("parents", "TwoPoints crossover must be used with 2 parents");
            if (parents.Any(p => p.GeneCount <= From))
                throw new ArgumentException("Every parents must have a number of genes greater than start split position (From)", "parents");
            if (parents.Any(p => p.GeneCount <= To))
                throw new ArgumentException("Every parents must have a number of genes greater than end split position (To)", "parents");
            if (parents.Any(p => p.GeneCount != parents[0].GeneCount))
                throw new ArgumentException("Every parents must have the same number of genes", "parents");

            ChromosomeBase<T> offspring1 = parents[0].Clone();
            ChromosomeBase<T> offspring2 = parents[1].Clone();

            // swap every gene >= from
            for (int gene = From; gene <= To; gene++)
                ChromosomeBase<T>.Swap(offspring1, offspring2, gene);

            return new List<ChromosomeBase<T>>
                {
                    offspring1,
                    offspring2
                };
        }
    }
}
