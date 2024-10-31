using System;
using Source.Logic;

namespace Source.Serialization.Data
{
    [Serializable]
    public class LineItemData : DataItem
    {
        public int Location;
        public MemoryData Memory;
    }
}