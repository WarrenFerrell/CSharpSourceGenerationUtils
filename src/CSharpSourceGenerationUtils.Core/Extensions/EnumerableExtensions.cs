using System;
using System.Collections.Generic;

namespace CSharpSourceGenerationUtils.Extensions
{
    internal static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var t in source)
            {
                action(t);
            }
        }
    }
}
