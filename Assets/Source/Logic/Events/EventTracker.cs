using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Logic.State;
using UnityEngine;

namespace Source.Logic.Events
{
    // TODO: EventTrackerBehavior in Visuals, Logic should minimize Unity Dependencies.
    public class EventTracker
    {
        /*
         * Requirements:
         * Event changes game state
         * Any observers can track Events for animations
         * Visuals affected by Event are notified to update
         */
        public event Action<EventCommand> EventStarted;
        public event Action<EventCommand> EventCompleted;
        
        public List<EventCommand> EventCommands => eventCommands;
        
        private List<EventCommand> eventCommands = new();
        private GameState gameState;
        private CancellationToken creatorCancellationToken;

        public EventTracker(GameState gameState, CancellationToken creatorCancellationToken = default)
        {
            this.gameState = gameState;
            this.creatorCancellationToken = creatorCancellationToken;
        }

        public async UniTask<bool> AddEvent(EventCommand eventCommand, bool silent = false)
        {
            eventCommands.Add(eventCommand);
            //gameState.RunningEventCommands.Add(eventCommand);
            
            var task = eventCommand.Apply(creatorCancellationToken);
            EventStarted?.Invoke(eventCommand);
            var result = await task;
            EventCompleted?.Invoke(eventCommand);
            
            //gameState.RunningEventCommands.Remove(eventCommand);
            if(!silent)
                Debug.Log($"Event Tracker added and performed event: {eventCommand} \n {eventCommand.GetLog()}");
            
            return result;
        }
    }
}
