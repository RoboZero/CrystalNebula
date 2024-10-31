using System.Collections.Generic;
using System.Text;
using Source.Logic.Data;
using Source.Utility;
using UnityEngine;

namespace Source.Logic.Events
{
    public class MoveUnitsEventCommand : EventCommand
    {
        private EventTracker eventTracker;
        private BattlefieldStorage battlefieldStorage;
        private List<int> fromSlots;
        private List<int> toSlots;
        private MoveUnitEventOverrides moveUnitEventOverrides;
        
        public MoveUnitsEventCommand(
            EventTracker eventTracker,
            BattlefieldStorage battlefieldStorage,
            List<int> fromSlots,
            List<int> toSlots,
            MoveUnitEventOverrides moveUnitEventOverrides
        )
        {
            this.eventTracker = eventTracker;
            this.battlefieldStorage = battlefieldStorage;
            this.fromSlots = fromSlots;
            this.toSlots = toSlots;
            this.moveUnitEventOverrides = moveUnitEventOverrides;
        }
        
        public override bool Perform()
        {
            AddLog($"Moving units from {fromSlots.ToItemString()} to {toSlots.ToItemString()} of {battlefieldStorage}");

            var success = true;

            for (var index = 0; index < fromSlots.Count; index++)
            {
                var fromSlot = fromSlots[index];
                
                if (!toSlots.InBounds(index))
                {
                    AddLog($"Failed to move unit from {fromSlot}: no associated to index.");
                    success = false;
                    continue;
                }
                
                var toSlot = toSlots[index];
                
                var result = PerformChildEventWithLog(new MoveUnitEventCommand(
                    eventTracker, 
                    battlefieldStorage, 
                    fromSlot, 
                    toSlot, 
                    moveUnitEventOverrides)
                );
                
                if (result == false) 
                    success = false;
            }
            
            return success;
        }
    }
}