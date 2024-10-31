using System;
using Source.Logic;
using Source.Logic.Data;

namespace Source.Serialization.Data
{
    [Serializable]
    public class LineItemData : DataItem
    {
        public int Location;
        public MemoryData Memory;
    }
}