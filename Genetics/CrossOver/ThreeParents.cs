using System;
using System.Collections.Generic;
using System.Linq;
using Genetics.Chromosones;

namespace Genetics.CrossOver
{
    public class ThreeParents<T> : ICrossOver<T>
        where T:struct 
    {
        public IEnumerable<ChromosomeBase<T>> CrossOver(IEnumerable<ChromosomeBase<T>> parents)
        {
            if (parents == null)
                throw new ArgumentNullException("parents");
            ChromosomeBase<T>[] chromosomeBases = parents as ChromosomeBase<T>[] ?? parents.ToArray();
            if (chromosomeBases.Length != 3)
                throw new ArgumentOutOfRangeException("parents", "ThreeParents crossover must be used with 3 parents");
            if (chromosomeBases.Any(p => p.GeneCount != chromosomeBases[0].GeneCount))
                throw new ArgumentException("Every parents must have the same number of genes", "parents");

            ChromosomeBase<T> offspring = chromosomeBases[0].Clone(); // clone is not needed but we cannot create instance of abstract class, so clone is the easiest way to create a new instance

            // for each gene, if parent1 and parent2 have the same value -> use this gene for offspring, otherwise -> use parent3 gene for offspring
            for (int gene = 0; gene < offspring.GeneCount; gene++)
                ChromosomeBase<T>.Merge(chromosomeBases[0], chromosomeBases[1], chromosomeBases[2], gene, offspring);

            return new List<ChromosomeBase<T>>
                {
                    offspring
                };
        }
    }
}
