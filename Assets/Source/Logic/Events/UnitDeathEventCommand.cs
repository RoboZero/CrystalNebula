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
            var logBuilder = new StringBuilder();
            logBuilder.AppendLine($"{ID} Unit at slot {deadUnitSlot} has died in {battlefieldStorage}");

            if (!battlefieldStorage.TryGetUnitAtSlot(deadUnitSlot, logBuilder, out _, out _))
            {
                logBuilder.AppendLine($"Failed have unit in slot {deadUnitSlot} die: could not get unit in slot");
                Debug.Log(logBuilder);
                return false;
            }

            battlefieldStorage.Items[deadUnitSlot].Unit = null;
            logBuilder.AppendLine($"Successfully killed unit by removing unit in slot {deadUnitSlot} (now null)");
            Debug.Log(logBuilder);
            return true;
        }
    }
}