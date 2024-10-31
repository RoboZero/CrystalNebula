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

        public static string ToItemString<T>(this IList<T> list)
        {
            string items = "";
            for (var index = 0; index < list.Count; index++)
            {
                var item = list[index];
                items += item + (index == list.Count - 1 ? "" : ", ");
            }

            return items;
        }

        public static bool InBounds<T>(this IList<T> list, int index)
        {
            return index >= 0 && index < list.Count;
        }
    }
}
