using System;

namespace Genetics
{
    public static class Singleton
    {
        #region Random

        private static readonly Lazy<Random> Lazy =
            new Lazy<Random>(() => new Random());

        public static Random Random
        {
            get { return Lazy.Value; }
        }

        #endregion
    }
}
