using Source.Logic;
using Source.Logic.Data;
using Source.Logic.State;
using Source.Serialization;
using UnityEngine;

namespace Source.Visuals.LineStorage
{
    public class MemoryStorage : LineStorage
    {
        protected override void UpdateStorageFromState(GameState gameState)
        {
            var ramStorageState = gameState.Players[playerId].MemoryStorage;
            itemStorageSize = ramStorageState.Length;

            foreach (var item in ramStorageState.Items)
            {
                // Debug.Log($"Memory Item: {item.Location}, {item.Memory?.Definition}");
                itemStorage.GetItemSlotReference(item.Location, out var itemSlot);
                itemSlot.Item ??= new LineItemData();
                itemSlot.Item.Location = item.Location;
                itemSlot.Item.Memory = item.Memory;
            }
        }
    }
}
