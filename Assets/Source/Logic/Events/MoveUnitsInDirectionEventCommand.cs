using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Logic.State;
using Source.Logic.State.Battlefield;
using Source.Logic.State.LineItems;
using Source.Utility;

namespace Source.Logic.Events
{
    public class MoveUnitsInDirectionEventCommand : EventCommand
    {
        public enum Direction
        {
            Right = 1,
            Left = -1
        }
        
        private EventTracker eventTracker;
        private LineStorage<BattlefieldItem> battlefieldStorage;
        private List<int> fromSlots;
        private Direction direction;
        private int distance;
        private MoveUnitEventOverrides moveUnitEventOverrides;
        
        public MoveUnitsInDirectionEventCommand(
            EventTracker eventTracker,
            LineStorage<BattlefieldItem> battlefieldStorage,
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
        
        public override async UniTask<bool> Apply(CancellationToken cancellationToken)
        {
            AddLog($"Moving units from {fromSlots.ToItemString()} in direction {direction.ToString()} with distance of {distance} within {battlefieldStorage}");

            var success = true;
            var toSlots = new List<int>();

            foreach (var fromSlot in fromSlots)
            {
                var toSlot = fromSlot + ((int) direction * distance);
                
                toSlots.Add(toSlot);
            }

            switch (direction)
            {
                case Direction.Right:
                    for (var i = fromSlots.Count - 1; i >= 0; i--)
                    {
                        var fromSlot = fromSlots[i];
                        var toSlot = toSlots[i];
                        var result = await PerformChildEventWithLog(new TeleportUnitEventCommand(
                                eventTracker,
                                battlefieldStorage,
                                fromSlot,
                                toSlot,
                                moveUnitEventOverrides
                            ), cancellationToken);
                        
                        if (result == false)
                            success = false;
                    }

                    break;
                case Direction.Left:
                    for (var i = 0; i < fromSlots.Count; i++)
                    {
                        var fromSlot = fromSlots[i];
                        var toSlot = toSlots[i];
                        var result = await PerformChildEventWithLog(new TeleportUnitEventCommand(
                                eventTracker,
                                battlefieldStorage,
                                fromSlot,
                                toSlot,
                                moveUnitEventOverrides
                            ), cancellationToken);
                        
                        if (result == false)
                            success = false;
                    }
                    break;
            }

            return success;
        }
    }
}