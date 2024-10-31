using System;
using System.Collections.Generic;
using System.Text;
using Source.Logic.Data;
using Source.Utility;
using UnityEngine;

namespace Source.Logic.Events
{
    public class MoveUnitsInDirectionEventCommand : EventCommand
    {
        public enum Direction
        {
            Up = 1,
            Down = -1
        }
        
        private EventTracker eventTracker;
        private BattlefieldStorage battlefieldStorage;
        private List<int> fromSlots;
        private Direction direction;
        private int distance;
        private MoveUnitEventOverrides moveUnitEventOverrides;
        
        public MoveUnitsInDirectionEventCommand(
            EventTracker eventTracker,
            BattlefieldStorage battlefieldStorage,
            List<int> fromSlots,
            Direction direction,
            int distance,
            MoveUnitEventOverrides moveUnitEventOverrides
        )
        {
            this.eventTracker = eventTracker;
            this.battlefieldStorage = battlefieldStorage;
            this.fromSlots = fromSlots;
            this.direction = direction;
            this.distance = distance;
            this.moveUnitEventOverrides = moveUnitEventOverrides;
        }
        
        public override bool Perform()
        {
            AddLog($"Moving units from {fromSlots.ToItemString()} in direction {direction.ToString()} with distance of {distance} within {battlefieldStorage}");

            var success = true;
            var toSlots = new List<int>();

            foreach (var fromSlot in fromSlots)
            {
                var toSlot = fromSlot + (int) direction;
                
                toSlots.Add(toSlot);
            }

            var result = PerformChildEventWithLog(new MoveUnitsEventCommand(
                    eventTracker,
                    battlefieldStorage,
                    fromSlots,
                    toSlots,
                    moveUnitEventOverrides
                )
            );

            if (result == false)
                success = false;
            
            return success;
        }
    }
}