using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Input;
using Source.Logic.Events;
using Source.Logic.State;
using Source.Serialization;
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
        [SerializeField] private LineGemStorageVisual personalLineGemStorageVisual;
        [SerializeField] private EventTrackerBehavior eventTrackerBehavior;
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
            bool hasInteracted = false;

            var interactedLines = playerInteractions.Interacted
                .OfType<LineGemItemVisual>()
                .ToList();

            if (interactedLines.Count > 0)
            {
                TransferPersonalAndMemoryStorages(interactedLines);
                hasInteracted = true;
            }

            if (!hasInteracted)
            {
                var interactedBattlefieldItems = playerInteractions.Interacted
                    .OfType<BattlefieldItemVisual>()
                    .ToList();

                if (interactedBattlefieldItems.Count > 0)
                {
                    TransferPersonalAndBattlefieldStorage(interactedBattlefieldItems);
                }
            }

            playerInteractions.Interacted.Clear();
            inputReader.ClickAndDrag = false;
        }

        private void TransferPersonalAndMemoryStorages(List<LineGemItemVisual> lineGemItemVisuals)
        {
            var interactedSlots = lineGemItemVisuals.Select(visual => visual.TrackedSlot).ToList();
            var interactedStorages = lineGemItemVisuals.Select(visual => visual.TrackedLineStorage).ToList();

            Debug.Log($"Storages {interactedStorages.ToItemString()}, Slots {interactedSlots.ToItemString()}");

            var multiOpenTransferEvent = new LineStorageOpenMultiTransferEventCommand(
                eventTrackerBehavior.EventTracker,
                interactedStorages,
                interactedSlots,
                personalStorageBehavior.State,
                transferEventOverrides
            );

            eventTrackerBehavior.EventTracker.AddEvent(multiOpenTransferEvent);
        }

        private void TransferPersonalAndBattlefieldStorage(List<BattlefieldItemVisual> battlefieldItemVisuals)
        {
            var interactedBattlefieldSlots = battlefieldItemVisuals.Select(visual => visual.TrackedSlot).ToList();
            var interactedBattlefields =
                battlefieldItemVisuals.Select(visual => visual.TrackedBattlefieldStorage).ToList();

            eventTrackerBehavior.EventTracker.AddEvent(new LineStorageBattlefieldOpenMultiTransferEventCommand(
                eventTrackerBehavior.EventTracker,
                interactedBattlefields,
                interactedBattlefieldSlots,
                personalStorageBehavior.State,
                LineStorageBattlefieldTransferEventCommand.TransferredItem.Unit,
                transferEventOverrides
            ));
        }
    }
}