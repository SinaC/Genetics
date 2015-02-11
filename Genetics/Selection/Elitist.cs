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
        public IEnumerable<ChromosomeBase<T>> Select(IEnumerable<ChromosomeBase<T>> population, int selectionCount)
        {
            if (population == null)
                throw new ArgumentNullException("population");
            ChromosomeBase<T>[] chromosomeBases = population as ChromosomeBase<T>[] ?? population.ToArray();
            if (selectionCount <= 0 || selectionCount > chromosomeBases.Length)
                throw new ArgumentNullException("selectionCount", "selectionCount must be between 1 and population count");

            for(int i = 0; i < selectionCount; i++)
            {
                int minIndex = i;
                double minValue = chromosomeBases[i].Fitness;
                // search the best in remaining items
                for(int j = i+1; j < chromosomeBases.Length; j++)
                    if (chromosomeBases[j].Fitness < minValue)
                    {
                        minIndex = j;
                        minValue = chromosomeBases[j].Fitness;
                    }
                // save best
                ChromosomeBase<T> temp = chromosomeBases[i];
                chromosomeBases[i] = chromosomeBases[minIndex];
                chromosomeBases[minIndex] = temp;
            }
            return chromosomeBases.Take(selectionCount).ToList();
        }
    }
}
