using Source.Logic.Data;

namespace Source.Logic.Events
{
    public class SwitchUnitEventCommand : EventCommand
    {
        private EventTracker eventTracker;
        private BattlefieldStorage battlefieldStorage;
        private int fromSlot;
        private int toSlot;

        public SwitchUnitEventCommand(
            EventTracker eventTracker,
            BattlefieldStorage battlefieldStorage,
            int fromSlot,
            int toSlot
        )
        {
            this.eventTracker = eventTracker;
            this.battlefieldStorage = battlefieldStorage;
            this.fromSlot = fromSlot;
            this.toSlot = toSlot;
        }
        
        public override bool Perform()
        {
            AddLog($"Switching units from {fromSlot} to {toSlot} in {battlefieldStorage}");

            if (!TryGetUnitAtSlot(battlefieldStorage, fromSlot, out var itemA, out var unitA))
            {
                AddLog($"Failed to switch units: no unit in from slot {fromSlot}");
                return false;
            }

            if (!TryGetUnitAtSlot(battlefieldStorage, toSlot, out var itemB, out var unitB))
            {
                AddLog($"Failed to switch units: no unit in to slot {toSlot}");
                return false;
            }

            (itemB.Unit, itemA.Unit) = (itemA.Unit, itemB.Unit);

            return true;
        }
    }
}