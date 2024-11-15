using System;

namespace Source.Serialization.Data
{
    [Serializable]
    public class MemoryItemData
    {
        public int Location;
        public MemoryData Memory;

        public override string ToString()
        {
            return $"MemoryItemData (Location: {Location}, Memory: {Memory})";
        }
    }
}