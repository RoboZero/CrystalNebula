using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Interactions;
using Source.Logic.State;
using Source.Logic.State.LineItems;
using UnityEngine;

namespace Source.Logic.Events
{
    public class StorageItemTransferEventCommand<T> : EventCommand where T : LineItem
    {
        private ItemStorage<T> fromStorage;
        private List<int> fromSlots;
        private ItemStorage<T> toStorage;
        private List<int> toSlots;

        public StorageItemTransferEventCommand(
            EventTracker eventTracker,
            ItemStorage<T> fromStorage,
            List<int> fromSlots,
            ItemStorage<T> toStorage,
            List<int> toSlots
        ) : base(eventTracker)
        {
            this.fromStorage = fromStorage;
            this.fromSlots = fromSlots;
            this.toStorage = toStorage;
            this.toSlots = toSlots;
        }

        public override async UniTask Apply(CancellationToken cancellationToken)
        {
            status = EventStatus.Started;
            AddLog($"Starting transfer of {fromSlots.Count} slots to {toSlots.Count} slots");

            if (fromSlots.Count > toSlots.Count)
            {
                AddLog("There are more from slots than to slots, cannot transfer. Exiting");
                status = EventStatus.Failed;
                return;
            }
            
            // fromSlots.Union(toSlots).ToList().Count != fromSlots.Count
            // Maybe use Hashset for performance with large amounts of slots. Do when needed
            
            var fails = 0;
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
                    fails++;
                }
            }

            status = EventStatus.Success;
        }
    }
}
