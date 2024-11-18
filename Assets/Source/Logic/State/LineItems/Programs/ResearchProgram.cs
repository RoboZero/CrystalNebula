using System;
using System.Collections.Generic;
using System.Linq;
using Source.Logic.Events;
using Source.Logic.Events.Overrides;
using Source.Serialization;
using Source.Visuals.MemoryStorage;
using UnityEngine;

namespace Source.Logic.State.LineItems.Programs
{
    public class ResearchProgram : ProgramMemory
    {
        //TODO: Extract game resources, not part of logic
        public GameResources GameResources;
        //public List<MemoryItem> CreatedMemoryItems;
        
        public CreateMemoryEventOverrides CreateMemoryEventOverrides;

        protected override void Run(EventTracker eventTracker, GameState gameState)
        {
            Debug.Log($"{GetType().Name} ran.");

            var player = gameState.Players.FirstOrDefault(player => player.Id == OwnerId);
            if (player == null)
            {
                Debug.LogWarning($"Unable to find player with Id == Owner Id {OwnerId}");
                return;
            }

            var diskStorage = player.DiskStorage;
            if (diskStorage.Length == 0)
            {
                Debug.Log($"{GetType().Name} unable to run as {diskStorage} has length of 0.");
                return;
            }

            var researchGraph = player.ResearchGraph;
            if (researchGraph.Edges.Count == 0)
            {
                Debug.Log($"{GetType().Name} unable to run as {researchGraph} has no entries");
                return;
            }

            if (researchGraph.Edges.TryGetValue(Definition, out var results))
            {
                foreach (var result in results)
                {
                    var slot = diskStorage.Items.FindIndex(item => item == null);

                    if (slot == -1 && CreateMemoryEventOverrides != null && CreateMemoryEventOverrides.Overwrite)
                    {
                        slot = diskStorage.Items.Count - 1;
                    }
                    
                    if(GameResources.TryLoadAsset(this, result.Definition, out MemoryDataSO memoryDataSO))
                    {
                        var createdItem = memoryDataSO.CreateDefaultInstance(OwnerId, result.Definition);
                        
                        eventTracker.AddEvent(new CreateLineStorageMemoryEventCommand(
                            eventTracker,
                            diskStorage,
                            slot,
                            createdItem,
                            CreateMemoryEventOverrides
                        ));
                    }
                }
            }
        }
    }
}