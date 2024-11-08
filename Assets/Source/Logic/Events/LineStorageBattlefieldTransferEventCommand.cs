using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Logic.State.Battlefield;
using Source.Logic.State.LineItems;
using Source.Logic.State.LineItems.Units;
using Source.Serialization.Data;
using Source.Utility;

namespace Source.Logic.Events
{
    public class LineStorageBattlefieldTransferEventCommand : EventCommand
    {
        public enum TransferredItem
        {
            Unit,
            Building
        }
        
        private LineStorage<MemoryItem> memoryStorage;
        private int memorySlot;
        private LineStorage<BattlefieldItem> battlefieldStorage;
        private int battlefieldSlot;
        private TransferredItem transferredItem;
        private TransferEventOverrides transferEventOverrides;
        
        public LineStorageBattlefieldTransferEventCommand(
            LineStorage<MemoryItem> memoryStorage,
            int memorySlot,
            LineStorage<BattlefieldItem> battlefieldStorage,
            int battlefieldSlot,
            TransferredItem transferredItem,
            TransferEventOverrides transferEventOverrides
        )
        {
            this.battlefieldStorage = battlefieldStorage;
            this.battlefieldSlot = battlefieldSlot;
            this.memoryStorage = memoryStorage;
            this.memorySlot = memorySlot;
            this.transferredItem = transferredItem;
            this.transferEventOverrides = transferEventOverrides;
        }
        
        public override async UniTask<bool> Apply(CancellationToken cancellationToken)
        {
            AddLog($"{GetType().Name} Starting line storage transfer from slot {battlefieldStorage}:{battlefieldSlot} to slot {memoryStorage}:{memorySlot}");
            var failurePrefix = $"Unable to transfer from {battlefieldStorage}:{battlefieldSlot} to {memoryStorage}:{memorySlot}: ";

            if (!battlefieldStorage.Items.InBounds(battlefieldSlot))
            {
                AddLog(failurePrefix + $"{nameof(battlefieldSlot)} {battlefieldSlot} is not in {nameof(battlefieldStorage)} bounds length {battlefieldStorage.Items.Count}");
                return false;
            }
            
            if (!memoryStorage.Items.InBounds(memorySlot))
            {
                AddLog(failurePrefix + $"{nameof(memorySlot)} {memorySlot} is not in {nameof(memoryStorage)} bounds length {memoryStorage.Items.Count}");
                return false;
            }
            
            if (transferEventOverrides is { CanSwitch: false } && memoryStorage.Items[memorySlot] != null)
            {
                AddLog(failurePrefix + $"cannot switch unit in {nameof(battlefieldSlot)} {battlefieldSlot} as {nameof(memorySlot)} has item in it {memoryStorage.Items[memorySlot]}");
                return false;
            }

            var memory = memoryStorage.Items[memorySlot];
            var battlefieldItem = battlefieldStorage.Items[battlefieldSlot];
            switch (transferredItem)
            {
                case TransferredItem.Unit:
                    var battlefieldUnit = battlefieldItem.Unit;
                    var unitMemory = memory as UnitMemory;
                    
                    if (memory != null && unitMemory == null)
                    {
                        AddLog(failurePrefix + $"cannot switch {TransferredItem.Unit} in {nameof(battlefieldSlot)} {battlefieldSlot} as {nameof(memorySlot)} has untransferable item in it {memory}");
                        return false;
                    }

                    (memoryStorage.Items[memorySlot], battlefieldStorage.Items[battlefieldSlot].Unit) = (battlefieldUnit, unitMemory);
                    break;
                case TransferredItem.Building:
                    var battlefieldBuilding = battlefieldItem.Building;
                    var buildingMemory = memory as BuildingMemory;
                    
                    if (memory != null && buildingMemory == null)
                    {
                        AddLog(failurePrefix + $"cannot switch {TransferredItem.Building} in {nameof(battlefieldSlot)} {battlefieldSlot} as {nameof(memorySlot)} has untransferable item in it {memory}");
                        return false;
                    }

                    (memoryStorage.Items[memorySlot], battlefieldStorage.Items[battlefieldSlot].Building) = (battlefieldBuilding, buildingMemory);

                    break;
            }

            AddLog($"Successfully transferred slot {battlefieldSlot} to slot {memorySlot}");
            return true;
        }
    }
}