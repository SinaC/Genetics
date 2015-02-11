using System.Collections.Generic;
using Genetics.Chromosones;

namespace Genetics.Selection
{
    public interface ISelection<T>
        where T:struct
    {
        IEnumerable<ChromosomeBase<T>> Select(IEnumerable<ChromosomeBase<T>> population, int selectionCount);
    }
}
