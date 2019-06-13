using System;
using System.Collections.Generic;
using System.Linq;

namespace Martelskiy.Api.Template.Features.Shared.Linq
{
    public static class EnumerableExtensions
    {
        public static int IndexOf<T>(this IEnumerable<T> source, T item)
        {
            return source.IndexOf(item, EqualityComparer<T>.Default);
        }

        public static int IndexOf<T>(this IEnumerable<T> source, T item, IEqualityComparer<T> comparer)
        {
            return source.Select((x, index) =>
            {
                if (!comparer.Equals(item, x))
                    return -1;
                return index;
            }).FirstOr(x => x != -1, -1);
        }

        public static T FirstOr<T>(this IEnumerable<T> source, Func<T, bool> predicate, T alternate)
        {
            return source.Where(predicate).FirstOr(alternate);
        }

        public static T FirstOr<T>(this IEnumerable<T> source, T alternate)
        {
            return source.DefaultIfEmpty<T>(alternate).First<T>();
        }
    }
}
