using Source.Logic;
using Source.Logic.Data;
using Source.Logic.State;
using Source.Serialization;
using UnityEngine;

namespace Source.Visuals.Battlefield
{
    public class BattlefieldStorage : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int itemStorageSize;
        
        [SerializeField] private GameStateLoader gameStateLoader;
        
        public ItemStorage<BattlefieldItemData> ItemStorage => itemStorage;
        
        private ItemStorage<BattlefieldItemData> itemStorage = new();

        public void Tick()
        {
            if(gameStateLoader.GameState != null)
                UpdateStorageFromState(gameStateLoader.GameState);
            
            itemStorage.Resize(itemStorageSize);
        }
        
        private void UpdateStorageFromState(GameState gameState)
        {
            itemStorageSize = gameState.BattlefieldStorage.Length;

            foreach (var battlefieldStorageItem in gameState.BattlefieldStorage.Items)
            {
                //Debug.Log($"Battlefield Item: {battlefieldStorageItem.Location}, {battlefieldStorageItem.Unit?.Definition}, {battlefieldStorageItem.Building?.Definition}");
                itemStorage.GetItemSlotReference(battlefieldStorageItem.Location, out var itemSlot);
                itemSlot.Item ??= new BattlefieldItemData();
                itemSlot.Item.Location = battlefieldStorageItem.Location;
                itemSlot.Item.Unit = battlefieldStorageItem.Unit;
                itemSlot.Item.Building = battlefieldStorageItem.Building;
            }
        }
    }
}