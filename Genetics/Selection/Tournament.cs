using System;
using System.Collections.Generic;
using System.Linq;
using Genetics.Chromosones;

namespace Genetics.Selection
{
    //http://en.wikipedia.org/wiki/Tournament_selection
    public class Tournament<T> : ISelection<T>
        where T:struct 
    {
        public double Tau { get; private set; }

        public Tournament(double tau)
        {
            if (tau <= 0 || tau > 1)
                throw new ArgumentOutOfRangeException("tau", "tau must be between 0 and 1");
            Tau = tau;
        }

        public IEnumerable<ChromosomeBase<T>> Select(IEnumerable<ChromosomeBase<T>> population, int selectionCount, bool isPopulationSorted = false)
        {
            if (population == null)
                throw new ArgumentNullException("population");
            ChromosomeBase<T>[] chromosomeBases = population as ChromosomeBase<T>[] ?? population.ToArray();
            if (selectionCount <= 0 || selectionCount > chromosomeBases.Length)
                throw new ArgumentNullException("selectionCount", "selectionCount must be between 1 and population count");

            int popSize = chromosomeBases.Length;
            int[] indexes = new int[popSize];
            for (int i = 0; i < indexes.Length; i++)
                indexes[i] = i;

            for (int i = 0; i < indexes.Length; i++) // shuffle
            {
                int r = Singleton.Random.Next(i, indexes.Length);
                int tmp = indexes[r]; indexes[r] = indexes[i]; indexes[i] = tmp;
            }

            // tau is selection pressure = % of population to grab
            int tournamentSize = (int)(Tau * popSize);
            if (tournamentSize < selectionCount)
                tournamentSize = selectionCount;

            // TODO: Could use partial select sort instead of a full sort, see Elitist
            ChromosomeBase<T>[] candidates = new ChromosomeBase<T>[tournamentSize];
            for (int i = 0; i < tournamentSize; i++)
                candidates[i] = chromosomeBases[indexes[i]];
            Array.Sort(candidates);

            //
            List<ChromosomeBase<T>> results = candidates.Take(selectionCount).ToList();
            return results;
        }
    }
}
