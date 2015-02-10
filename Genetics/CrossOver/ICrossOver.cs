using System.Collections.Generic;
using Genetics.Chromosones;

namespace Genetics.CrossOver
{
    public interface ICrossOver<T>
        where T:struct
    {
        List<ChromosomeBase<T>> CrossOver(List<ChromosomeBase<T>> parents);
    }
}
