using System;
using System.Collections.Generic;
using System.Linq;
using Genetics.Chromosones;

namespace Genetics.CrossOver
{
    public class Uniform<T> : ICrossOver<T>
        where T:struct 
    {
        public double MixingRatio { get; private set; }

        public Uniform(double mixingRatio)
        {
            if (mixingRatio <= 0 || mixingRatio > 1)
                throw new ArgumentOutOfRangeException("mixingRatio", "mixingRatio must be between 0 and 1");

            MixingRatio = mixingRatio;
        }

        public List<ChromosomeBase<T>> CrossOver(List<ChromosomeBase<T>> parents)
        {
            if (parents == null)
                throw new ArgumentNullException("parents");
            if (parents.Count != 2)
                throw new ArgumentOutOfRangeException("parents", "Uniform crossover must be used with 2 parents");
            if (parents.Any(p => p.GeneCount != parents[0].GeneCount))
                throw new ArgumentException("Every parents must have the same number of genes", "parents");

            ChromosomeBase<T> offspring1 = parents[0].Clone();
            ChromosomeBase<T> offspring2 = parents[1].Clone();

            // for each gene, check probability and swap if needed
            for (int gene = 0; gene < offspring1.GeneCount; gene++)
            {
                double p = Singleton.Random.NextDouble();
                if (p < MixingRatio)
                    ChromosomeBase<T>.Swap(offspring1, offspring2, gene);
            }

            return new List<ChromosomeBase<T>>
                {
                    offspring1,
                    offspring2
                };
        }
    }
}
