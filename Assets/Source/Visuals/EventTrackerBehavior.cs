using System;
using Cysharp.Threading.Tasks;
using Source.Logic.Events;
using Source.Serialization;
using UnityEngine;

namespace Source.Visuals
{
    public class EventTrackerBehavior : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private GameStateLoader gameStateLoader;

        public EventTracker EventTracker => eventTracker;

        private EventTracker eventTracker;

        private void Awake()
        {
            eventTracker = new EventTracker(gameStateLoader.GameState, destroyCancellationToken);
        }
    }
}