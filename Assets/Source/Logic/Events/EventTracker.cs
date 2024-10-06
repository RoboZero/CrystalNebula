using System.Collections.Generic;
using UnityEngine;

namespace Source.Logic.Events
{
    public class EventTracker : MonoBehaviour
    {
        private List<EventCommand> eventCommands = new();

        public void AddEvent(EventCommand eventCommand)
        {
            Debug.Log($"Event Tracker added and performed event: {eventCommand}");
            eventCommands.Add(eventCommand);
            eventCommand.Perform();
        }
    }
}
