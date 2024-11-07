using Source.Logic.State;
using Source.Logic.State.Battlefield;
using Source.Logic.State.LineItems;
using UnityEngine;

namespace Source.Logic.Events
{
    public class TeleportUnitEventCommand : EventCommand
    {
        private EventTracker eventTracker;
        private LineStorage<BattlefieldItem> battlefieldStorage;
        private int fromSlot;
        private int toSlot;
        private MoveUnitEventOverrides moveUnitEventOverrides;
        
        private Unit fromUnit;
        private BattlefieldItem fromItem;
        
        public TeleportUnitEventCommand(
            EventTracker eventTracker,
            LineStorage<BattlefieldItem> battlefieldStorage,
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
        
        public TeleportUnitEventCommand(
            EventTracker eventTracker,
            LineStorage<BattlefieldItem> battlefieldStorage,
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
            AddLog($"{nameof(TeleportUnitEventCommand)}: Moving unit from {fromSlot} to {toSlot} of {battlefieldStorage}");
            var failPrefix = $"Failed to move unit from {fromSlot} to {toSlot}: ";

            if (fromUnit == null)
            {
                if (!TryGetUnitAtSlot(battlefieldStorage, fromSlot, out fromItem, out fromUnit))
                {
                    AddLog(failPrefix + "unit at from slot does not exit");
                    return false;
                }
            }
            
            failPrefix = $"Failed to move unit {fromUnit} from {fromSlot} to {toSlot}: ";

            if (TryGetUnitAtSlot(battlefieldStorage, toSlot, out var toItem, out var otherUnit) && otherUnit != null)
            {
                AddLog($"To slot {toSlot}, To item: {toItem}, Other unit {otherUnit}, battlefield item {battlefieldStorage.Items[toSlot]}");
                if (fromUnit.OwnerId == otherUnit.OwnerId)
                {
                    if (!fromUnit.CanSwitchPlaces &&
                        (moveUnitEventOverrides == null || !moveUnitEventOverrides.CanSwitchPlacesOverride))
                    {
                        AddLog(failPrefix + $"friendly unit {otherUnit} on to spot and is not switching");
                        return false;
                    }
                    
                    return PerformChildEventWithLog(new SwitchUnitEventCommand(
                        eventTracker,
                        battlefieldStorage,
                        fromSlot,
                        toSlot
                    ));
                }
                else
                {
                    if (!fromUnit.CanEngageCombat &&
                        (moveUnitEventOverrides == null || !moveUnitEventOverrides.CanEngageCombatOverride))
                    {
                        AddLog(failPrefix + $"enemy unit {otherUnit} on to slot and cannot engage combat");
                        return false;
                    }

                    return PerformChildEventWithLog(new UnitCombatEventCommand(
                        eventTracker,
                        battlefieldStorage,
                        fromSlot,
                        toSlot,
                        true
                    ));
                }
            }

            if (toItem == null)
            {
                AddLog(failPrefix + $" to slot does not exist");
                return false;
            }

            AddLog($"Successfully moved unit {fromUnit} from {fromSlot} to {toSlot}");
            toItem.Unit = fromUnit;
            
            if (fromItem != null)
                fromItem.Unit = null;
            
            return true;
        }
    }
}