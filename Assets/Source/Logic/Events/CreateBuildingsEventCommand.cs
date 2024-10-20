using System.Collections.Generic;
using Source.Logic.Data;
using Source.Utility;
using UnityEngine;

namespace Source.Logic.Events
{
    public class CreateBuildingsEventCommand : EventCommand 
    {
        private BattlefieldStorage battlefieldStorage;
        private List<int> slots;
        private Building building;

        public CreateBuildingsEventCommand(
            BattlefieldStorage battlefieldStorage,
            List<int> slots,
            Building building
        )
        {
            this.battlefieldStorage = battlefieldStorage;
            this.slots = slots;
            this.building = building;
        }

        public override bool Perform()
        {
            Debug.Log($"Creating buildings of type {building.Definition} in slots {slots.ToItemString()} of {battlefieldStorage}");

            var success = true;
            foreach (var slot in slots)
            {
                if (slot >= 0 && slot < battlefieldStorage.Items.Count)
                {
                    battlefieldStorage.Items[slot].Building = building;
                } 
                else
                {
                    success = false;
                }
            }
            
            Debug.Log($"{(success ? "Successfully created" : "Failed to create")} buildings of type {building.Definition} in slots {slots.ToItemString()} of {battlefieldStorage}");
            return success;
        }
    }
}