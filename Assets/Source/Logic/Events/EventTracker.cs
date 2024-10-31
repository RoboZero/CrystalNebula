using System.Collections.Generic;
using UnityEngine;

namespace Source.Logic.Events
{
    public class EventTracker : MonoBehaviour
    {
        public List<EventCommand> EventCommands => eventCommands;
        
        private List<EventCommand> eventCommands = new();

        public bool AddEvent(EventCommand eventCommand)
        {
            eventCommands.Add(eventCommand);
            var result = eventCommand.Perform();
            Debug.Log($"Event Tracker added and performed event: {eventCommand} \n {eventCommand.GetLog()}");
            return result;
        }
    }
}
