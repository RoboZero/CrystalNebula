using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Logic.State;
using Source.Logic.State.Battlefield;
using Source.Logic.State.LineItems;
using Source.Logic.State.LineItems.Units;
using Source.Utility;

namespace Source.Logic.Events
{
    public abstract class EventCommand
    {
        private readonly StringBuilder logBuilder = new();
        protected string ID => id;
        private string id;

        private int parentCount = 0;

        protected EventCommand()
        {
            id = "[" + Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 4) + "]";
        }

        public virtual bool CanPerform() { return true; }
        public abstract UniTask<bool> Perform(CancellationToken cancellationToken);

        protected UniTask<bool> PerformChildEventWithLog(EventCommand eventCommand, CancellationToken cancellationToken)
        {
            eventCommand.parentCount = parentCount + 1;
            var result = eventCommand.Perform(cancellationToken);
            logBuilder.AppendLine(eventCommand.GetLog());
            return result;
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
    }
}
