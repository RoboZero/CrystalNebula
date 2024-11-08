using Cysharp.Threading.Tasks;
using Source.Logic.State;
using Source.Logic.State.Battlefield;
using Source.Logic.State.LineItems;

namespace Source.Logic.Events
{
    public class SwitchUnitEventCommand : EventCommand
    {
        private EventTracker eventTracker;
        private LineStorage<BattlefieldItem> battlefieldStorage;
        private int fromSlot;
        private int toSlot;

        public SwitchUnitEventCommand(
            EventTracker eventTracker,
            LineStorage<BattlefieldItem> battlefieldStorage,
            int fromSlot,
            int toSlot
        )
        {
            this.eventTracker = eventTracker;
            this.battlefieldStorage = battlefieldStorage;
            this.fromSlot = fromSlot;
            this.toSlot = toSlot;
        }
        
        public override async UniTask<bool> Perform()
        {
            AddLog($"Switching units from {fromSlot} to {toSlot} in {battlefieldStorage}");

            if (!TryGetUnitAtSlot(battlefieldStorage, fromSlot, out var itemA, out _))
            {
                AddLog($"Failed to switch units: no unit in from slot {fromSlot}");
                return false;
            }

            if (!TryGetUnitAtSlot(battlefieldStorage, toSlot, out var itemB, out _))
            {
                AddLog($"Failed to switch units: no unit in to slot {toSlot}");
                return false;
            }
            
            AddLog($"Switching units A {itemA.Unit} and B {itemB.Unit}.");

            (itemB.Unit, itemA.Unit) = (itemA.Unit, itemB.Unit);
            AddLog($"Successfully switch units.");
            return true;
        }
    }
}