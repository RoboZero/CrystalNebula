using System.Text;
using Source.Logic.Data;
using Source.Utility;
using UnityEngine;

namespace Source.Logic.Events
{
    public class UnitDeathEventCommand : EventCommand
    {
        private BattlefieldStorage battlefieldStorage;
        private int deadUnitSlot;

        public UnitDeathEventCommand(
            BattlefieldStorage battlefieldStorage,
            int deadUnitSlot
        )
        {
            this.battlefieldStorage = battlefieldStorage;
            this.deadUnitSlot = deadUnitSlot;
        }
        
        public override bool Perform()
        {
            AddLog($"Unit at slot {deadUnitSlot} has died in {battlefieldStorage}");

            if (!TryGetUnitAtSlot(battlefieldStorage, deadUnitSlot, out _, out _))
            {
                AddLog($"Failed have unit in slot {deadUnitSlot} die: could not get unit in slot");
                return false;
            }

            battlefieldStorage.Items[deadUnitSlot].Unit = null;
            AddLog($"Successfully killed unit by removing unit in slot {deadUnitSlot} (now null)");
            return true;
        }
    }
}