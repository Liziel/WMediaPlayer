using System;
using System.Collections.Generic;
using System.Threading;

namespace PlaylistPlugin.Ressources
{
    public static class ThreadSafeRandom
    {
        [ThreadStatic] private static Random local;
        
            public static Random Random
            => local ?? (local = new Random(unchecked (Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId)));
    }

    static class ListShuffle
    {

        public static List<T> Shuffle<T>(this IList<T> list)
        {
            var l = new List<T>(list.Count);
            var n = list.Count;

            l.AddRange(list);

            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.Random.Next(n + 1);
                var value = l[k];
                l[k] = list[n];
                l[n] = value;
            }
            return l;
        }
    }
}