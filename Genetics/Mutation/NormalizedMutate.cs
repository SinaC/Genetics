using System;
using Genetics.Chromosones;

namespace Genetics.Mutation
{
    public class NormalizedMutate<T> : IMutation<T>
        where T:struct 
    {
        public void Mutate(ChromosomeBase<T> chromosome)
        {
            if (chromosome == null)
                throw new ArgumentNullException("chromosome");

            double probability = 1.0 / chromosome.GeneCount;
            for (int gene = 0; gene < chromosome.GeneCount; gene++)
            {
                double p = Singleton.Random.NextDouble();
                if (p < probability)
                    chromosome.Mutate(gene);
            }
        }
    }
}
