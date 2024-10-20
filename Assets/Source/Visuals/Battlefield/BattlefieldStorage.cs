using Source.Logic;
using Source.Logic.Data;
using Source.Logic.State;
using Source.Serialization;
using Source.Utility;
using UnityEngine;

namespace Source.Visuals.Battlefield
{
    public class BattlefieldStorage : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int itemStorageSize;
        
        [SerializeField] private GameStateLoader gameStateLoader;
        
        public ItemStorage<BattlefieldItem> ItemStorage => itemStorage;
        
        private ItemStorage<BattlefieldItem> itemStorage = new();

        public void Tick()
        {
            if(gameStateLoader.GameState != null)
                UpdateStorageFromState(gameStateLoader.GameState);
            
            itemStorage.Resize(itemStorageSize);
        }
        
        private void UpdateStorageFromState(GameState gameState)
        {
            itemStorageSize = gameState.BattlefieldStorage.Items.Count;


            for (var index = 0; index < gameState.BattlefieldStorage.Items.Count; index++)
            {
                var item = gameState.BattlefieldStorage.Items[index];
                //Debug.Log($"Battlefield Item: {battlefieldStorageItem.Location}, {battlefieldStorageItem.Unit?.Definition}, {battlefieldStorageItem.Building?.Definition}");
                itemStorage.GetItemSlotReference(index, out var itemSlot);
                itemSlot.Item = item;
            }
        }
    }
}