using System;
using System.Collections.Generic;

namespace Genetics.Chromosones
{
    public abstract class ChromosomeBase<T> : IComparable<ChromosomeBase<T>> 
        where T : struct
    {
        private bool _isDirty; // true if last fitness is not anymore correct
        private double _lastFitness;

        protected readonly Func<ChromosomeBase<T>, double> FitnessFunc;
        protected readonly T[] GeneArray;

        public readonly Guid Id;
        public readonly int GeneCount;

        public IEnumerable<T> Genes
        {
            get { return GeneArray; }
        }

        protected ChromosomeBase(int geneCount, Func<ChromosomeBase<T>, double> fitnessFunc)
        {
            if (geneCount <= 0)
                throw new ArgumentException("geneCount must be greater than 0", "geneCount");
            if (fitnessFunc == null)
                throw new ArgumentNullException("fitnessFunc");

            Id = Guid.NewGuid();
            GeneCount = geneCount;
            GeneArray = new T[geneCount];
            FitnessFunc = fitnessFunc;
            _isDirty = true;
        }

        public abstract void Randomize();
        public abstract void Mutate(int gene);
        public abstract ChromosomeBase<T> Clone();

        public double Fitness
        {
            get
            {
                if (_isDirty)
                    _lastFitness = FitnessFunc(this);
                return _lastFitness;
            }
        }

        public static void Swap(ChromosomeBase<T> parent1, ChromosomeBase<T> parent2, int gene)
        {
            T value = parent1.GeneArray[gene];
            parent1.GeneArray[gene] = parent2.GeneArray[gene];
            parent2.GeneArray[gene] = value;
            parent1._isDirty = true;
            parent2._isDirty = true;
        }

        public static void Merge(ChromosomeBase<T> parent1, ChromosomeBase<T> parent2, ChromosomeBase<T> parent3, int gene, ChromosomeBase<T> offspring)
        {
            // if parent1 and parent2 have the same gene -> use it for offspring, otherwise -> use parent3 gene for offspring
            if (parent1.GeneArray[gene].Equals(parent2.GeneArray[gene]))
                offspring.GeneArray[gene] = parent1.GeneArray[gene];
            else
                offspring.GeneArray[gene] = parent3.GeneArray[gene];
            offspring._isDirty = true;
        }

        #region IComparable<T>

        public int CompareTo(ChromosomeBase<T> other)
        {
            if (Fitness < other.Fitness)
                return -1;
            if (Fitness > other.Fitness)
                return +1;
            return 0;
        }

        #endregion
    }
}
