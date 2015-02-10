using System;

namespace Genetics.Chromosones
{
    public class ChromosomeBit : ChromosomeBase<bool>
    {
        public ChromosomeBit(int geneCount, Func<ChromosomeBase<bool>, double> fitnessFunc)
            :base(geneCount, fitnessFunc)
        {
        }

        public override void Randomize()
        {
            for (int i = 0; i < GeneArray.Length; i++)
                GeneArray[i] = Singleton.Random.NextDouble() > 0.5;
        }
        
        public override void Mutate(int gene)
        {
            GeneArray[gene] = !GeneArray[gene];
        }

        public override ChromosomeBase<bool> Clone()
        {
            ChromosomeBit cloned = new ChromosomeBit(GeneCount, FitnessFunc);
            for (int gene = 0; gene < GeneCount; gene++)
                cloned.GeneArray[gene] = GeneArray[gene];
            return cloned;
        }

        public static ChromosomeBit FromString(string bitString, Func<ChromosomeBase<bool>, double> fitnessFunc)
        {
            if (String.IsNullOrEmpty(bitString))
                throw new ArgumentNullException("bitString", "bitString parameter is empty");
            if (fitnessFunc == null)
                throw new ArgumentNullException("fitnessFunc", "fitnessFunc parameter is empty");

            bitString = bitString.Replace(" ", "");

            ChromosomeBit genome = new ChromosomeBit(bitString.Length, fitnessFunc);
            for (int i = 0; i < bitString.Length; i++)
                if (bitString[i] != '0')
                    genome.GeneArray[i] = true;
            return genome;
        }
    }
}
