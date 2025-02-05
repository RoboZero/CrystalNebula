using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Source.Logic.State;
using Source.Logic.State.Battlefield;
using Source.Logic.State.LineItems;
using Source.Logic.State.LineItems.Units;
using Source.Utility;

namespace Source.Logic.Events
{
    public abstract class EventCommand
    {
        public enum EventStatus
        {
            Created,
            Started,
            Success,
            PartiallyFailed,
            Failed,
            Canceled
        }

        public EventStatus Status => status;
        
        protected EventTracker eventTracker;
        protected EventStatus status;
        
        private readonly StringBuilder logBuilder = new();
        protected string ID => id;
        private string id;

        private int parentCount = 0;

        protected EventCommand(EventTracker eventTracker)
        {
            this.id = CreateID();
            this.eventTracker = eventTracker;
            this.status = EventStatus.Created;
        }

        public virtual bool CanPerform() { return true; }
        public abstract UniTask Apply(CancellationToken cancellationToken);

        protected UniTask ApplyChildEventWithLog(EventCommand eventCommand, CancellationToken cancellationToken)
        {
            eventCommand.parentCount = parentCount + 1;
            var task = eventTracker.AddEvent(eventCommand, true, cancellationToken);
            logBuilder.AppendLine(eventCommand.GetLog());
            return task;
        }

        protected void AddLog(string log)
        {
            for (var i = 0; i < parentCount; i++)
            {
                logBuilder.Append("\t");
            }

            logBuilder.Append(ID).Append(" ").AppendLine(log);
        }

        public string GetLog()
        {
            return logBuilder.ToString();
        }

        protected void UpdateMultiStatus(int fails, int total)
        {
            if (fails == 0)
                status = EventStatus.Success;
            else if (fails == total)
                status = EventStatus.Failed;
            else
                status = EventStatus.PartiallyFailed;
        }
        
        
        protected bool TryGetUnitAtSlot(LineStorage<BattlefieldItem> battlefieldStorage, int slot, out BattlefieldItem item, out UnitMemory unit){
            var failedLog = $"Failed to get unit at slot {slot} in {battlefieldStorage}: ";

            if(!TryGetBattlefieldItemAtSlot(battlefieldStorage, slot, out item))
            {
                AddLog(failedLog + "battlefield item unit is on does not exit (null)");
                unit = null;
                return false;
            }

            if (item.Unit == null)
            {
                AddLog(failedLog + "unit does not exist (null)");
                unit = null;
                return false;
            }

            unit = item.Unit;
            return true;
        }
        
        protected bool TryGetBuildingAtSlot(LineStorage<BattlefieldItem> battlefieldStorage, int slot, out BattlefieldItem item, out BuildingMemory building){
            var failedLog = $"Failed to get building at slot {slot} in {battlefieldStorage}: ";

            if(!TryGetBattlefieldItemAtSlot(battlefieldStorage, slot, out item))
            {
                AddLog(failedLog + "battlefield item building is on does not exit (null)");
                building = null;
                return false;
            }

            if (item.Building == null)
            {
                AddLog(failedLog + "building does not exist (null)");
                building = null;
                return false;
            }

            building = item.Building;
            return true;
        }

        
        protected bool TryGetBattlefieldItemAtSlot(LineStorage<BattlefieldItem> battlefieldStorage, int slot, out BattlefieldItem item)
        {
            var failedLog = $"Failed to get battlefield item at slot {slot} in {battlefieldStorage}: ";
            
            if (!battlefieldStorage.Items.InBounds(slot))
            {
                AddLog(failedLog + "slot not in bounds");
                item = null;
                return false;
            }

            item = battlefieldStorage.Items[slot];
            return true;
        }

        protected string CreateID()
        {
            return "[" + Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 4) + "]";
        }
        
        public static async UniTask DOVirtualAsync(float from, float to, float duration, TweenCallback<float> onUpdate, CancellationToken cancellationToken)
        {
            var tcs = new UniTaskCompletionSource();
            var tween = DOVirtual.Float(from, to, duration, onUpdate)
                .OnComplete(() => tcs.TrySetResult());

            using (cancellationToken.Register(() => { 
                       tween.Kill();
                       tcs.TrySetCanceled(); 
                   }))
            {
                await tcs.Task;
            }
        }
    }
}
