using System;
using System.Collections.Generic;

namespace Source.Logic.State
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