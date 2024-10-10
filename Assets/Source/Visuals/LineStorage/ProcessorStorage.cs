using System.Collections;
using System.Collections.Generic;
using Source.Logic.Data;
using Source.Logic.State;
using Source.Visuals.LineStorage;
using UnityEngine;

public class ProcessorStorage : LineStorage
{
    protected override void UpdateStorageFromState(GameState gameState)
    {
        var diskStorageState = gameState.Players[playerId].ProcessorStorage;
        itemStorageSize = diskStorageState.Length;

        foreach (var item in diskStorageState.Items)
        {
            // Debug.Log($"Memory Item: {item.Location}, {item.Memory?.Definition}");
            itemStorage.GetItemSlotReference(item.Location, out var itemSlot);
            itemSlot.Item ??= new LineItemData();
            itemSlot.Item.Location = item.Location;
            itemSlot.Item.Memory = item.Memory;
        }
    }
}
