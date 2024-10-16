using Source.Logic.Data;
using Source.Logic.State;
using UnityEngine;
using UnityEngine.Assertions;

namespace Source.Visuals.LineStorage
{
    public class ProcessorStorage : LineStorage
    {
        [Header("Dependencies")]
        [SerializeField] private int processorIndex = 0;
        
        protected override void UpdateStorageFromState(GameState gameState)
        {
            var player = gameState.Players[playerId];

            if (processorIndex < player.Processors.Count) return;

            var processor = player.Processors[processorIndex];
            itemStorageSize = processor.ProcessorStorage.Length;

            foreach (var item in processor.ProcessorStorage.Items)
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
