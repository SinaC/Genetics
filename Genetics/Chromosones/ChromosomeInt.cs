using System;

namespace Genetics.Chromosones
{
    public class ChromosomeInt : ChromosomeBase<int>
    {
        public int Min { get; private set; }
        public int Max { get; private set; }
        public double MutationMagnitude { get; private set; }

        public ChromosomeInt(int geneCount, Func<ChromosomeBase<int>, double> fitnessFunc, int min, int max, double mutationMagnitude) : base(geneCount, fitnessFunc)
        {
            if (mutationMagnitude < 0 || mutationMagnitude >= 1)
                throw new ArgumentOutOfRangeException("mutationMagnitude", "mutationMagnitude must be between 0 and 1");

            Min = min;
            Max = max;
            MutationMagnitude = mutationMagnitude;
        }

        public override void Randomize()
        {
            for (int gene = 0; gene < GeneCount; gene++)
                GeneArray[gene] = Singleton.Random.Next(Min, Max+1);
        }

        public override void Mutate(int gene)
        {
            int backup = GeneArray[gene];
            double hi = MutationMagnitude * (Max - Min);
            double lo = -hi;
            double delta = (hi - lo) * Singleton.Random.NextDouble() + lo;
            GeneArray[gene] = (GeneArray[gene]+(int)delta).Clamp(Min, Max);
            //if (GeneArray[gene] < Min)
            //    GeneArray[gene] = Min;
            //else if (GeneArray[gene] > Max)
            //    GeneArray[gene] = Max;
        }

        public override ChromosomeBase<int> Clone()
        {
            ChromosomeInt cloned = new ChromosomeInt(GeneCount, FitnessFunc, Min, Max, MutationMagnitude);
            for (int gene = 0; gene < GeneCount; gene++)
                cloned.GeneArray[gene] = GeneArray[gene];
            return cloned;
        }
    }
}
