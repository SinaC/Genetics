using System;
using System.Collections.Generic;
using System.Linq;
using Genetics.Chromosones;
using Genetics.CrossOver;
using Genetics.Mutation;
using Genetics.Selection;

namespace Genetics.Solver
{
    public class EvolutionarySolver<T>
        where T:struct 
    {
        private readonly Func<int, ChromosomeBase<T>> _generateChromosomeFunc;
        private readonly ISelection<T> _selection;
        private readonly ICrossOver<T> _crossOver;
        private readonly IMutation<T> _mutation;

        private List<ChromosomeBase<T>> _population;

        public int Generation { get; private set; }

        public IEnumerable<ChromosomeBase<T>> Population
        {
            get { return _population; }
        }

        public EvolutionarySolver(Func<int, ChromosomeBase<T>> generateChromosomeFunc, ISelection<T> selection, ICrossOver<T> crossOver, IMutation<T> mutation)
        {
            if (generateChromosomeFunc == null)
                throw new ArgumentNullException("generateChromosomeFunc");

            _generateChromosomeFunc = generateChromosomeFunc;
            _selection = selection;
            _crossOver = crossOver;
            _mutation = mutation;
        }

        public ChromosomeBase<T> Solve(int geneCount, int populationCount, int breedCount, int maxGenerations, double exitFitness)
        {
            if (geneCount <= 0)
                throw new ArgumentOutOfRangeException("geneCount", "geneCount must be strictly positive");
            if (populationCount <= 0)
                throw new ArgumentOutOfRangeException("populationCount", "populationCount must be strictly positive");
            if (breedCount <= 0)
                throw new ArgumentOutOfRangeException("breedCount", "breedCount must be strictly positive");
            if (2*breedCount > populationCount-1)
                throw new ArgumentOutOfRangeException("breedCount", "breedCount must be strictly lower than half populationCount");
            if (maxGenerations <= 0)
                throw new ArgumentOutOfRangeException("maxGenerations", "maxGenerations must be strictly positive");

            ChromosomeBase<T> best = null;

            // Population initialisation
            _population = new List<ChromosomeBase<T>>(populationCount);
            for (int i = 0; i < populationCount; i++)
            {
                ChromosomeBase<T> chromosome = _generateChromosomeFunc(geneCount);
                chromosome.Randomize();
                best = UpdateBest(best, chromosome);
                _population.Add(chromosome);
            }

            // Solve
            while (true)
            {
                if (Generation%200 == 0)
                {
                    System.Diagnostics.Debug.WriteLine("Generation = " + Generation);
                    System.Diagnostics.Debug.WriteLine("Best = " + (best == null ? "none" : best.ToString()));
                    foreach(ChromosomeBase<T> chromosome in _population)
                        System.Diagnostics.Debug.WriteLine(chromosome);
                }

                // Sort on Fitness, worst chromosome are at the end of population
                _population.Sort();
                 
                // Generate offsprings replacing worst chromosomes
                //  position 0: fresh flesh
                //  position 1: offspring1 (offspring of parent1 and parent2)
                //  position 2: offspring2 (offspring of parent1 and parent2)
                //  position 3: offspring3 (offspring of parent3 and parent4)
                //  position 4: offspring4 (offspring of parent3 and parent4)
                //  etc...
                int offspringCount = 2*breedCount+1; // +1 for fresh flesh
                ChromosomeBase<T>[] offsprings = new ChromosomeBase<T>[offspringCount]; // offsprings are stored to optimize best search (avoid looping on whole population)
                // Fresh flesh
                ChromosomeBase<T> freshFlesh = _generateChromosomeFunc(geneCount);
                freshFlesh.Randomize();
                offsprings[0] = freshFlesh;
                _population[_population.Count - 1] = offsprings[0]; // replace worst chromosome
                //System.Diagnostics.Debug.WriteLine("fresh -> 0 -> {0}", _population.Count - 1);

                // Select
                ChromosomeBase<T>[] parents = _selection.Select(_population, 2*breedCount).ToArray();
                // Breed by pairs
                for (int i = 0; i < breedCount; i++)
                {
                    ChromosomeBase<T> offspring1, offspring2;
                    Breed(parents.Skip(2*i).Take(2), out offspring1, out offspring2);
                    offsprings[1 + i*2] = offspring1; // 1 + because fresh flesh has been set on position 0
                    offsprings[1 + i*2 + 1] = offspring2;
                    // Replace worst chromosome
                    _population[_population.Count - 2 - i*2] = offspring1;
                    _population[_population.Count - 2 - i*2 - 1] = offspring2;
                    //System.Diagnostics.Debug.WriteLine("{0} -> {1} -> {2}", i*2, 1 + i*2, _population.Count - 2 - i*2);
                    //System.Diagnostics.Debug.WriteLine("{0} -> {1} -> {2}", i*2 + 1, 1 + i*2 + 1, _population.Count - 2 - i*2 - 1);
                }

                // Check if any new chromosomes is better than current best
                best = UpdateBest(best, offsprings);

                if (best != null && best.Fitness < exitFitness)
                    break;

                Generation++;
                if (Generation >= maxGenerations)
                    break;
            }

            return best;
        }

        private static ChromosomeBase<T> UpdateBest(ChromosomeBase<T> best, params ChromosomeBase<T>[] chromosomes)
        {
            if (chromosomes == null || chromosomes.Length == 0)
                return best;
            foreach (ChromosomeBase<T> chromosome in chromosomes)
                if (best == null || chromosome.Fitness < best.Fitness)
                    best = chromosome;
            return best;
        }

        // Breed 2 chromosomes (crossover + mutation)
        private void Breed(IEnumerable<ChromosomeBase<T>> parents, out ChromosomeBase<T> offspring1, out ChromosomeBase<T> offspring2)
        {
            // Crossover
            ChromosomeBase<T>[] offsprings = _crossOver.CrossOver(parents).ToArray();

            if (offsprings.Length != 2)
                throw new ApplicationException("Invalid CrossOver operator: number of offsprings must be 2");
            
            // Mutation
            _mutation.Mutate(offsprings[0]);
            _mutation.Mutate(offsprings[1]);

            offspring1 = offsprings[0];
            offspring2 = offsprings[1];
        }
    }
}
