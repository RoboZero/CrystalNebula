using System;
using Source.Logic.State;

namespace Source.Logic.Data
{
    [Serializable]
    public class MemoryStorageData
    {
        public int Length;
        public MemoryItemData[] Items;
    }
}