using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Logic.Events.Overrides;
using Source.Logic.State.LineItems;
using Source.Utility;

namespace Source.Logic.Events
{
    public class CreateLineStorageMemoryEventCommand : EventCommand
    {
        public float ProgressPercent => progressPercent;
        
        private EventTracker eventTracker;
        private LineStorage<MemoryItem> memoryStorage;
        private int slot;
        private MemoryItem memoryItem;
        private CreateMemoryEventOverrides createMemoryEventOverrides;
        
        private float progressPercent;

        public CreateLineStorageMemoryEventCommand(
            EventTracker eventTracker,
            LineStorage<MemoryItem> memoryStorage,
            int slot,
            MemoryItem memoryItem,
            CreateMemoryEventOverrides createMemoryEventOverrides
        ) : base(eventTracker)
        {
            this.eventTracker = eventTracker;
            this.memoryStorage = memoryStorage;
            this.slot = slot;
            this.memoryItem = memoryItem;
            this.createMemoryEventOverrides = createMemoryEventOverrides;

            status = EventStatus.Created;
        }

        public override async UniTask Apply(CancellationToken cancellationToken)
        {
            status = EventStatus.Started;
            AddLog($"{GetType().Name} Creating line storage memory in {memoryStorage}:{slot}");
            var failurePrefix = $"Unable to create line storage memory in {memoryStorage}:{slot}: ";

            if (!memoryStorage.Items.InBounds(slot))
            {
                status = EventStatus.Failed;
                AddLog(failurePrefix + $"slot out of bounds {memoryStorage.Items.Count}");
                return;
            }
            
            if (createMemoryEventOverrides is { Overwrite: false } && memoryStorage.Items[slot] != null)
            {
                status = EventStatus.Failed;
                AddLog(failurePrefix + $"unable to overwrite item in slot {memoryStorage.Items[slot]}");
                return;
            }

            if (createMemoryEventOverrides is { CreationTime: > 0 })
            {
                try
                {
                    await DOVirtualAsync(0, 1, createMemoryEventOverrides.CreationTime, (x) => { progressPercent = x; }, cancellationToken);
                }
                catch (Exception e)
                {
                    status = EventStatus.Failed;
                    AddLog(failurePrefix + "creation time canceled");
                    return;
                }
            }
            
            memoryStorage.Items[slot] = memoryItem;
            status = EventStatus.Success;
            AddLog($"Successfully added Line Storage Memory {memoryItem} at slot {slot}");
        }
    }
}