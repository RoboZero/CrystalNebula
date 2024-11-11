using System;
using System.Collections.Generic;
using System.Text;
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
        public event Action<EventCommand> EventFinished;
        
        public List<EventCommand> EventCommands => eventCommands;
        
        private List<EventCommand> eventCommands = new();
        private GameState gameState;
        private CancellationToken creatorCancellationToken;

        public EventTracker(GameState gameState, CancellationToken creatorCancellationToken = default)
        {
            this.gameState = gameState;
            this.creatorCancellationToken = creatorCancellationToken;
        }

        public async UniTask AddEvent(EventCommand eventCommand, bool silent = false, CancellationToken eventCallerCancellationToken = default)
        {
            eventCommands.Add(eventCommand);
            var linked = CancellationTokenSource.CreateLinkedTokenSource(eventCallerCancellationToken, creatorCancellationToken);
            var logBuilder = new StringBuilder().AppendLine($"Event Tracker added and performed event: {eventCommand}");
            
            EventStarted?.Invoke(eventCommand);
            try
            {
                await eventCommand.Apply(linked.Token);
            }
            catch (OperationCanceledException operationCanceledException) { }
            
            logBuilder.AppendLine($"Status: {eventCommand.Status.ToString()}").AppendLine(eventCommand.GetLog());
            EventFinished?.Invoke(eventCommand);
            
            if(!silent)
                Debug.Log(logBuilder);
        }
    }
}
