using System;
using System.Collections.Generic;

namespace Source.Logic.State.LineItems
{
    [Serializable]
    public class LineStorage
    {
        public string StorageName;
        public int Length;
        public List<LineItem> Items;

        public override string ToString()
        {
            return $"{StorageName}:(Length: {Length})";
        }
    }
}