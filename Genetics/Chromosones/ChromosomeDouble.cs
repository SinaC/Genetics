using System;

namespace Genetics.Chromosones
{
    public class ChromosomeDouble : ChromosomeBase<double>
    {
        public double Min { get; private set; }
        public double Max { get; private set; }
        public double MutationMagnitude { get; private set; }

        public ChromosomeDouble(int geneCount, Func<ChromosomeBase<double>, double> fitnessFunc, double min, double max, double mutationMagnitude)
            : base(geneCount, fitnessFunc)
        {
            Min = min;
            Max = max;
            MutationMagnitude = mutationMagnitude;
        }

        public override void Randomize()
        {
            for (int gene = 0; gene < GeneCount; gene++)
                GeneArray[gene] = (Max - Min) * Singleton.Random.NextDouble() + Min;
        }

        public override void Mutate(int gene)
        {
            double hi = MutationMagnitude * Max;
            double lo = -hi;
            double delta = (hi - lo) * Singleton.Random.NextDouble() + lo;
            GeneArray[gene] += delta;
        }

        public override ChromosomeBase<double> Clone()
        {
            ChromosomeDouble cloned = new ChromosomeDouble(GeneCount, FitnessFunc, Min, Max, MutationMagnitude);
            for (int gene = 0; gene < GeneCount; gene++)
                cloned.GeneArray[gene] = GeneArray[gene];
            return cloned;
        }
    }
}
