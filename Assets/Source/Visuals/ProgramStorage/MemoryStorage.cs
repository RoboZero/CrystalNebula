using Source.Logic;
using Source.Logic.Data;
using Source.Logic.State;
using Source.Serialization;
using UnityEngine;

namespace Source.Visuals.ProgramStorage
{
    public class MemoryStorage : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int playerId = 0;
        [SerializeField] private int itemStorageSize;

        [SerializeField] private GameStateLoader gameStateLoader;

        public ItemStorage<MemoryItemData> ItemStorage => itemStorage;
        private ItemStorage<MemoryItemData> itemStorage = new();

        public void Tick()
        {
            if(gameStateLoader.GameState != null)
                UpdateStorageFromState(gameStateLoader.GameState);

            itemStorage.Resize(itemStorageSize);
        }
        
        private void UpdateStorageFromState(GameState gameState)
        {
            var memoryStorageState = gameState.Players[playerId].MemoryStorage;
            itemStorageSize = memoryStorageState.Length;

            foreach (var item in memoryStorageState.Items)
            {
                // Debug.Log($"Memory Item: {item.Location}, {item.Memory?.Definition}");
                itemStorage.GetItemSlotReference(item.Location, out var itemSlot);
                itemSlot.Item ??= new MemoryItemData();
                itemSlot.Item.Location = item.Location;
                itemSlot.Item.Memory = item.Memory;
            }
        }
    }
}
