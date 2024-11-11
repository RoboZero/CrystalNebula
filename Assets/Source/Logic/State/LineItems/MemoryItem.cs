using System;
using Source.Logic.Events;
using UnityEngine;

namespace Source.Logic.State.LineItems
{
    [Serializable]
    public class MemoryItem : LineItem
    {
        public int OwnerId;
        public string Definition;
        public int CurrentRunProgress;
        public int MaxRunProgress;
        public float DataSize;

        public virtual void Tick(EventTracker eventTracker, GameState gameState)
        {
            Debug.Log($"{Definition} tick. Progress: {CurrentRunProgress} / {MaxRunProgress}");
            
            CurrentRunProgress++;

            if (CurrentRunProgress <= MaxRunProgress) return;
            
            Run(eventTracker, gameState);
            CurrentRunProgress = 0;
        }

        protected virtual void Run(EventTracker eventTracker, GameState gameState) { }

        public override string ToString()
        {
            return $"{Definition}:(OId: {OwnerId}, Prog: {CurrentRunProgress}, Max Prog: {MaxRunProgress})";
        }
    }
}