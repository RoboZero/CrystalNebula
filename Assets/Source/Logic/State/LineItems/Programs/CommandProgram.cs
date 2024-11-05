using System.Collections.Generic;
using Source.Logic.Events;
using UnityEngine;

namespace Source.Logic.State.LineItems.Programs
{
    public class CommandProgram : Memory
    {
        protected override void Run(EventTracker eventTracker, GameState gameState)
        {
            Debug.Log("Command ran!");
            
            var battlefield = gameState.BattlefieldStorage;

            var fromSlots = new List<int>();
            for (var index = 0; index < battlefield.Items.Count; index++)
            {
                var item = battlefield.Items[index];
                if (item != null && item.Unit != null)
                {
                    if (item.Unit.OwnerId == OwnerId)
                    {
                        fromSlots.Add(index);
                    }
                }
            }
                
            eventTracker.AddEvent(new MoveUnitsInDirectionEventCommand(
                eventTracker,
                battlefield,
                fromSlots,
                MoveUnitsInDirectionEventCommand.Direction.Right,
                1,
                null
            ));
        }
    }
}