using System;
using System.Collections.Generic;
using System.Linq;
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

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
        {
            var e = enumerable.ToArray();
            var indexes = new List<int>(e.Length);

            for (var i = 0; i < e.Length; i++)
                indexes.Add(i);
            for (var i = 0; i < e.Length; i++)
            {
                int index = indexes[ThreadSafeRandom.Random.Next(indexes.Count)];
                indexes.Remove(index);
                yield return e[index];
            }
        }

    }
}