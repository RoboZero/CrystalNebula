using System;
using Source.Logic;
using Source.Logic.State;

namespace Source.Serialization.Data
{
    [Serializable]
    public class LineItemData : DataItem
    {
        public int Location;
        public MemoryData Memory;
    }
}