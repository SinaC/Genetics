using System;
using System.Collections.Generic;
using System.Linq;
using Genetics.Chromosones;

namespace Genetics.CrossOver
{
    public class ThreeParents<T> : ICrossOver<T>
        where T:struct 
    {
        public List<ChromosomeBase<T>> CrossOver(List<ChromosomeBase<T>> parents)
        {
            if (parents == null)
                throw new ArgumentNullException("parents");
            if (parents.Count != 3)
                throw new ArgumentOutOfRangeException("parents", "ThreeParents crossover must be used with 3 parents");
            if (parents.Any(p => p.GeneCount != parents[0].GeneCount))
                throw new ArgumentException("Every parents must have the same number of genes", "parents");

            ChromosomeBase<T> offspring = parents[0].Clone(); // clone is not needed but we cannot create instance of abstract class, so clone is the easiest way to create a new instance

            // for each gene, if parent1 and parent2 have the same value -> use this gene for offspring, otherwise -> use parent3 gene for offspring
            for (int gene = 0; gene < offspring.GeneCount; gene++)
                ChromosomeBase<T>.Merge(parents[0], parents[1], parents[2], gene, offspring);

            return new List<ChromosomeBase<T>>
                {
                    offspring
                };
        }
    }
}
