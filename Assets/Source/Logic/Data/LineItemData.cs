using System;

namespace Source.Logic.Data
{
    [Serializable]
    public class LineItemData : DataItem
    {
        public int Location;
        public MemoryData Memory;
    }
}