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
            List<LineStorage<MemoryItem>> fromStorages,
            List<int> fromSlots,
            LineStorage<MemoryItem> toStorage,
            List<int> toSlots,
            TransferEventOverrides transferEventOverrides
        )
        {
            this.fromStorages = fromStorages;
            this.fromSlots = fromSlots;
            this.toStorage = toStorage;
            this.toSlots = toSlots;
            this.transferEventOverrides = transferEventOverrides;
        }

        public override async UniTask<bool> Apply(CancellationToken cancellationToken)
        {
            AddLog($"{GetType().Name} Starting multiple line storage transfers from storages {fromStorages.ToItemString()}: slots {fromSlots.ToItemString()} to slot {toStorage}:{toSlots.ToItemString()}");
            var failurePrefix = $"Failed to multi transfer: ";

            if (fromStorages.Count != fromSlots.Count)
            {
                AddLog(failurePrefix +
                       $"from storages Count {fromStorages.Count} is not equal to from slots count {fromSlots.Count}. Unable to determine which storage from slot is from. ");
                return false;
            }

            var success = true;

            var toSet = new HashSet<int>();
            var tasks = new List<UniTask<bool>>();
            for (var index = 0; index < fromSlots.Count; index++)
            {
                if (index >= toSlots.Count)
                {
                    AddLog(failurePrefix + $"Unable to transfer at transfer {index}, from slots index {index} is greater than to slots count {toSlots.Count}");
                    success = false;
                    continue;
                }

                if (!toSet.Add(toSlots[index]))
                {
                    AddLog(failurePrefix + $" tried to transfer from multiple slots to a single to slot {toSlots[index]}");
                    continue;
                }

                var transferEventCommand = new LineStorageTransferEventCommand(
                    fromStorages[index],
                    fromSlots[index],
                    toStorage,
                    toSlots[index],
                    transferEventOverrides
                );
                TransferEventCommands.Add(transferEventCommand);
                var task = ApplyChildEventWithLog(transferEventCommand, cancellationToken);
                
                tasks.Add(task);
            }

            var results = await UniTask.WhenAll(tasks);

            foreach (var result in results)
            {
                if (!result)
                    success = false;
            }

            if(success)
                AddLog($"Successfully multi transferred");
            else
                AddLog($"Failed to fully multi transfer");
            
            return success;
        }
    }
}