using System;
using System.Collections.Generic;
using System.Linq;
using Source.Logic.Events;
using Source.Logic.Events.Overrides;
using Source.Logic.State.LineItems.Units;
using UnityEngine;

namespace Source.Logic.State.LineItems.Programs
{
    public class BuildProgram : ProgramMemory
    {
        public MemoryItem MemoryItem;
        public CreateMemoryEventOverrides CreateMemoryEventOverrides;
        
        protected override void Run(EventTracker eventTracker, GameState gameState)
        {
            Debug.Log("Command ran!");

            var memoryStorage = gameState.Players[MemoryItem.OwnerId].MemoryStorage;

            if (memoryStorage.Length == 0)
            {
                return;
            }
            
            var slot = memoryStorage.Items.FindIndex(item => item == null);

            if (slot == -1 && CreateMemoryEventOverrides != null && CreateMemoryEventOverrides.Overwrite)
            {
                slot = memoryStorage.Items.Count - 1;
            }

            var createdItem = MemoryItem.CreateInstance();

            eventTracker.AddEvent(new CreateLineStorageMemoryEventCommand(
                eventTracker,
                memoryStorage,
                slot,
                createdItem,
                CreateMemoryEventOverrides
            ));
        }
    }
}