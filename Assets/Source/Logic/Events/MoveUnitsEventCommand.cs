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
                
                if (!toSlots.InBounds(index))
                {
                    logBuilder.AppendLine($"Failed to move unit from {fromSlot}: no associated to index.");
                    success = false;
                    continue;
                }
                
                var toSlot = toSlots[index];

                var moveUnitEvent = new MoveUnitEventCommand(eventTracker, battlefieldStorage, fromSlot, toSlot, canSwitchPlacesOverride, canEngageCombatOverride);
                var result = moveUnitEvent.Perform();
                
                if (result == false) 
                    success = false;
            }

            Debug.Log(logBuilder);
            return success;
        }
    }
}