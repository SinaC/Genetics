using System;
using System.Collections.Generic;
using Genetics.Chromosones;

namespace Genetics.Selection
{
    // TODO
    //http://en.wikipedia.org/wiki/Stochastic_universal_sampling
    public class StochasticUniversalSampling<T> : ISelection<T>
        where T:struct 
    {
        public List<ChromosomeBase<T>> Select(List<ChromosomeBase<T>> population, int selectionCount)
        {
            throw new NotImplementedException();
        }
    }
}
