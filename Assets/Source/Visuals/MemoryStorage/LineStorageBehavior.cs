using Cysharp.Threading.Tasks;
using Source.Logic.Events;
using Source.Logic.State;
using Source.Logic.State.LineItems;
using Source.Serialization;
using UnityEngine;

namespace Source.Visuals.MemoryStorage
{
    public abstract class LineStorageBehavior : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] protected GameStateLoader gameStateLoader;

        [Header("Settings")]
        [SerializeField] protected int playerID = 0;
        [SerializeField] protected int itemStorageSize;

        public LineStorage<MemoryItem> State => state;
        protected LineStorage<MemoryItem> state;

        public void Tick()
        {
            if(gameStateLoader.GameState != null)
                UpdateStorageFromState(gameStateLoader.GameState);
        }

        protected abstract void UpdateStorageFromState(GameState gameState);
    }
}
