using System;
using System.Collections.Generic;

namespace Source.Serialization.Data
{
    [Serializable]
    public class MemoryStorageData
    {
        public int Length;
        public List<MemoryItemData> Items;
    }
}