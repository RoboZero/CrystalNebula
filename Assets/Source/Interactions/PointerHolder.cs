using System;
using System.Linq;
using Source.Input;
using Source.Logic;
using Source.Logic.Events;
using Source.Logic.State;
using Source.Utility;
using Source.Visuals;
using Source.Visuals.MemoryStorage;
using UnityEngine;

namespace Source.Interactions
{
    public class PointerHolder : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private PlayerInteractions playerInteractions;
        [SerializeField] private PersonalStorageBehavior personalStorageBehavior;
        [SerializeField] private EventTracker eventTracker;
        [SerializeField] private InputReaderSO inputReader;

        private TransferEventOverrides transferEventOverrides = new TransferEventOverrides()
        {
            CanSwitch = true,
        };

        private void OnEnable()
        {
            inputReader.HoldPressedEvent += OnHoldPressed;
        }

        private void OnDisable()
        {
            inputReader.HoldPressedEvent -= OnHoldPressed;
        }
        
        private void OnHoldPressed()
        {
            Debug.Log("Player pressed hold. ");
            var interactedLines = playerInteractions.Interacted
                .OfType<LineGemItemVisual>()
                .ToList();

            var interactedSlots = interactedLines.Select(visual => visual.TrackedSlot).ToList();
            var interactedStorages= interactedLines.Select(visual => visual.TrackedLineStorage).ToList();
            
            Debug.Log($"Storages {interactedStorages.ToItemString()}, Slots {interactedSlots.ToItemString()}");
            
            eventTracker.AddEvent(new LineStorageOpenMultiTransferEventCommand(
                    interactedStorages,
                    interactedSlots,
                    personalStorageBehavior.State,
                    transferEventOverrides
                )
            );
            
            playerInteractions.Interacted.Clear();
            inputReader.ClickAndDrag = false;
        }
    }
}
