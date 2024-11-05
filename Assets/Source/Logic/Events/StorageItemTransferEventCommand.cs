using System.Collections.Generic;
using System.Linq;
using Source.Interactions;
using Source.Logic.State;
using Source.Logic.State.LineItems;
using UnityEngine;

namespace Source.Logic.Events
{
    public class StorageItemTransferEventCommand<T> : EventCommand where T : DataItem
    {
        private ItemStorage<T> fromStorage;
        private List<int> fromSlots;
        private ItemStorage<T> toStorage;
        private List<int> toSlots;

        public StorageItemTransferEventCommand(
                ItemStorage<T> fromStorage,
                List<int> fromSlots,
                ItemStorage<T> toStorage,
                List<int> toSlots
            )
        {
            this.fromStorage = fromStorage;
            this.fromSlots = fromSlots;
            this.toStorage = toStorage;
            this.toSlots = toSlots;
        }

        public override bool Perform()
        {
            AddLog($"Starting transfer of {fromSlots.Count} slots to {toSlots.Count} slots");

            if (fromSlots.Count > toSlots.Count)
            {
                AddLog("There are more from slots than to slots, cannot transfer. Exiting");
                return false;
            }
            
            // fromSlots.Union(toSlots).ToList().Count != fromSlots.Count
            // Maybe use Hashset for performance with large amounts of slots. Do when needed
            
            var success = true;
            for (var i = 0; i < fromSlots.Count; i++)
            {
                if (fromStorage.GetItemSlotReference(fromSlots[i], out var fromSlot) && 
                    toStorage.GetItemSlotReference(toSlots[i], out var toSlot))
                {
                    toSlot.Item = fromSlot.Item;
                    fromSlot.Item = null; 
                    AddLog($"Transferred slot {fromSlots[i]} to slot {toSlots[i]}");
                }
                else
                {
                    success = false;
                }
            }

            return success;
        }
    }
}
