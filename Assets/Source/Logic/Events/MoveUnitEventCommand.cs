using System.Collections.Generic;
using System.Text;
using Source.Logic.Data;
using Source.Utility;
using UnityEngine;

namespace Source.Logic.Events
{
    public class MoveUnitEventCommand : EventCommand
    {
        
        private EventTracker eventTracker;
        private BattlefieldStorage battlefieldStorage;
        private int fromSlot;
        private int toSlot;
        private bool canSwitchPlacesOverride;
        private bool canEngageCombatOverride;
        
        public MoveUnitEventCommand(
            EventTracker eventTracker,
            BattlefieldStorage battlefieldStorage,
            int fromSlot,
            int toSlot,
            bool canSwitchPlacesOverride,
            bool canEngageCombatOverride
        )
        {
            this.eventTracker = eventTracker;
            this.battlefieldStorage = battlefieldStorage;
            this.fromSlot = fromSlot;
            this.toSlot = toSlot;
            this.canSwitchPlacesOverride = canSwitchPlacesOverride;
            this.canEngageCombatOverride = canEngageCombatOverride;
        }
        
        public override bool Perform()
        {
            var logBuilder = new StringBuilder();
            logBuilder.AppendLine($"{ID} Moving unit from {fromSlot} to {toSlot} of {battlefieldStorage}");
            
            if (!battlefieldStorage.TryGetUnitAtSlot(fromSlot, logBuilder, out _, out var unit))
            {
                logBuilder.AppendLine($"Failed to move unit from {fromSlot} to {toSlot}: unit at from slot does not exit");
                Debug.Log(logBuilder);
                return false;
            }

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
            Debug.Log(logBuilder);
            return true;
        }
    }
}