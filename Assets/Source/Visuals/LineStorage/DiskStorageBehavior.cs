using Source.Logic;
using Source.Logic.State;
using Source.Serialization;
using UnityEngine;

namespace Source.Visuals.LineStorage
{
    public class DiskStorageBehavior : LineStorageBehavior
    {
        protected override void UpdateStorageFromState(GameState gameState)
        {
            if (!gameState.Players.ContainsKey(playerID))
            {
                Debug.LogWarning($"Failed to read from disk storage: playerId {playerID} is invalid, gamestate players count {gameState.Players.Count}");
                return;
            }
            
            var diskStorageState = gameState.Players[playerID].DiskStorage;
            itemStorageSize = diskStorageState.Length;
            state = diskStorageState;
        }
    }
}
