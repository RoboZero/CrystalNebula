using System;
using System.Collections.Generic;

namespace Source.Serialization.Data
{
    [Serializable]
    public class MemoryStorageData
    {
        public int Length;
        public float DataPerSecondTransfer;
        public List<MemoryItemData> Items;
    }
}