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
            var nonTransferringVisuals = lineGemItemVisuals.Where(item => !item.IsTransferring);
                
            var interactedSlots = lineGemItemVisuals.Select(visual => visual.TrackedSlot).ToList();
            var interactedStorages = lineGemItemVisuals.Select(visual => visual.TrackedLineStorage).ToList();
            
            Debug.Log($"Storages {interactedStorages.ToItemString()}, Slots {interactedSlots.ToItemString()}");
            
            var multiOpenTransferEvent = new LineStorageOpenMultiTransferEventCommand(
                interactedStorages,
                interactedSlots,
                personalStorageBehavior.State,
                transferEventOverrides
            );

            var transferEventTask = eventTracker.AddEvent(multiOpenTransferEvent);
            
            var personalOpenSlotVisuals = new List<LineGemItemVisual>();
            var personalOpenSlots = multiOpenTransferEvent.OpenSlots;
            foreach (var personalOpenSlot in personalOpenSlots)
            { 
                personalOpenSlotVisuals.Add(personalLineGemStorageVisual.GetItemVisual(personalOpenSlot));
                Debug.Log("Personal item: " + personalOpenSlot);
            }

            personalOpenSlotVisuals.Dump();

            var fromTransferEventDictionary = multiOpenTransferEvent.TransferEventCommands.ToDictionary(item => item.FromSlot);
            var toTransferEventDictionary = multiOpenTransferEvent.TransferEventCommands.ToDictionary(item => item.ToSlot);

            foreach (var VARIABLE in toTransferEventDictionary)
            {
                Debug.Log("To Transfer Dictionary item " + VARIABLE);
            }
            
            // TODO: Separate logic and animation, more elegant way to update progress.
            UpdateStorageTransferPercentProgress(lineGemItemVisuals, fromTransferEventDictionary, transferEventTask);
            UpdateStorageTransferPercentProgress(personalOpenSlotVisuals, toTransferEventDictionary, transferEventTask);
        }

        private async UniTask UpdateStorageTransferPercentProgress(List<LineGemItemVisual> lineGemItemVisuals, Dictionary<int, LineStorageTransferEventCommand> transferEventCommands, UniTask doUntil)
        {
            do
            {
                foreach (var lineGemItemVisual in lineGemItemVisuals)
                {
                    if (transferEventCommands.ContainsKey(lineGemItemVisual.TrackedSlot))
                        lineGemItemVisual.SetTransferProgressPercent(transferEventCommands[lineGemItemVisual.TrackedSlot].TransferPercentProgress);
                }

                await UniTask.NextFrame(destroyCancellationToken);
            } while (!doUntil.GetAwaiter().IsCompleted && !destroyCancellationToken.IsCancellationRequested);
        }
        
        private void TransferPersonalAndBattlefieldStorage(List<BattlefieldItemVisual> battlefieldItemVisuals)
        {
            var interactedBattlefieldSlots = battlefieldItemVisuals.Select(visual => visual.TrackedSlot).ToList();
            var interactedBattlefields = battlefieldItemVisuals.Select(visual => visual.TrackedBattlefieldStorage).ToList();

            eventTracker.AddEvent(new LineStorageBattlefieldOpenMultiTransferEventCommand(
                    interactedBattlefields,
                    interactedBattlefieldSlots,
                    personalStorageBehavior.State,
                    LineStorageBattlefieldTransferEventCommand.TransferredItem.Unit,
                    transferEventOverrides
                )
            );
        }
    }
}
