using System;
using System.Collections.Generic;

namespace Source.Serialization.Data
{
    [Serializable]
    public class PlayerData
    {
        public int Id;
        public MemoryStorageData PersonalStorage;
        public List<ProcessorData> Processors;
        public MemoryStorageData MemoryStorage;
        public MemoryStorageData DiskStorage;
        public ResearchGraphData ResearchGraph;
    }
}