using Source.Logic;
using Source.Logic.Data;
using Source.Logic.State;
using Source.Serialization;
using Source.Utility;
using UnityEngine;

namespace Source.Visuals.Battlefield
{
    public class BattlefieldStorageBehavior : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int itemStorageSize;
        
        [SerializeField] private GameStateLoader gameStateLoader;

        public BattlefieldStorage State => state;
 
        private BattlefieldStorage state;

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