using System.Collections.Generic;
using Source.Logic.State.LineItems;
using Source.Utility;

namespace Source.Logic.Events
{
    public class LineStorageMultiTransferEventCommand : EventCommand
    {
        private List<LineStorage> fromStorages;
        private List<int> fromSlots;
        private LineStorage toStorage;
        private List<int> toSlots;
        private TransferEventOverrides transferEventOverrides;

        
        public LineStorageMultiTransferEventCommand(
            List<LineStorage> fromStorages,
            List<int> fromSlots,
            LineStorage toStorage,
            List<int> toSlot,
            TransferEventOverrides transferEventOverrides
        )
        {
            this.fromStorages = fromStorages;
            this.fromSlots = fromSlots;
            this.toStorage = toStorage;
            this.toSlots = toSlot;
            this.transferEventOverrides = transferEventOverrides;
        }

        public override bool Perform()
        {
            AddLog($"{nameof(GetType)} Starting multiple line storage transfers from storages {fromStorages.ToItemString()} slots {fromSlots.ToItemString()} to slot {toSlots.ToItemString()}");
            var failurePrefix = $"Unable to multi transfer: ";

            var success = true;

            for (var index = 0; index < fromSlots.Count; index++)
            {
                if (index > toSlots.Count)
                {
                    AddLog(failurePrefix + $"Unable to transfer at transfer {index}, from slots {fromSlots.Count} has more indices than to slots {toSlots.Count}");
                    success = false;
                    continue;
                }
                
                var result = PerformChildEventWithLog(new LineStorageTransferEventCommand(
                    fromStorages[index],
                    fromSlots[index],
                    toStorage,
                    toSlots[index],
                    transferEventOverrides
                ));

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