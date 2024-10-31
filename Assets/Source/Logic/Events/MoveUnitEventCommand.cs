using System.Collections.Generic;
using System.Text;
using Source.Logic.Data;

namespace Source.Logic.Events
{
    public class MoveUnitEventCommand : EventCommand
    {
        
        private EventTracker eventTracker;
        private BattlefieldStorage battlefieldStorage;
        private int fromSlot;
        private int toSlot;
        private MoveUnitEventOverrides moveUnitEventOverrides;
        
        public MoveUnitEventCommand(
            EventTracker eventTracker,
            BattlefieldStorage battlefieldStorage,
            int fromSlot,
            int toSlot,
            MoveUnitEventOverrides moveUnitEventOverrides
        )
        {
            this.eventTracker = eventTracker;
            this.battlefieldStorage = battlefieldStorage;
            this.fromSlot = fromSlot;
            this.toSlot = toSlot;
            this.moveUnitEventOverrides = moveUnitEventOverrides;
        }
        
        public override bool Perform()
        {
            AddLog($"Moving unit from {fromSlot} to {toSlot} of {battlefieldStorage}");
            
            if (!TryGetUnitAtSlot(battlefieldStorage, fromSlot, out _, out var unit))
            {
                AddLog($"Failed to move unit from {fromSlot} to {toSlot}: unit at from slot does not exit");
                return false;
            }

            if (TryGetUnitAtSlot(battlefieldStorage, toSlot, out _, out var otherUnit))
            {
                // TODO: Allow units to determine if able to switch
                if (unit.OwnerId == otherUnit.OwnerId)
                {
                    if (moveUnitEventOverrides != null && !moveUnitEventOverrides.canSwitchPlacesOverride)
                    {
                        AddLog($"Failed to move unit from {fromSlot} to {toSlot}: friendly unit {otherUnit.Definition} on to spot and is not switching");
                        return false;
                    }
                }
                else
                {
                    if (moveUnitEventOverrides != null && !moveUnitEventOverrides.canEngageCombatOverride)
                    {
                        AddLog($"Failed to move unit from {fromSlot} to {toSlot}: enemy unit {otherUnit.Definition} on to slot and cannot engage combat");
                        return false;
                    }

                    PerformChildEventWithLog(new UnitCombatEventCommand(
                        eventTracker,
                        battlefieldStorage,
                        fromSlot,
                        toSlot,
                        true
                    ));

                    return true;
                }
            }

            AddLog($"Successfully moved unit {unit.Definition} from {fromSlot} to {toSlot}");
            battlefieldStorage.Items[toSlot] ??= new BattlefieldItem();
            battlefieldStorage.Items[toSlot].Unit = battlefieldStorage.Items[fromSlot].Unit;
            battlefieldStorage.Items[fromSlot].Unit = null;
            return true;
        }
    }
}