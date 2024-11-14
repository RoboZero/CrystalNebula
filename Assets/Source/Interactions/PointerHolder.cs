using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Input;
using Source.Logic.Events;
using Source.Logic.State;
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

        private bool lineStorageTransferring;
        private CancellationTokenSource transferCancelSource;
        private CancellationToken token;

        private void OnEnable()
        {
            inputReader.HoldPressedEvent += OnHoldPressed;
            inputReader.CommandCanceledEvent += OnCancel;
        }

        private void OnDisable()
        {
            inputReader.HoldPressedEvent -= OnHoldPressed;
            inputReader.CommandCanceledEvent -= OnCancel;
        }
        
        private void OnHoldPressed()
        {
            Debug.Log("Player pressed hold. ");
            bool hasInteracted = false;

            var interactedLines = playerInteractions.Interacted
                .OfType<LineGemItemVisual>()
                .ToList();

            if (lineStorageTransferring) return;
            
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

        private void OnCancel()
        {
            CancelTransferStorages();
        }

        private async UniTask TransferPersonalAndMemoryStorages(List<LineGemItemVisual> lineGemItemVisuals)
        {
            transferCancelSource = new CancellationTokenSource();
            token = transferCancelSource.Token;

            var interactedSlots = lineGemItemVisuals.Select(visual => visual.TrackedSlot).ToList();
            var interactedStorages = lineGemItemVisuals.Select(visual => visual.TrackedLineStorage).ToList();

            Debug.Log($"RAM-7 Transfer storage: Storages {interactedStorages.ToItemString()}, Slots {interactedSlots.ToItemString()}");

            var multiOpenTransferEvent = new LineStorageOpenMultiTransferEventCommand(
                eventTrackerBehavior.EventTracker,
                interactedStorages,
                interactedSlots,
                personalStorageBehavior.State,
                transferEventOverrides
            );

            lineStorageTransferring = true;
            await eventTrackerBehavior.EventTracker.AddEvent(multiOpenTransferEvent, false, token);
            lineStorageTransferring = false;
        }

        private void CancelTransferStorages()
        {
            if (!lineStorageTransferring) return;
            
            transferCancelSource.Cancel();
            transferCancelSource.Dispose();
            Debug.Log($"RAM-7 Transfer storage holder finished: canceled. Token status: {token.IsCancellationRequested}");
            lineStorageTransferring = false;
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