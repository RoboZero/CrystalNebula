using System.Collections.Generic;
using System.Linq;
using System.Text;
using Source.Logic.State;
using Source.Logic.State.Battlefield;
using Source.Logic.State.LineItems;
using Source.Logic.State.LineItems.Units;
using Source.Utility;
using UnityEngine;

namespace Source.Logic.Events
{
    public class TeleportUnitsEventCommand : EventCommand
    {
        private EventTracker eventTracker;
        private LineStorage<BattlefieldItem> battlefieldStorage;
        private List<int> fromSlots;
        private List<int> toSlots;
        private MoveUnitEventOverrides moveUnitEventOverrides;

        private List<FromData> fromData = new();

        public TeleportUnitsEventCommand(
            EventTracker eventTracker,
            LineStorage<BattlefieldItem> battlefieldStorage,
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

            if (fromSlots.Count != toSlots.Count)
            {
                AddLog($"Failed to move units: from count {fromSlots.Count} != to count {toSlots.Count}");
                return false;
            }

            for (var index = 0; index < fromSlots.Count; index++)
            {
                var fromSlot = fromSlots[index];

                if (!TryGetUnitAtSlot(battlefieldStorage, fromSlot, out var fromItem, out var fromUnit))
                {
                    AddLog($"Failed to move unit in {fromSlot}: unit does not exist (null)");
                    success = false;
                    continue;
                }
                
                fromData.Add(new FromData
                {
                    FromSlotsIndex = index,
                    Item = fromItem,
                    Unit = fromUnit
                });
            }

            foreach (var from in fromData)
            {
                var toSlot = toSlots[from.FromSlotsIndex];

                var result = PerformChildEventWithLog(new TeleportUnitEventCommand(
                    eventTracker, 
                    battlefieldStorage, 
                    from.Unit, 
                    toSlot, 
                    moveUnitEventOverrides
                ));

                if (result) 
                    success = false;
            }
            
            return success;
        }

        private struct FromData
        {
            public int FromSlotsIndex;
            public BattlefieldItem Item;
            public UnitMemory Unit;
        }
    }
}