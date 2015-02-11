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

        public EvolutionarySolver(Func<int, ChromosomeBase<T>> generateChromosomeFunc, ISelection<T> selection, ICrossOver<T> crossOver, IMutation<T> mutation)
        {
            if (generateChromosomeFunc == null)
                throw new ArgumentNullException("generateChromosomeFunc");

            _generateChromosomeFunc = generateChromosomeFunc;
            _selection = selection;
            _crossOver = crossOver;
            _mutation = mutation;
        }

        public ChromosomeBase<T> Solve(int geneCount, int populationCount, int maxGenerations, double exitFitness)
        {
            if (geneCount <= 0)
                throw new ArgumentOutOfRangeException("geneCount", "geneCount must be strictly positive");
            if (populationCount <= 0)
                throw new ArgumentOutOfRangeException("populationCount", "populationCount must be strictly positive");
            if (maxGenerations <= 0)
                throw new ArgumentOutOfRangeException("maxGenerations", "maxGenerations must be strictly positive");

            ChromosomeBase<T> best = null;

            // Population initialisation
            _population = new List<ChromosomeBase<T>>(populationCount);
            for (int i = 0; i < populationCount; i++)
            {
                ChromosomeBase<T> chromosome = _generateChromosomeFunc(geneCount);
                chromosome.Randomize();
                UpdateBest(ref best, chromosome);
                _population.Add(chromosome);
            }

            // TODO: no need to sort, partial selection (see elitist selection) could be used to find 3 worst candidates

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

                // Select
                List<ChromosomeBase<T>> parents = _selection.Select(_population, 2).ToList();
                // Reproduce
                ChromosomeBase<T> offspring1, offspring2;
                Reproduce(parents, out offspring1, out offspring2);
                // Replace 2 worst chromosomes with new offsprings
                _population.Sort();
                //System.Diagnostics.Debug.WriteLine("Offspring1 "+offspring1+" replacing " + _population[_population.Count - 1]);
                _population[_population.Count - 1] = offspring1;
               // System.Diagnostics.Debug.WriteLine("Offspring2 "+offspring2 + " replacing " + _population[_population.Count - 2]);
                _population[_population.Count - 2] = offspring2;
                // Replace 3rd worst chromosome with a new one
                ChromosomeBase<T> freshFlesh = _generateChromosomeFunc(geneCount);
                freshFlesh.Randomize();
                //System.Diagnostics.Debug.WriteLine("Fresh flesh "+freshFlesh + " replacing " + _population[_population.Count - 3]);
                _population[_population.Count - 3] = freshFlesh;

                // Check if any of 3 new chromosomes is better than current best
                UpdateBest(ref best, offspring1, offspring2, freshFlesh);

                if (best != null && best.Fitness < exitFitness)
                    break;

                Generation++;
                if (Generation >= maxGenerations)
                    break;
            }

            return best;
        }

        private static void UpdateBest(ref ChromosomeBase<T> best, params ChromosomeBase<T>[] chromosomes)
        {
            foreach (ChromosomeBase<T> chromosome in chromosomes)
                if (best == null || chromosome.Fitness < best.Fitness)
                    best = chromosome;
        }

        // Reproduce 2 chromosomes (crossover + mutation)
        private void Reproduce(List<ChromosomeBase<T>> parents, out ChromosomeBase<T> offspring1, out ChromosomeBase<T> offspring2)
        {
            // Crossover
            List<ChromosomeBase<T>> offsprings = _crossOver.CrossOver(parents);
            
            // Mutation
            _mutation.Mutate(offsprings[0]);
            _mutation.Mutate(offsprings[1]);

            offspring1 = offsprings[0];
            offspring2 = offsprings[1];
        }
    }
}
