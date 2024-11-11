using System;
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
        
        private async UniTask TransferItem(int slot, bool TFromFTo, LineStorageTransferEventCommand command, CancellationToken cancellationToken)
        {
            var visual = lineGemStorageVisual.GetItemVisual(slot);
            var transferMemory = TFromFTo
                ? command.ToStorage.Items[command.ToSlot]
                : command.FromStorage.Items[command.FromSlot];
            
            if (visual == null)
            {
                Debug.Log($"Transfer animation visual at slot {command.FromSlot} is null");
                return;
            }
            
            visual.SetTransferProgressPercent(0);
            visual.SetTransferDataItem(transferMemory);
            visual.IsTransferring(true);
            
            /*
             * I get it! The epiphany - if I call an async and don't wait, then the rest of the code happens IMMEDIATELY
             * Want to mark this mental achievement :)
             * Second epiphany - cancellation token cancel throws an exception, that's why the task doesn't finish.
             * TODO: Determine better way for UniTask completion than try catch.
             */
            
            try
            {
                await TransferItemAsync(visual, command, cancellationToken);
            }
            catch (OperationCanceledException e) 
            {
                Debug.Log($"RAM-7 Finished transfer animation. Command status {command.Status.ToString()}");
                visual.SetTransferProgressPercent(1);
                visual.SetTransferDataItem(null);
                visual.IsTransferring(false);

                if (command.Status == EventCommand.EventStatus.Success)
                {
                    
                }
            }
        }
        
        private async UniTask TransferItemAsync(LineGemItemVisual visual, LineStorageTransferEventCommand command, CancellationToken cancellationToken)
        {
            do
            {
                visual.SetTransferProgressPercent(command.TransferProgressPercent);
                await UniTask.NextFrame(cancellationToken);
            } while (!cancellationToken.IsCancellationRequested);
        }
    }
}