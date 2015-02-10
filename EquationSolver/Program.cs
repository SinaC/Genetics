using System;
using System.Linq;
using Genetics.Chromosones;
using Genetics.CrossOver;
using Genetics.Mutation;
using Genetics.Selection;
using Genetics.Solver;

namespace EquationSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            ISelection<double> selection = new Tournament<double>(0.4);
            //ISelection<double> selection = new Bogo<double>();
            //ISelection<double> selection = new Elitist<double>();
            //ICrossOver<double> crossOver = new OnePoint<double>(3);
            ICrossOver<double> crossOver = new Uniform<double>(0.5);
            IMutation<double> mutation = new ProbabilityMutate<double>(0.2);

            EvolutionarySolver<double> solver = new EvolutionarySolver<double>(
                geneCount => new ChromosomeDouble(geneCount, Evaluation, -10, 10, 0.01), 
                selection, 
                crossOver, 
                mutation);
            ChromosomeBase<double> best = solver.Solve(6, 50, 50000, 0.00001);
        }

        static double Evaluation(ChromosomeBase<double> chromosome)
        {
            // absolute error for hyper-sphere function
            const double trueMin = 0.0;
            double z = chromosome.Genes.Sum(gene => (gene*gene));
            return Math.Abs(trueMin - z);
        }
    }
}
