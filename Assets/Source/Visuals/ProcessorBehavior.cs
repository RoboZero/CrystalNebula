using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Source.Logic;
using Source.Logic.Events;
using Source.Serialization;
using Source.Visuals.LineStorage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Visuals
{
    // Debug
    public class ProcessorBehavior : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private GameStateLoader gameStateLoader;
        [SerializeField] private ProcessorStorageBehavior processorStorageBehavior;
        [SerializeField] private EventTracker eventTracker;
        
        [SerializeField] private TextMeshProUGUI clockSpeedText;
        [SerializeField] private Image flashingPanel;
        [SerializeField] private Gradient flashingPanelGradient;

        private float time;

        private void Update()
        {
            var processor = processorStorageBehavior.Processor;

            if (processor == null)
            {
                Debug.LogWarning("Processor Behavior's processor is null");
                return;
            }

            clockSpeedText.text = processor.ClockSpeed.ToString(CultureInfo.InvariantCulture);
            flashingPanel.color = flashingPanelGradient.Evaluate(time / processor.ClockSpeed);

            time += Time.deltaTime;

            if (processor.ClockSpeed < 0.1f)
            {
                Debug.LogWarning("Please don't set processor clock speed below 0.1f");
                return;
            }
            
            if (time >= 1 / processor.ClockSpeed)
            {
                foreach (var lineItem in processorStorageBehavior.State.Items)
                {
                    if (lineItem.Memory != null)
                    {
                        lineItem.Memory.Tick(eventTracker, gameStateLoader.GameState);
                    }
                    else
                    {
                        Debug.Log($"Line item {lineItem} memory is null");
                    }
                }
                time = 0;
            }
        }
    }
}