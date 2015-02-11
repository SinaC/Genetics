using System;
using System.Collections.Generic;
using System.Linq;

namespace Genetics
{
    public static class Extensions
    {
        public static T Clamp<T>(this T val, T min, T max) 
            where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }

        // http://codereview.stackexchange.com/questions/61338/generate-random-numbers-without-repetitions
        // Note, max is exclusive here!
        public static List<int> GenerateRandom(this Random random, int count, int min, int max, bool shuffle = false)
        {
            //  initialize set S to empty
            //  for J := N-M + 1 to N do
            //    T := RandInt(1, J)
            //    if T is not in S then
            //      insert T in S
            //    else
            //      insert J in S
            //
            // adapted for C# which does not have an inclusive Next(..)
            // and to make it from configurable range not just 1.

            if (max <= min || count < 0 ||
                // max - min > 0 required to avoid overflow
                    (count > max - min && max - min > 0))
            {
                // need to use 64-bit to support big ranges (negative min, positive max)
                throw new ArgumentOutOfRangeException("Range " + min + " to " + max +" (" + (max - min) + " values), or count " + count + " is illegal");
            }

            // generate count random values.
            HashSet<int> candidates = new HashSet<int>();

            // start count values before max, and end at max
            for (int top = max - count; top < max; top++)
            {
                // May strike a duplicate.
                // Need to add +1 to make inclusive generator
                // +1 is safe even for MaxVal max value because top < max
                if (!candidates.Add(random.Next(min, top + 1)))
                {
                    // collision, add inclusive max.
                    // which could not possibly have been added before.
                    candidates.Add(top);
                }
            }

            if (shuffle)
            {
                // load them in to a list, to sort
                List<int> result = candidates.ToList();

                // shuffle the results because HashSet has messed
                // with the order, and the algorithm does not produce
                // random-ordered results (e.g. max-1 will never be the first value)
                for (int i = result.Count - 1; i > 0; i--)
                {
                    int k = random.Next(i + 1);
                    int tmp = result[k];
                    result[k] = result[i];
                    result[i] = tmp;
                }
                return result;
            }
            else
                return candidates.ToList();
        }

    }
}
