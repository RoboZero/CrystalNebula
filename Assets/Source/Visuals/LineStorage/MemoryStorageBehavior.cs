using Source.Logic;
using Source.Logic.Data;
using Source.Logic.State;
using Source.Serialization;
using Source.Utility;
using UnityEngine;

namespace Source.Visuals.LineStorage
{
    public class MemoryStorageBehavior : LineStorageBehavior
    {
        protected override void UpdateStorageFromState(GameState gameState)
        {
            var memoryStorageState = gameState.Players[playerId].MemoryStorage;
            itemStorageSize = memoryStorageState.Length;
            state = memoryStorageState;
        }
    }
}
