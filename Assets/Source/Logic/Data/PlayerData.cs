using System;

namespace Source.Logic.Data
{
    [Serializable]
    public class PlayerData
    {
        public int Id;
        public LineStorageData ProcessorStorage;
        public LineStorageData MemoryStorage;
        public LineStorageData DiskStorage;
    }
}