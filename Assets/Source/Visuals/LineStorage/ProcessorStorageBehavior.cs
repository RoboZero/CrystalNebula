using Source.Logic.State;
using UnityEngine;

namespace Source.Visuals.LineStorage
{
    public class ProcessorStorageBehavior : LineStorageBehavior
    {
        [Header("Dependencies")]
        [SerializeField] private int processorIndex = 0;
        
        protected override void UpdateStorageFromState(GameState gameState)
        {
            var player = gameState.Players[playerId];

            if (processorIndex < 0 || processorIndex >= player.Processors.Count) return;
            
            var processor = player.Processors[processorIndex];
            itemStorageSize = processor.ProcessorStorage.Length;
            state = processor.ProcessorStorage;
        }
    }
}
