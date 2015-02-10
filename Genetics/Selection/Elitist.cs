using System;
using System.Collections.Generic;
using System.Linq;
using Genetics.Chromosones;

namespace Genetics.Selection
{
    // Select only the best candidates
    public class Elitist<T> : ISelection<T>
        where T:struct
    {
        //http://en.wikipedia.org/wiki/Selection_algorithm#Partial_selection_sort
        public List<ChromosomeBase<T>> Select(List<ChromosomeBase<T>> population, int selectionCount)
        {
            if (population == null)
                throw new ArgumentNullException("population");
            if (selectionCount <= 0 || selectionCount > population.Count)
                throw new ArgumentNullException("selectionCount", "selectionCount must be between 1 and population count");

            for(int i = 0; i < selectionCount; i++)
            {
                int minIndex = i;
                double minValue = population[i].Fitness;
                // search the best in remaining items
                for(int j = i+1; j < population.Count; j++)
                    if (population[j].Fitness < minValue)
                    {
                        minIndex = j;
                        minValue = population[j].Fitness;
                    }
                // save best
                ChromosomeBase<T> temp = population[i];
                population[i] = population[minIndex];
                population[minIndex] = temp;
            }
            return population.Take(selectionCount).ToList();
        }
    }
}
