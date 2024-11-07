using Source.Logic.State.LineItems;
using Source.Utility;

namespace Source.Logic.Events
{
    public class LineStorageTransferEventCommand : EventCommand
    {
        private LineStorage<MemoryItem> fromStorage;
        private int fromSlot;
        private LineStorage<MemoryItem> toStorage;
        private int toSlot;
        private TransferEventOverrides transferEventOverrides;

        public LineStorageTransferEventCommand(
            LineStorage<MemoryItem> fromStorage,
            int fromSlot,
            LineStorage<MemoryItem> toStorage,
            int toSlot,
            TransferEventOverrides transferEventOverrides
        )
        {
            this.fromStorage = fromStorage;
            this.fromSlot = fromSlot;
            this.toStorage = toStorage;
            this.toSlot = toSlot;
            this.transferEventOverrides = transferEventOverrides;
        }

        public override bool Perform()
        {
            AddLog($"{GetType().Name} Starting line storage transfer from slot {fromStorage}:{fromSlot} to slot {fromStorage}:{toSlot}");
            var failurePrefix = $"Unable to transfer from {fromStorage}:{fromSlot} to {toStorage}:{toSlot}: ";

            if (!fromStorage.Items.InBounds(fromSlot))
            {
                AddLog(failurePrefix + $"from slot {fromSlot} is not in fromStorage bounds length {fromStorage.Items.Count}");
                return false;
            }
            
            if (!toStorage.Items.InBounds(toSlot))
            {
                AddLog(failurePrefix + $"to slot {toSlot} is not in toStorage bounds length {toStorage.Items.Count}");
                return false;
            }

            if (transferEventOverrides != null && !transferEventOverrides.CanSwitch && toStorage.Items[toSlot] != null)
            {
                AddLog(failurePrefix + $"cannot switch and to slot {toSlot} has item in it {toStorage.Items[toSlot]}");
                return false;
            }

            (toStorage.Items[toSlot], fromStorage.Items[fromSlot]) = (fromStorage.Items[fromSlot], toStorage.Items[toSlot]);
            AddLog($"Successfully transferred slot {fromSlot} to slot {toSlot}");
            
            return true;
        }
    }
}