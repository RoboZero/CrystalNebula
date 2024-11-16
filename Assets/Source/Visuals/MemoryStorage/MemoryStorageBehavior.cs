using System.Linq;
using Source.Logic.State;
using UnityEngine;

namespace Source.Visuals.MemoryStorage
{
    public class MemoryStorageBehavior : LineStorageBehavior
    {
        protected override void UpdateStorageFromState(GameState gameState)
        {
            var player = gameState.Players.FirstOrDefault(player => player.Id == playerId);
            if (player == null)
            {
                Debug.LogWarning($"Failed to read from memory storage: playerId {playerId} is invalid, gamestate players count {gameState.Players.Count}");
                return;
            }
            
            var memoryStorageState = player.MemoryStorage;
            itemStorageSize = memoryStorageState.Length;
            state = memoryStorageState;
            level = gameState.Level;
        }
    }
}
