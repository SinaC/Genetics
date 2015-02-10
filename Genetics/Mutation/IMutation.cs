using Genetics.Chromosones;

namespace Genetics.Mutation
{
    public interface IMutation<T>
        where T:struct
    {
        void Mutate(ChromosomeBase<T> chromosome);
    }
}
