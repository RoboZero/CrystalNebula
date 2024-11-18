using System.Linq;
using Source.Logic.Events;
using Source.Logic.Events.Overrides;
using UnityEngine;

namespace Source.Logic.State.LineItems.Programs
{
    public class BuildProgram : ProgramMemory
    {
        public MemoryItem MemoryItem;
        public CreateMemoryEventOverrides CreateMemoryEventOverrides;
        
        protected override void Run(EventTracker eventTracker, GameState gameState)
        {
            base.Run(eventTracker, gameState);
            Debug.Log($"{GetType().Name} ran.");

            var player = gameState.Players.FirstOrDefault(player => player.Id == MemoryItem.OwnerId);
            if (player == null)
            {
                Debug.LogWarning($"Unable to find player with Id == Memory item Owner Id {MemoryItem.OwnerId}");
                return;
            }

            var memoryStorage = player.MemoryStorage;

            if (memoryStorage.Length == 0)
            {
                Debug.Log($"{GetType().Name} unable to run as {memoryStorage} has length of 0.");
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