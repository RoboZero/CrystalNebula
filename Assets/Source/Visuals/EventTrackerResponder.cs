using System.Collections.Generic;
using System.Threading;
using Source.Logic.Events;
using UnityEngine;

namespace Source.Visuals
{
    public abstract class EventTrackerResponder : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private EventTrackerBehavior eventTrackerBehavior;

        private Dictionary<EventCommand, CancellationTokenSource> cancellationTokens = new();

        private void Start()
        {
            eventTrackerBehavior.EventTracker.EventStarted += OnEventStarted;
            eventTrackerBehavior.EventTracker.EventCompleted += OnEventCompleted;
        }

        private void OnDisable()
        {
            eventTrackerBehavior.EventTracker.EventStarted -= OnEventStarted;
            eventTrackerBehavior.EventTracker.EventCompleted -= OnEventCompleted;
        }
        
        private void OnEventStarted(EventCommand eventCommand)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationTokenSource.Token, destroyCancellationToken);

            var created = CreateEventResponseTasks(eventCommand, linked.Token);
            if(created)
                cancellationTokens.Add(eventCommand, linked);
        }

        private void OnEventCompleted(EventCommand eventCommand)
        {
            Debug.Log($"RAM-7 Event completed: {eventCommand}");
            if (cancellationTokens.TryGetValue(eventCommand, out var source))
            {
                Debug.Log($"RAM-7 Found completed token stored: {eventCommand}");
                source.Cancel();
                cancellationTokens.Remove(eventCommand);
            }
        }

        /*
         * Return true if an event task was created and will be canceled once EventCommand task completes. 
         */
        protected abstract bool CreateEventResponseTasks(EventCommand eventCommand, CancellationToken eventCommandCancellationToken);
    }
}