using Source.Logic.Events;
using UnityEngine;

namespace Source.Logic.State.LineItems.Units
{
    public class UnitMemory : MemoryItem
    {
        public int Health;
        public int Power;
        public bool CanSwitchPlaces = true;
        public bool CanEngageCombat = true;
        
        public UnitMemory(){}

        public override MemoryItem CreateInstance()
        {
            return new UnitMemory()
            {
                OwnerId = OwnerId,
                Definition = Definition,
                CurrentRunProgress = CurrentRunProgress,
                MaxRunProgress = MaxRunProgress,
                DataSize = DataSize,
                Health = Health,
                Power = Power,
                CanSwitchPlaces = CanSwitchPlaces,
                CanEngageCombat = CanEngageCombat,
            };
        }

        protected override void Run(EventTracker eventTracker, GameState gameState)
        {
            Debug.Log("Unit ran!");
        }
        
        public override string ToString()
        {
            return $"{Definition}: (OId: {OwnerId} H {Health}, P {Power})";
        }
    }
}