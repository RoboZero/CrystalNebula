using Source.Logic;
using Source.Logic.Data;
using Source.Logic.State;
using Source.Serialization;
using Source.Utility;
using UnityEngine;

namespace Source.Visuals.LineStorage
{
    public class MemoryStorage : LineStorage
    {
        protected override void UpdateStorageFromState(GameState gameState)
        {
            var memoryStorageState = gameState.Players[playerId].MemoryStorage;
            itemStorageSize = memoryStorageState.Length;


            for (var index = 0; index < memoryStorageState.Items.Count; index++)
            {
                var item = memoryStorageState.Items[index];
                // Debug.Log($"Memory Item: {item.Location}, {item.Memory?.Definition}");
                itemStorage.GetItemSlotReference(index, out var itemSlot);
                itemSlot.Item = item;
            }
        }
    }
}
