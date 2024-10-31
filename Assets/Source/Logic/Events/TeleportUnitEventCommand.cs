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
        
        private Unit fromUnit;
        private BattlefieldItem fromItem;
        
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
        
        public MoveUnitEventCommand(
            EventTracker eventTracker,
            BattlefieldStorage battlefieldStorage,
            Unit fromUnit,
            int toSlot,
            MoveUnitEventOverrides moveUnitEventOverrides
        )
        {
            this.eventTracker = eventTracker;
            this.battlefieldStorage = battlefieldStorage;
            this.fromUnit = fromUnit;
            this.toSlot = toSlot;
            this.moveUnitEventOverrides = moveUnitEventOverrides;
        }

        public override bool Perform()
        {
            AddLog($"Moving unit from {fromSlot} to {toSlot} of {battlefieldStorage}");

            if (fromUnit == null)
            {
                if (!TryGetUnitAtSlot(battlefieldStorage, fromSlot, out fromItem, out fromUnit))
                {
                    AddLog($"Failed to move unit from {fromSlot} to {toSlot}: unit at from slot does not exit");
                    return false;
                }
            }

            if (TryGetUnitAtSlot(battlefieldStorage, toSlot, out var toItem, out var otherUnit))
            {
                if (fromUnit.OwnerId == otherUnit.OwnerId)
                {
                    if (!fromUnit.CanSwitchPlaces &&
                        (moveUnitEventOverrides == null || !moveUnitEventOverrides.canSwitchPlacesOverride))
                    {
                        AddLog($"Failed to move unit from {fromSlot} to {toSlot}: friendly unit {otherUnit.Definition} on to spot and is not switching");
                        return false;
                    }
                    
                    PerformChildEventWithLog(new SwitchUnitEventCommand(
                        eventTracker,
                        battlefieldStorage,
                        fromSlot,
                        toSlot
                    ));
                }
                else
                {
                    if (!fromUnit.CanEngageCombat &&
                        (moveUnitEventOverrides == null || !moveUnitEventOverrides.canEngageCombatOverride))
                    {
                        AddLog($"Failed to move unit from to {toSlot}: enemy unit {otherUnit.Definition} on to slot and cannot engage combat");
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

            if (toItem == null)
            {
                AddLog($"Failed to move unit from {fromSlot} to {toSlot}: to slot does not exist");
                return false;
            }

            AddLog($"Successfully moved unit {fromUnit.Definition} from {fromSlot} to {toSlot}");
            toItem.Unit = fromUnit;
            
            if (fromItem != null)
                fromItem.Unit = null;
            
            return true;
        }
    }
}