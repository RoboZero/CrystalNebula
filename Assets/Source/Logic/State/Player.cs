using System;
using System.Collections.Generic;
using System.Linq;
using Source.Logic.State.LineItems;
using Source.Serialization.Data;

namespace Source.Logic.State
{
    [Serializable]
    public class Player
    {
        public int Id;
        public LineStorage<MemoryItem> PersonalStorage;
        public List<Processor> Processors;
        public LineStorage<MemoryItem> MemoryStorage;
        public LineStorage<MemoryItem> DiskStorage;
    }
}