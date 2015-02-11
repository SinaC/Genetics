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
        public IEnumerable<ChromosomeBase<T>> Select(IEnumerable<ChromosomeBase<T>> population, int selectionCount)
        {
            throw new NotImplementedException();
        }
    }
}
