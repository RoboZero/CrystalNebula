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
        private bool canSwitchPlacesOverride;
        private bool canEngageCombatOverride;
        
        public MoveUnitsEventCommand(
            EventTracker eventTracker,
            BattlefieldStorage battlefieldStorage,
            List<int> fromSlots,
            List<int> toSlots,
            bool canSwitchPlacesOverride,
            bool canEngageCombatOverride
        )
        {
            this.eventTracker = eventTracker;
            this.battlefieldStorage = battlefieldStorage;
            this.fromSlots = fromSlots;
            this.toSlots = toSlots;
            this.canSwitchPlacesOverride = canSwitchPlacesOverride;
            this.canEngageCombatOverride = canEngageCombatOverride;
        }
        
        public override bool Perform()
        {
            var logBuilder = new StringBuilder();
            logBuilder.AppendLine($"{ID} Moving units from {fromSlots.ToItemString()} to {toSlots.ToItemString()} of {battlefieldStorage}");

            var success = true;

            for (var index = 0; index < fromSlots.Count; index++)
            {
                var fromSlot = fromSlots[index];
                if (!battlefieldStorage.TryGetUnitAtSlot(fromSlot, logBuilder, out _, out var unit))
                {
                    continue;
                }
                
                if (!toSlots.InBounds(index))
                {
                    logBuilder.AppendLine($"Failed to move unit from {fromSlot}: no associated to index.");
                    success = false;
                    continue;
                }
                
                var toSlot = toSlots[index];
                if (battlefieldStorage.TryGetUnitAtSlot(toSlot, logBuilder, out _, out var otherUnit))
                {
                    // TODO: Allow units to determine if able to switch
                    if (unit.OwnerId == otherUnit.OwnerId)
                    {
                        if (!canSwitchPlacesOverride)
                        {
                            logBuilder.AppendLine($"Failed to move unit from {fromSlot} to {toSlot}: friendly unit {otherUnit.Definition} on to spot and is not switching");
                            Debug.Log(logBuilder);
                            return false;
                        }
                    }
                    else
                    {
                        if (!canEngageCombatOverride)
                        {
                            logBuilder.AppendLine($"Failed to move unit from {fromSlot} to {toSlot}: enemy unit {otherUnit.Definition} on to slot and cannot engage combat");
                            Debug.Log(logBuilder);
                            return false;
                        }
                    
                        eventTracker.AddEvent(new UnitCombatEventCommand(
                            eventTracker,
                            battlefieldStorage, 
                            fromSlot, 
                            toSlot, 
                            true
                        ));

                        return true;
                    }
                }

                logBuilder.AppendLine($"Successfully moved unit {unit.Definition} from {fromSlot} to {toSlot}");
                battlefieldStorage.Items[toSlot] = battlefieldStorage.Items[fromSlot];
                battlefieldStorage.Items[fromSlot] = null;
            }

            Debug.Log(logBuilder);
            return success;
        }
    }
}