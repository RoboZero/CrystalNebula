using Source.Logic;
using Source.Logic.Data;
using Source.Logic.State;
using Source.Serialization;
using UnityEngine;

namespace Source.Visuals.LineStorage
{
    public class DiskStorageBehavior : LineStorageBehavior
    {
        protected override void UpdateStorageFromState(GameState gameState)
        {
            var diskStorageState = gameState.Players[playerId].DiskStorage;
            itemStorageSize = diskStorageState.Length;
            state = diskStorageState;
        }
    }
}
