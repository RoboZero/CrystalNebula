using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Logic.State.LineItems;
using Source.Utility;

namespace Source.Logic.Events
{
    public class LineStorageOpenMultiTransferEventCommand : EventCommand
    {
        // TODO: Consider removal propagation of sub-events.
        public List<LineStorageTransferEventCommand> TransferEventCommands { get; private set; } = new();
        public List<int> OpenSlots { get; private set; } = new();
        
        private List<LineStorage<MemoryItem>> fromStorages;
        private List<int> fromSlots;
        private LineStorage<MemoryItem> toStorage;
        private TransferEventOverrides transferEventOverrides;

        public LineStorageOpenMultiTransferEventCommand(
            EventTracker eventTracker,
            List<LineStorage<MemoryItem>> fromStorages,
            List<int> fromSlots,
            LineStorage<MemoryItem> toStorage,
            TransferEventOverrides transferEventOverrides
        ) : base(eventTracker)
        {
            this.fromStorages = fromStorages;
            this.fromSlots = fromSlots;
            this.toStorage = toStorage;
            this.transferEventOverrides = transferEventOverrides;
        }

        
        public override async UniTask<bool> Apply(CancellationToken cancellationToken)
        {
            AddLog($"{GetType().Name} Starting multiple line storage transfers from slots {fromStorages.ToItemString()}:{fromSlots.ToItemString()} to all {toStorage} open slots");
            var failurePrefix = "Failed to start multiple line storage transfers to open slots: ";

            for (var index = 0; index < toStorage.Items.Count; index++)
            {
                var item = toStorage.Items[index];
                if(item == null || transferEventOverrides.CanSwitch){
                    OpenSlots.Add(index);
                }
            }

            if (OpenSlots.Count == 0)
            {
                AddLog(failurePrefix + $"No open slots");
                return false;
            }

            var multiTransferEventCommand = new LineStorageMultiTransferEventCommand(
                eventTracker,
                fromStorages,
                fromSlots,
                toStorage,
                OpenSlots,
                transferEventOverrides
            );
            TransferEventCommands = multiTransferEventCommand.TransferEventCommands;
            var result = await ApplyChildEventWithLog(multiTransferEventCommand);

            return result;
        }
    }
}