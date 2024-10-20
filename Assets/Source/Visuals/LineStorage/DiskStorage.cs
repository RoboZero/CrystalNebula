using Source.Logic;
using Source.Logic.Data;
using Source.Logic.State;
using Source.Serialization;
using UnityEngine;

namespace Source.Visuals.LineStorage
{
    public class DiskStorage : LineStorage
    {
        protected override void UpdateStorageFromState(GameState gameState)
        {
            var diskStorageState = gameState.Players[playerId].DiskStorage;
            itemStorageSize = diskStorageState.Length;

            for (var index = 0; index < diskStorageState.Items.Count; index++)
            {
                var item = diskStorageState.Items[index];
                // Debug.Log($"Memory Item: {item.Location}, {item.Memory?.Definition}");
                itemStorage.GetItemSlotReference(index, out var itemSlot);
                itemSlot.Item = item;
            }
        }
    }
}
