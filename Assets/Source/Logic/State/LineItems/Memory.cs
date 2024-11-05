using System;
using Source.Logic.Events;
using UnityEngine;

namespace Source.Logic.State.LineItems
{
    [Serializable]
    public class Memory
    {
        public int OwnerId;
        public string Definition;
        public int CurrentProgress;
        public int MaxProgress;

        public virtual void Tick(EventTracker eventTracker, GameState gameState)
        {
            Debug.Log($"{Definition} tick. Progress: {CurrentProgress} / {MaxProgress}");
            
            CurrentProgress++;

            if (CurrentProgress <= MaxProgress) return;
            
            Run(eventTracker, gameState);
            CurrentProgress = 0;
        }

        protected virtual void Run(EventTracker eventTracker, GameState gameState)
        {
            
        }
    }
}