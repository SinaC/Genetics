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
            //ISelection<double> selection = new Tournament<double>(0.4);
            ////ISelection<double> selection = new Bogo<double>();
            ////ISelection<double> selection = new Elitist<double>();
            ////ICrossOver<double> crossOver = new OnePoint<double>(3);
            //ICrossOver<double> crossOver = new Uniform<double>(0.5);
            //IMutation<double> mutation = new ProbabilityMutate<double>(0.2);

            //EvolutionarySolver<double> solver = new EvolutionarySolver<double>(
            //    geneCount => new ChromosomeDouble(geneCount, HyperSphereMinimumVolume, -10, 10, 0.1),
            //    selection,
            //    crossOver,
            //    mutation);
            //ChromosomeBase<double> best = solver.Solve(6, 50, 50000, 0.00001);

            //ISelection<int> selection = new Tournament<int>(0.4);
            //ISelection<int> selection = new Bogo<int>();
            ISelection<int> selection = new Elitist<int>();
            ICrossOver<int> crossOver = new Uniform<int>(0.5);
            //ICrossOver<int> crossOver = new OnePoint<int>(1);
            IMutation<int> mutation = new ProbabilityMutate<int>(0.2);

            //EvolutionarySolver<int> solver = new EvolutionarySolver<int>(
            //    geneCount => new ChromosomeInt(geneCount, LinearEquations, 0, 20, 0.2),
            //    selection,
            //    crossOver,
            //    mutation);
            //ChromosomeBase<int> best = solver.Solve(3, 50, 50000, 0.00001);

            EvolutionarySolver<int> solver = new EvolutionarySolver<int>(
                geneCount => new ChromosomeInt(geneCount, DiophantineEquation, -300, 300, 0.01),
                selection,
                crossOver,
                mutation);
            ChromosomeBase<int> best = solver.Solve(2, 500, 50000, 0.00001);
        }

        // Minimum volume of hyper-sphere
        static double HyperSphereMinimumVolume(ChromosomeBase<double> chromosome)
        {
            // absolute error for hyper-sphere function
            const double trueMin = 0.0;
            double z = chromosome.Genes.Sum(gene => (gene*gene));
            return Math.Abs(trueMin - z);
        }

        static double DiophantineEquation(ChromosomeBase<int> chromosome)
        {
            // 1027*x+712y=1  -> -165, 238
            int[] genes = chromosome.Genes.ToArray();

            int lhs = 1027*genes[0] + 712*genes[1];
            int rhs = 1;

            return Math.Abs(lhs - rhs);
        }

        static double DiophantineEquation4(ChromosomeBase<int> chromosome)
        {
            // a+2b+3c+4d=30
            int[] genes = chromosome.Genes.ToArray();

            int lhs = genes[0] + 2 * genes[1] + 3 * genes[2] + 4 * genes[3];
            int rhs = 30;

            return Math.Abs(lhs - rhs);
        }

        static double DiophantineEquation12(ChromosomeBase<int> chromosome)
        {
            // x1²+x2²+x3²+x4²+x5²...+x12²=3842
            int lhs = chromosome.Genes.Aggregate((i, i1) => i + i1 * i1);
            //int lhs = chromosome.Genes.Select((n, i) => n*n*i).Sum();
            int rhs = 3842;

            return Math.Abs(lhs - rhs);
        }

        static double DiophantineEquationPell(ChromosomeBase<int> chromosome)
        {
            int[] genes = chromosome.Genes.ToArray();
            int lhs = Square(genes[0])-3*Square(genes[1]);
            int rhs = 1;

            return Math.Abs(lhs - rhs);
        }

        static double DiophantineEquationErdosStraus(ChromosomeBase<int> chromosome)
        {
            int[] genes = chromosome.Genes.ToArray();
            const int n = -3;
            int x = genes[0];
            int y = genes[1];
            int z = genes[2];
            int lhs = 4*x*y*z;
            int rhs = n*(x*y+x*z+y*z);

            return Math.Abs(lhs - rhs);
        }

        static double DiophantineEquationDifficult1(ChromosomeBase<int> chromosome)
        {
            int[] genes = chromosome.Genes.ToArray();
            int x = genes[0];
            int y = genes[1];
            int z = genes[2];
            long lhs = 3*(x*x+y*y+z*z);
            long rhs = 14*(x*y+x*z+y*z);

            return Math.Abs(lhs - rhs);
        }

        // Solve 3 linear equations
        static double LinearEquations(ChromosomeBase<int> chromosome)
        {
            //2*x1  +   4*x2    +8*x3   =44
            //4*x1  +   6*x2    +10*x3  =66
            //6*x1  +   8*x2    +10*x3  =84
            int[] genes = chromosome.Genes.ToArray();

            int lhs1 = 2 * genes[0] + 4*genes[1] + 8*genes[2];
            int rhs1 = 44;
            int lhs2 = 4 * genes[0] + 6 * genes[1] + 10*genes[2];
            int rhs2 = 66;
            int lhs3 = 6 * genes[0] + 8 * genes[1] + 10 * genes[2];
            int rhs3 = 84;

            return Math.Abs(lhs1-rhs1) + Math.Abs(lhs2-rhs2) + Math.Abs(lhs3-rhs3);

            ////http://en.wikipedia.org/wiki/Coefficient_of_determination   TODO
            //double mean = (lhs1 + lhs2 + lhs3) / 3.0;
            //double sstot = Square(lhs1 - mean) + Square(lhs2 - mean) + Square(lhs3 - mean);
            //double ssreg = Square(rhs1 - mean) + Square(rhs2 - mean) + Square(rhs3 - mean);
            //double ssres = Square(lhs1 - rhs1) + Square(lhs2 - rhs2) + Square(lhs3 - rhs3);

            ////double r2 = 1.0 - ssres / sstot;
            //double r2 = ssreg/sstot;
            //return r2;
        }

        static double Square(double v)
        {
            return v*v;
        }

        static int Square(int v)
        {
            return v * v;
        }

        public static long LongPower(int x, short power)
        {
            if (power == 0) return 1;
            if (power == 1) return x;
            if (x == 0) return 0;
            if (x == 1) return 1;
            // ----------------------
            int n = 15;
            while ((power <<= 1) >= 0) n--;

            long tmp = x;
            while (--n > 0)
                tmp = tmp * tmp *
                     (((power <<= 1) < 0) ? x : 1);
            return tmp;
        }
    }
}
