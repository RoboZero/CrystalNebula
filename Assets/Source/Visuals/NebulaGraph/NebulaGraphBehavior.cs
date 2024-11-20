using System.Linq;
using Source.Logic.State;
using Source.Logic.State.LineItems;
using Source.Logic.State.ResearchGraphs;
using Source.Serialization;
using UnityEngine;

namespace Source.Visuals.NebulaGraph
{
    public class NebulaGraphBehavior : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private GameStateLoader gameStateLoader;

        [Header("Settings")]
        [SerializeField] private  int playerId = 0;

        public int PlayerId => playerId;
        public Level Level => level;
        public ResearchGraph State => state;

        private Level level;
        private ResearchGraph state;

        public void Tick()
        {
            if(gameStateLoader.GameState != null)
                UpdateStorageFromState(gameStateLoader.GameState);
        }

        private void UpdateStorageFromState(GameState gameState)
        {
            var player = gameState.Players.FirstOrDefault(player => player.Id == playerId);
            if (player == null)
            {
                Debug.LogWarning($"Failed to read from disk storage: playerId {playerId} is invalid, gamestate players count {gameState.Players.Count}");
                return;
            }
            
            level = gameState.Level;
            state = player.ResearchGraph;
        }
    }
}