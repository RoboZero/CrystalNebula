using System;
using System.Collections.Generic;

namespace Source.Logic.State.LineItems
{
    [Serializable]
    public class LineStorage<T> where T : LineItem
    {
        public string StorageName;
        public int Length;
        public float DataPerSecondTransfer;
        public List<T> Items;

        public override string ToString()
        {
            return $"{StorageName}:(Length: {Length}, DpSTrans: {DataPerSecondTransfer})";
        }
    }
}