using Source.Logic;
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
            if (!gameState.Players.ContainsKey(playerID))
            {
                Debug.LogWarning($"Failed to read from memory storage: playerId {playerID} is invalid, gamestate players count {gameState.Players.Count}");
                return;
            }
            
            var memoryStorageState = gameState.Players[playerID].MemoryStorage;
            itemStorageSize = memoryStorageState.Length;
            state = memoryStorageState;
        }
    }
}
