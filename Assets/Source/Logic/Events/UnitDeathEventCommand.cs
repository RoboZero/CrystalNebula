using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Logic.State;
using Source.Logic.State.Battlefield;
using Source.Logic.State.LineItems;
using Source.Utility;
using UnityEngine;

namespace Source.Logic.Events
{
    public class UnitDeathEventCommand : EventCommand
    {
        private LineStorage<BattlefieldItem> battlefieldStorage;
        private int deadUnitSlot;

        public UnitDeathEventCommand(
            EventTracker eventTracker,
            LineStorage<BattlefieldItem> battlefieldStorage,
            int deadUnitSlot
        ) : base(eventTracker)
        {
            this.battlefieldStorage = battlefieldStorage;
            this.deadUnitSlot = deadUnitSlot;
        }
        
        public override async UniTask Apply(CancellationToken cancellationToken)
        {
            AddLog($"Unit at slot {deadUnitSlot} has died in {battlefieldStorage}");

            if (!TryGetUnitAtSlot(battlefieldStorage, deadUnitSlot, out _, out _))
            {
                AddLog($"Failed have unit in slot {deadUnitSlot} die: could not get unit in slot");
                status = EventStatus.Failed;
            }

            battlefieldStorage.Items[deadUnitSlot].Unit = null;
            AddLog($"Successfully killed unit by removing unit in slot {deadUnitSlot} (now null)");
            status = EventStatus.Success;
        }
    }
}