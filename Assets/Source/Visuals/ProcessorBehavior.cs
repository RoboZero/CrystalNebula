using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Source.Logic;
using Source.Logic.Events;
using Source.Serialization;
using Source.Serialization.Data;
using Source.Utility;
using Source.Visuals.LineStorage;
using UnityEngine;

namespace Source.Visuals
{
    // Debug
    public class ProcessorBehavior : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private GameStateLoader gameStateLoader;
        [SerializeField] private ProcessorStorageBehavior processorStorageBehavior;
        [SerializeField] private EventTracker eventTracker;

        [Header("Settings")]
        [SerializeField] private int playerID = 0;
        [SerializeField] private int processorIndex = 0;

        private float time;

        private void Update()
        {
            var processor = processorStorageBehavior.Processor;

            if (processor == null)
            {
                Debug.LogWarning("Processor Behavior's processor is null");
                return;
            }
            
            time += Time.deltaTime;
            if (time >= processor.ClockSpeed)
            {
                foreach (var lineItem in processorStorageBehavior.State.Items)
                {
                    if (lineItem.Memory != null)
                    {
                        lineItem.Memory.Tick(eventTracker, gameStateLoader.GameState);
                    }
                    else
                    {
                        Debug.LogWarning($"Line item {lineItem} memory is null");
                    }
                }
                time = 0;
            }
        }
    }
}