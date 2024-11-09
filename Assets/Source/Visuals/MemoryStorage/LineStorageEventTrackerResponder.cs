using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Logic.Events;
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
                        
                        TransferItem(lineStorageTransferEventCommand.FromSlot, lineStorageTransferEventCommand, eventCommandCancellationToken);
                        created = true;
                    }

                    if (lineStorageBehavior.State == lineStorageTransferEventCommand.ToStorage)
                    {
                        Debug.Log(
                            $"{this.GetType().Name} {gameObject.name} detected matching TO transfer event from {lineStorageBehavior.State}");
                        TransferItem(lineStorageTransferEventCommand.ToSlot, lineStorageTransferEventCommand, eventCommandCancellationToken);
                        created = true;
                    }

                    break;
            }

            return created;
        }
        
        private void TransferItem(int slot, LineStorageTransferEventCommand command, CancellationToken cancellationToken)
        {
            var visual = lineGemStorageVisual.GetItemVisual(slot);

            if (visual == null)
            {
                Debug.Log($"Transfer animation visual at slot {command.FromSlot} is null");
            }
            
            TransferItemAsync(visual, command, cancellationToken);
        }
        
        private async UniTask TransferItemAsync(LineGemItemVisual visual, LineStorageTransferEventCommand command, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                visual.SetTransferProgressPercent(command.TransferPercentProgress);
                await UniTask.NextFrame(cancellationToken);
            }
        }
    }
}