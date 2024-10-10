using System;

namespace Source.Logic.Data
{
    [Serializable]
    public class MemoryItemData : DataItem
    {
        public int Location;
        public MemoryData Memory;
    }
}