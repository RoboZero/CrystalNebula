using Source.Logic.State;
using Source.Logic.State.Battlefield;
using Source.Logic.State.LineItems;
using Source.Serialization;
using UnityEngine;

namespace Source.Visuals.BattlefieldStorage
{
    public class BattlefieldStorageBehavior : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int itemStorageSize;
        
        [SerializeField] private GameStateLoader gameStateLoader;

        public LineStorage<BattlefieldItem> State => state;
 
        private LineStorage<BattlefieldItem> state;

        public void Tick()
        {
            if(gameStateLoader.GameState != null)
                UpdateStorageFromState(gameStateLoader.GameState);
        }
        
        private void UpdateStorageFromState(GameState gameState)
        {
            itemStorageSize = gameState.BattlefieldStorage.Items.Count;
            state = gameState.BattlefieldStorage;
        }
    }
}