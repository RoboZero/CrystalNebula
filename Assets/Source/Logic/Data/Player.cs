using System;
using System.Collections.Generic;

namespace Source.Logic.Data
{
    [Serializable]
    public class Player
    {
        public int Id;
        public List<Processor> Processors;
        public LineStorage MemoryStorage;
        public LineStorage DiskStorage;
    }
}