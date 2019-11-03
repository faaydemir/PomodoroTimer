using System;
using System.Collections.Generic;
using System.Text;

namespace HelperLib.Extentions
{
    public static class RandomExtentions
    {
        public static T OneOf<T>(this Random rng, IList<T> things)
        {
            return things[rng.Next(things.Count)];
        }
        public static T OneOf<T>(this Random rng, params T[] things)
        {
            return things[rng.Next(things.Length)];
        }
    }
}
