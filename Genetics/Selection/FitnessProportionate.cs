using System;
using System.Collections.Generic;
using System.Linq;
using Genetics.Chromosones;

namespace Genetics.Selection
{
    //http://en.wikipedia.org/wiki/Fitness_proportionate_selection
    public class FitnessProportionate<T> : ISelection<T>
        where T:struct 
    {
        public IEnumerable<ChromosomeBase<T>> Select(IEnumerable<ChromosomeBase<T>> population, int selectionCount)
        {
            if (population == null)
                throw new ArgumentNullException("population");
            ChromosomeBase<T>[] chromosomeBases = population as ChromosomeBase<T>[] ?? population.ToArray();
            if (selectionCount <= 0 || selectionCount > chromosomeBases.Length)
                throw new ArgumentNullException("selectionCount", "selectionCount must be between 1 and population count");

            double maxFitness = chromosomeBases.Max(x => x.Fitness);
            int popSize = chromosomeBases.Length;
            HashSet<int> selectedIndices = new HashSet<int>();
            for (int i = 0; i < selectionCount; i++)
            {
                while (true)
                {
                    int index = Singleton.Random.Next(popSize);
                    if (selectedIndices.Contains(index))
                        continue;
                    ChromosomeBase<T> candidate = chromosomeBases[index];
                    if (Singleton.Random.NextDouble() * maxFitness < candidate.Fitness)
                        selectedIndices.Add(index);
                }
            }
            List<ChromosomeBase<T>> results = selectedIndices.Select(index => chromosomeBases[index]).ToList();
            return results;
        }
    }
}
