using System.Collections.Generic;

namespace Source.Utility
{
    public static class ListExtensions
    {
        public static bool TryRemoveLast<T>(this IList<T> list)
        {
            if (list.Count == 0) return false;
            list.RemoveAt(list.Count - 1);
            return true;
        }

        public static T Peek<T>(this IList<T> list)
        {
            return list[^1];
        }
    }
}
