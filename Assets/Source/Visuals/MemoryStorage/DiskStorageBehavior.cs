using System.Linq;
using Source.Logic.State;
using UnityEngine;

namespace Source.Visuals.MemoryStorage
{
    public class DiskStorageBehavior : LineStorageBehavior
    {
        protected override void UpdateStorageFromState(GameState gameState)
        {
            var player = gameState.Players.FirstOrDefault(player => player.Id == playerID);
            if (player == null)
            {
                Debug.LogWarning($"Failed to read from disk storage: playerId {playerID} is invalid, gamestate players count {gameState.Players.Count}");
                return;
            }
            
            var diskStorageState = player.DiskStorage;
            itemStorageSize = diskStorageState.Length;
            state = diskStorageState;
            level = gameState.Level;
        }
    }
}
