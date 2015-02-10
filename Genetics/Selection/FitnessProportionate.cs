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
        public List<ChromosomeBase<T>> Select(List<ChromosomeBase<T>> population, int selectionCount)
        {
            if (population == null)
                throw new ArgumentNullException("population");
            if (selectionCount <= 0 || selectionCount > population.Count)
                throw new ArgumentNullException("selectionCount", "selectionCount must be between 1 and population count");

            double maxFitness = population.Max(x => x.Fitness);
            int popSize = population.Count;
            int lastSelected = 0;
            int[] selectedIndices = new int[selectionCount];
            for (int i = 0; i < selectionCount; i++)
            {
                while (true)
                {
                    int index = Singleton.Random.Next(popSize);
                    bool isAlreadySelected = false;
                    for (int alreadySelectedIndex = 0; alreadySelectedIndex < lastSelected; alreadySelectedIndex++)
                        if (selectedIndices[alreadySelectedIndex] == index)
                        {
                            isAlreadySelected = true;
                            break;
                        }
                    if (isAlreadySelected)
                        continue;
                    ChromosomeBase<T> candidate = population[index];
                    if (Singleton.Random.NextDouble() * maxFitness < candidate.Fitness)
                    {
                        selectedIndices[lastSelected] = index;
                        lastSelected++;
                        break;
                    }
                }
            }
            List<ChromosomeBase<T>> results = new List<ChromosomeBase<T>>(selectionCount);
            for (int i = 0; i < selectionCount; i++)
                results.Add(population[selectedIndices[i]]);
            return results;
        }
    }
}
