using System.Collections.Generic;
using Genetics.Chromosones;

namespace Genetics.Selection
{
    public interface ISelection<T>
        where T:struct
    {
        List<ChromosomeBase<T>> Select(List<ChromosomeBase<T>> population, int selectionCount);
    }
}
