using System;

namespace Source.Logic.Data
{
    [Serializable]
    public class PlayerData
    {
        public int Id;
        public MemoryStorageData ProcessorStorage;
        public MemoryStorageData MemoryStorage;
        public MemoryStorageData DiskStorage;
    }
}