using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Logic.State.LineItems;
using Source.Utility;

namespace Source.Logic.Events
{
    public class LineStorageMultiTransferEventCommand : EventCommand
    {
        // TODO: Consider removal propagation of sub-events.
        public List<LineStorageTransferEventCommand> TransferEventCommands { get; } = new();

        private List<LineStorage<MemoryItem>> fromStorages;
        private List<int> fromSlots;
        private LineStorage<MemoryItem> toStorage;
        private List<int> toSlots;
        private TransferEventOverrides transferEventOverrides;

        public LineStorageMultiTransferEventCommand(
            EventTracker eventTracker,
            List<LineStorage<MemoryItem>> fromStorages,
            List<int> fromSlots,
            LineStorage<MemoryItem> toStorage,
            List<int> toSlots,
            TransferEventOverrides transferEventOverrides
        ) : base(eventTracker)
        {
            this.fromStorages = fromStorages;
            this.fromSlots = fromSlots;
            this.toStorage = toStorage;
            this.toSlots = toSlots;
            this.transferEventOverrides = transferEventOverrides;
        }

        public override async UniTask Apply(CancellationToken cancellationToken)
        {
            status = EventStatus.Started;
            
            AddLog($"{GetType().Name} Starting multiple line storage transfers from storages {fromStorages.ToItemString()}: slots {fromSlots.ToItemString()} to slot {toStorage}:{toSlots.ToItemString()}");
            var failurePrefix = $"Failed to multi transfer: ";

            if (fromStorages.Count != fromSlots.Count)
            {
                AddLog(failurePrefix + $"from storages Count {fromStorages.Count} is not equal to from slots count {fromSlots.Count}. Unable to determine which storage from slot is from. ");
                status = EventStatus.Failed;
                return;
            }

            var fails = 0;

            var toSet = new HashSet<int>();
            var tasks = new List<UniTask>();
            for (var index = 0; index < fromSlots.Count; index++)
            {
                if (index >= toSlots.Count)
                {
                    AddLog(failurePrefix + $"Unable to transfer at transfer {index}, from slots index {index} is greater than to slots count {toSlots.Count}");
                    fails++;
                    continue;
                }

                if (!toSet.Add(toSlots[index]))
                {
                    AddLog(failurePrefix + $" tried to transfer from multiple slots to a single to slot {toSlots[index]}");
                    fails++;
                    continue;
                }

                var transferEventCommand = new LineStorageTransferEventCommand(
                    eventTracker,
                    fromStorages[index],
                    fromSlots[index],
                    toStorage,
                    toSlots[index],
                    transferEventOverrides
                );
                TransferEventCommands.Add(transferEventCommand);
                var task = ApplyChildEventWithLog(transferEventCommand);
                
                tasks.Add(task);
            } 
            
            await UniTask.WhenAll(tasks);

            foreach (var transferEventCommand in TransferEventCommands)
            {
                if (transferEventCommand.Status == EventStatus.Failed)
                    fails++;
            }

            UpdateMultiStatus(fails, fromSlots.Count);
            AddLog($"Multi transfer Status: {status.ToString()}");
        }
    }
}