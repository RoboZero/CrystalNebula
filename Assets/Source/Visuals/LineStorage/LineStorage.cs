using Source.Logic;
using Source.Logic.Data;
using Source.Logic.State;
using Source.Serialization;
using UnityEngine;

namespace Source.Visuals.LineStorage
{
    public abstract class LineStorage : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] protected int playerId = 0;
        [SerializeField] protected int itemStorageSize;
    
        [SerializeField] protected GameStateLoader gameStateLoader;
    
        public ItemStorage<LineItem> ItemStorage => itemStorage;
        protected readonly ItemStorage<LineItem> itemStorage = new();
    
        public void Tick()
        {
            if(gameStateLoader.GameState != null)
                UpdateStorageFromState(gameStateLoader.GameState);

            itemStorage.Resize(itemStorageSize);
        }

        protected abstract void UpdateStorageFromState(GameState gameState);
    }
}
