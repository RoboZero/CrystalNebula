using System;
using System.Collections.Generic;
using System.Text;
using Source.Logic.State;
using Source.Logic.State.Battlefield;
using Source.Logic.State.LineItems;
using Source.Utility;
using UnityEngine;

namespace Source.Logic.Events
{
    public class CreateUnitsEventCommand : EventCommand 
    {
        private LineStorage<BattlefieldItem> battlefieldStorage;
        private List<int> slots;
        private Unit unit;
        private bool forceIfOccupied;

        public CreateUnitsEventCommand(
            LineStorage<BattlefieldItem> battlefieldStorage,
            List<int> slots,
            Unit unit,
            bool forceIfOccupied
        )
        {
            this.battlefieldStorage = battlefieldStorage;
            this.slots = slots;
            this.unit = unit;
            this.forceIfOccupied = forceIfOccupied;
        }

        public override bool Perform()
        {
            AddLog($"{ID} Creating units of type {unit.Definition} in slots {slots.ToItemString()} of {battlefieldStorage}");

            var success = true;
            foreach (var slot in slots)
            {
                if (slot < 0 || slot >= battlefieldStorage.Items.Count)
                {
                    AddLog($"Failed to create unit of type {unit.Definition} in slot {slot} of {battlefieldStorage}: slot {slot} out of battlefield index bounds {battlefieldStorage.Items.Count}");
                    success = false;
                    continue;
                }
                
                battlefieldStorage.Items[slot] ??= new BattlefieldItem();
                if (!forceIfOccupied && battlefieldStorage.Items[slot].Unit != null)
                {
                    AddLog($"Failed to create unit of type {unit.Definition} in slot {slot} of {battlefieldStorage}: slot is occupied by {battlefieldStorage.Items[slot].Unit.Definition}\n");
                    success = false;
                    continue;
                }
                
                battlefieldStorage.Items[slot].Unit = unit;
                AddLog($"Successfully created unit of type {unit.Definition} in slot {slot} of {battlefieldStorage}");
            }
            
            return success;
        }
    }
}