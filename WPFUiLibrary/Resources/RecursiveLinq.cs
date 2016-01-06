using System;
using System.Collections.Generic;

namespace WPFUiLibrary.Resources
{
    public static class RecursiveLinQ
    {

        public static IEnumerable<T> SelectRecursive<T>(this IEnumerable<T> enumerable, Func<T, IEnumerable<T>> access)
        {
            foreach (var value in enumerable)
            {
                yield return value;
                foreach (var subValue in access(value).SelectRecursive(access))
                {
                    yield return subValue;
                }
            }
        }

    }
}