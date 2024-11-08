using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Logic.State.LineItems;
using Source.Utility;

namespace Source.Logic.Events
{
    public class LineStorageOpenMultiTransferEventCommand : EventCommand
    {
        private List<LineStorage<MemoryItem>> fromStorages;
        private List<int> fromSlots;
        private LineStorage<MemoryItem> toStorage;
        private TransferEventOverrides transferEventOverrides;

        public LineStorageOpenMultiTransferEventCommand(
            List<LineStorage<MemoryItem>> fromStorages,
            List<int> fromSlots,
            LineStorage<MemoryItem> toStorage,
            TransferEventOverrides transferEventOverrides
        )
        {
            this.fromStorages = fromStorages;
            this.fromSlots = fromSlots;
            this.toStorage = toStorage;
            this.transferEventOverrides = transferEventOverrides;
        }

        
        public override async UniTask<bool> Perform(CancellationToken cancellationToken)
        {
            AddLog($"{GetType().Name} Starting multiple line storage transfers from slots {fromStorages.ToItemString()}:{fromSlots.ToItemString()} to all {toStorage} open slots");
            var failurePrefix = "Failed to start multiple line storage transfers to open slots: ";

            var openSlots = new List<int>();
            for (var index = 0; index < toStorage.Items.Count; index++)
            {
                var item = toStorage.Items[index];
                if(item == null || transferEventOverrides.CanSwitch){
                    openSlots.Add(index);
                }
            }

            if (openSlots.Count == 0)
            {
                AddLog(failurePrefix + $"No open slots");
                return false;
            }

            var result = await PerformChildEventWithLog(new LineStorageMultiTransferEventCommand(
                fromStorages,
                fromSlots,
                toStorage,
                openSlots,
                transferEventOverrides
            ), cancellationToken);

            return result;
        }
    }
}