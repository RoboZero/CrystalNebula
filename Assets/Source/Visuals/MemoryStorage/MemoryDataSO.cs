using Source.Logic.State.LineItems;
using Source.Serialization.Data;
using Source.Utility;
using UnityEngine;

namespace Source.Visuals.MemoryStorage
{
    public abstract class MemoryDataSO : DescriptionBaseSO
    {
        public abstract Sprite MemoryBackgroundIcon { get; }
        public abstract Sprite MemoryForegroundIcon { get; }
        public abstract string MemoryName { get; }
        
        public abstract MemoryItem CreateMemoryInstance(MemoryData memoryData);
    }
}