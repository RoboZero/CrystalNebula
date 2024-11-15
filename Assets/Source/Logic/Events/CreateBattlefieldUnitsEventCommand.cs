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
using UnityEngine;

namespace Source.Logic.Events
{
    public class CreateBattlefieldUnitsEventCommand : EventCommand 
    {
        private LineStorage<BattlefieldItem> battlefieldStorage;
        private List<int> slots;
        private UnitMemory unit;
        private bool forceIfOccupied;

        public CreateBattlefieldUnitsEventCommand(
            EventTracker eventTracker,
            LineStorage<BattlefieldItem> battlefieldStorage,
            List<int> slots,
            UnitMemory unit,
            bool forceIfOccupied
        ) : base(eventTracker)
        {
            this.battlefieldStorage = battlefieldStorage;
            this.slots = slots;
            this.unit = unit;
            this.forceIfOccupied = forceIfOccupied;
        }

        public override async UniTask Apply(CancellationToken cancellationToken)
        {
            status = EventStatus.Started;
            AddLog($"{ID} Creating units of type {unit.Definition} in slots {slots.ToItemString()} of {battlefieldStorage}");

            var fails = 0;
            foreach (var slot in slots)
            {
                if (slot < 0 || slot >= battlefieldStorage.Items.Count)
                {
                    AddLog($"Failed to create unit of type {unit.Definition} in slot {slot} of {battlefieldStorage}: slot {slot} out of battlefield index bounds {battlefieldStorage.Items.Count}");
                    fails++;
                    continue;
                }
                
                battlefieldStorage.Items[slot] ??= new BattlefieldItem();
                if (!forceIfOccupied && battlefieldStorage.Items[slot].Unit != null)
                {
                    AddLog($"Failed to create unit of type {unit.Definition} in slot {slot} of {battlefieldStorage}: slot is occupied by {battlefieldStorage.Items[slot].Unit.Definition}\n");
                    fails++;
                    continue;
                }
                
                battlefieldStorage.Items[slot].Unit = unit;
                AddLog($"Successfully created unit of type {unit.Definition} in slot {slot} of {battlefieldStorage}");
            }
            
            UpdateMultiStatus(fails, slots.Count);
            AddLog($"Multi create units Status: {status.ToString()}");
        }
    }
}