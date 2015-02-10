using System;
using Genetics.Chromosones;

namespace Genetics.Mutation
{
    public class ProbabilityMutate<T> : IMutation<T>
        where T:struct
    {
        public double Probability { get; private set; }

        public ProbabilityMutate(double probability)
        {
            if (probability <= 0 || probability > 1)
                throw new ArgumentOutOfRangeException("probability", "probability must be between 0 and 1");

            Probability = probability;
        }

        public void Mutate(ChromosomeBase<T> chromosome)
        {
            if (chromosome == null)
                throw new ArgumentNullException("chromosome");

            for (int gene = 0; gene < chromosome.GeneCount; gene++)
            {
                double p = Singleton.Random.NextDouble();
                if (p < Probability)
                    chromosome.Mutate(gene);
            }
        }
    }
}
