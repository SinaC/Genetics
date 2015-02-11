using System;
using System.Collections.Generic;
using System.Linq;
using Genetics.Chromosones;

namespace Genetics.Selection
{
    // Return 'n' random chromosome
    public class Bogo<T> : ISelection<T>
        where T:struct 
    {
        public IEnumerable<ChromosomeBase<T>> Select(IEnumerable<ChromosomeBase<T>> population, int selectionCount)
        {
            if (population == null)
                throw new ArgumentNullException("population");
            ChromosomeBase<T>[] chromosomeBases = population as ChromosomeBase<T>[] ?? population.ToArray();
            if (selectionCount <= 0 || selectionCount > chromosomeBases.Length)
                throw new ArgumentNullException("selectionCount", "selectionCount must be between 1 and population count");

            List<int> randomIndexes = Singleton.Random.GenerateRandom(2, 0, chromosomeBases.Length);
            List<ChromosomeBase<T>> results = new List<ChromosomeBase<T>>
                {
                    chromosomeBases[randomIndexes[0]],
                    chromosomeBases[randomIndexes[1]]
                };
            return results;
            ////TODO: http://stackoverflow.com/questions/2394246/algorithm-to-select-a-single-random-combination-of-values/2394292#2394292   Communications of the ACM, September 1987, Volume 30, Number 9
            ////TODO: http://codereview.stackexchange.com/questions/61338/generate-random-numbers-without-repetitions/61809#61809
            //List<ChromosomeBase<T>> results = new List<ChromosomeBase<T>>(selectionCount);
            //for(int i = 0; i < selectionCount; i++)
            //{
            //    while(true)
            //    {
            //        int index = Singleton.Random.Next(population.Count);
            //        ChromosomeBase<T> candidate = population[index];
            //        if (!results.Contains(candidate))
            //        {
            //            results.Add(candidate);
            //            break;
            //        }
            //    }
            //}
            //return results;
        }
    }
}
