using System;
using System.Collections.Generic;
using System.Linq;
using Source.Input;
using Source.Logic;
using Source.Logic.Events;
using Source.Logic.State;
using Source.Logic.State.LineItems;
using Source.Utility;
using Source.Visuals;
using Source.Visuals.BattlefieldStorage;
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

        private List<int> allPersonalSlots = new();
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
            /*
            var interactedLines = playerInteractions.Interacted
                .OfType<LineGemItemVisual>()
                .ToList();

            var interactedSlots = interactedLines.Select(visual => visual.TrackedSlot).ToList();
            var interactedStorages = interactedLines.Select(visual => visual.TrackedLineStorage).ToList();
            
            Debug.Log($"Storages {interactedStorages.ToItemString()}, Slots {interactedSlots.ToItemString()}");
            
            eventTracker.AddEvent(new LineStorageOpenMultiTransferEventCommand(
                    interactedStorages,
                    interactedSlots,
                    personalStorageBehavior.State,
                    transferEventOverrides
                )
            );
            */
            var interactedBattlefieldItems = playerInteractions.Interacted
                .OfType<BattlefieldItemVisual>()
                .ToList();

            var interactedBattlefieldSlots = interactedBattlefieldItems.Select(visual => visual.TrackedSlot).ToList();
            var interactedBattlefields = interactedBattlefieldItems.Select(visual => visual.TrackedBattlefieldStorage).ToList();

            for (var i = 0; i < personalStorageBehavior.State.Items.Count; i++)
            { 
                allPersonalSlots.Add(i);
            }
            
            eventTracker.AddEvent(new LineStorageBattlefieldMultiTransferEventCommand(
                    interactedBattlefields,
                    interactedBattlefieldSlots,
                    personalStorageBehavior.State,
                    allPersonalSlots,
                    LineStorageBattlefieldTransferEventCommand.TransferredItem.Unit,
                    transferEventOverrides
                )
            );

            allPersonalSlots.Clear();
            playerInteractions.Interacted.Clear();
            inputReader.ClickAndDrag = false;
        }
    }
}
