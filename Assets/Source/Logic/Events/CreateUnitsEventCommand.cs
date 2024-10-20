using System.Collections.Generic;
using Source.Logic.Data;
using Source.Utility;
using UnityEngine;

namespace Source.Logic.Events
{
    public class CreateUnitsEventCommand : EventCommand 
    {
        private BattlefieldStorage battlefieldStorage;
        private List<int> slots;
        private Unit unit;

        public CreateUnitsEventCommand(
            BattlefieldStorage battlefieldStorage,
            List<int> slots,
            Unit unit
        )
        {
            this.battlefieldStorage = battlefieldStorage;
            this.slots = slots;
            this.unit = unit;
        }

        public override bool Perform()
        {
            var success = true;
            foreach (var slot in slots)
            {
                if (slot >= 0 && slot < battlefieldStorage.Items.Count)
                {
                    battlefieldStorage.Items[slot].Unit = unit;
                }
                else
                {
                    success = false;
                }
            }
            
            Debug.Log($"{(success ? "Successfully created" : "Failed to create")} units of type {unit.Definition} in slots {slots.ToItemString()} of {battlefieldStorage}");
            return success;
        }
    }
}