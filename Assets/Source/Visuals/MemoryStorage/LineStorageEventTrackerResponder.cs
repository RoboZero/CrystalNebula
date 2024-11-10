using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Logic.Events;
using Source.Logic.State.LineItems;
using UnityEngine;

namespace Source.Visuals.MemoryStorage
{
    public class LineStorageEventTrackerResponder : EventTrackerResponder
    {
        [Header("Line Storage Dependencies")]
        [SerializeField] private LineStorageBehavior lineStorageBehavior;
        [SerializeField] private LineGemStorageVisual lineGemStorageVisual;
        
        protected override bool CreateEventResponseTasks(EventCommand eventCommand, CancellationToken eventCommandCancellationToken)
        {
            var created = false;
            switch (eventCommand)
            {
                case LineStorageTransferEventCommand lineStorageTransferEventCommand:
                    if (lineStorageBehavior.State == lineStorageTransferEventCommand.FromStorage)
                    {
                        Debug.Log(
                            $"{this.GetType().Name} {gameObject.name} detected matching FROM transfer event from {lineStorageBehavior.State}");
                        
                        TransferItem(lineStorageTransferEventCommand.FromSlot, true, lineStorageTransferEventCommand, eventCommandCancellationToken);
                        created = true;
                    }

                    if (lineStorageBehavior.State == lineStorageTransferEventCommand.ToStorage)
                    {
                        Debug.Log(
                            $"{this.GetType().Name} {gameObject.name} detected matching TO transfer event from {lineStorageBehavior.State}");
                        TransferItem(lineStorageTransferEventCommand.ToSlot, false, lineStorageTransferEventCommand, eventCommandCancellationToken);
                        created = true;
                    }

                    break;
            }

            return created;
        }
        
        private void TransferItem(int slot, bool TFromFTo, LineStorageTransferEventCommand command, CancellationToken cancellationToken)
        {
            var visual = lineGemStorageVisual.GetItemVisual(slot);
            var otherMemory = TFromFTo
                ? command.FromStorage.Items[command.FromSlot]
                : command.ToStorage.Items[command.ToSlot];

            if (visual == null)
            {
                Debug.Log($"Transfer animation visual at slot {command.FromSlot} is null");
            }
            
            TransferItemAsync(visual, otherMemory, command, cancellationToken);
            visual.SetTransferProgressPercent(1);
        }
        
        private async UniTask TransferItemAsync(LineGemItemVisual visual, MemoryItem otherMemory, LineStorageTransferEventCommand command, CancellationToken cancellationToken)
        {
            do
            {
                visual.SetTransferProgressPercent(command.TransferPercentProgress);
                await UniTask.NextFrame(cancellationToken);
            } while (!cancellationToken.IsCancellationRequested);
        }
    }
}