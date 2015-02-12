using System.Collections.Generic;
using Genetics.Chromosones;

namespace Genetics.CrossOver
{
    public interface ICrossOver<T>
        where T:struct
    {
        IEnumerable<ChromosomeBase<T>> CrossOver(IEnumerable<ChromosomeBase<T>> parents);
    }
}
