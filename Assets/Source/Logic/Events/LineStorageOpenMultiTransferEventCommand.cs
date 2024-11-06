using System.Collections.Generic;
using System.Linq;
using Source.Logic.State.LineItems;
using Source.Utility;
using UnityEngine;

namespace Source.Logic.Events
{
    public class LineStorageOpenMultiTransferEventCommand : EventCommand
    {
        private List<LineStorage> fromStorages;
        private List<int> fromSlots;
        private LineStorage toStorage;
        private TransferEventOverrides transferEventOverrides;

        public LineStorageOpenMultiTransferEventCommand(
            List<LineStorage> fromStorages,
            List<int> fromSlots,
            LineStorage toStorage,
            TransferEventOverrides transferEventOverrides
        )
        {
            this.fromStorages = fromStorages;
            this.fromSlots = fromSlots;
            this.toStorage = toStorage;
            this.transferEventOverrides = transferEventOverrides;
        }

        
        public override bool Perform()
        {
            AddLog($"{GetType().Name} Starting multiple line storage transfers from slots {fromStorages.ToItemString()}:{fromSlots.ToItemString()} to all {toStorage} open slots");
            var failurePrefix = "Failed to start multiple line storage transfers to open slots: ";

            var openSlots = new List<int>();
            Debug.Log($"Personal storage item count = {toStorage.Items.Count}");
            for (var index = 0; index < toStorage.Items.Count; index++)
            {
                var item = toStorage.Items[index];
                if(item.Memory == null || transferEventOverrides.CanSwitch){
                    Debug.Log($"Adding slot {index}");
                    openSlots.Add(index);
                }
            }

            if (openSlots.Count == 0)
            {
                AddLog(failurePrefix + $"No open slots");
                return false;
            }

            var result = PerformChildEventWithLog(new LineStorageMultiTransferEventCommand(
                fromStorages,
                fromSlots,
                toStorage,
                openSlots,
                transferEventOverrides
            ));

            return result;
        }
    }
}