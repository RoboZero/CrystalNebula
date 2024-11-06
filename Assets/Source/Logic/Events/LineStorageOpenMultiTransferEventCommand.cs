using System.Collections.Generic;
using System.Linq;
using Source.Logic.State.LineItems;
using Source.Utility;

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
            AddLog($"{nameof(GetType)} Starting multiple line storage transfers from slots {fromSlots.ToItemString()} to all open slots");

            var openSlots = new List<int>();
            for (var index = 0; index < toStorage.Items.Count; index++)
            {
                var item = toStorage.Items[index];
                if(item != null){
                    openSlots.Add(index);
                }
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