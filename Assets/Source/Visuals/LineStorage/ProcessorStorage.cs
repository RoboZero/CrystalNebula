using Source.Logic;
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

            for (var index = 0; index < processor.ProcessorStorage.Items.Count; index++)
            {
                var item = processor.ProcessorStorage.Items[index];
                // Debug.Log($"Memory Item: {item.Location}, {item.Memory?.Definition}");
                itemStorage.GetItemSlotReference(index, out var itemSlot);
                itemSlot.Item = item;
            }
        }
    }
}
