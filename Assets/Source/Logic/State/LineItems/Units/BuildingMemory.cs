using System;

namespace Source.Logic.State.LineItems.Units
{
    public class BuildingMemory : MemoryItem
    {
        public int Health;
        public int Power;
        
        public override MemoryItem CreateInstance()
        { 
            return new BuildingMemory()
            {
                OwnerId = OwnerId,
                Definition = Definition,
                CurrentRunProgress = CurrentRunProgress,
                MaxRunProgress = MaxRunProgress,
                DataSize = DataSize,
                Health = Health,
                Power = Power,
            };
        }
    }
}