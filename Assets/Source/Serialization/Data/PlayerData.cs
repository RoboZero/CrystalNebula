using System;
using System.Collections.Generic;

namespace Source.Serialization.Data
{
    [Serializable]
    public class PlayerData
    {
        public int Id;
        public List<ProcessorData> Processors;
        public LineStorageData MemoryStorage;
        public LineStorageData DiskStorage;
    }
}