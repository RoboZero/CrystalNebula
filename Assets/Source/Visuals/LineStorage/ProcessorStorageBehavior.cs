using Source.Logic.State;
using Source.Utility;
using UnityEngine;

namespace Source.Visuals.LineStorage
{
    public class ProcessorStorageBehavior : LineStorageBehavior
    {
        [Header("Dependencies")]
        [SerializeField] private int processorIndex = 0;
        
        protected override void UpdateStorageFromState(GameState gameState)
        {
            if (!gameState.Players.InBounds(playerId))
            {
                Debug.LogWarning($"Failed to read from processor: processor index {processorIndex} is invalid, gamestate players count {gameState.Players.Count}" );
                return;
            }
            
            var player = gameState.Players[playerId];

            if (!player.Processors.InBounds(processorIndex)) return;
            
            var processor = player.Processors[processorIndex];
            itemStorageSize = processor.ProcessorStorage.Length;
            state = processor.ProcessorStorage;
        }
    }
}
