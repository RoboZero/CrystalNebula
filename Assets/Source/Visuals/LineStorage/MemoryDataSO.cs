using Source.Logic.State;
using Source.Logic.State.LineItems;
using Source.Serialization.Data;
using Source.Utility;
using UnityEngine;

namespace Source.Visuals.LineStorage
{
    public abstract class MemoryDataSO : DescriptionBaseSO
    {
        public abstract Sprite MemoryBackgroundIcon { get; }
        public abstract Sprite MemoryForegroundIcon { get; }
        public abstract string MemoryName { get; }
        
        public abstract Memory CreateMemoryInstance(MemoryData memoryData);
    }
}