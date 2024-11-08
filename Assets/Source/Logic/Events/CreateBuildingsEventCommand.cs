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
    public class CreateBuildingsEventCommand : EventCommand
    {
        private LineStorage<BattlefieldItem> battlefieldStorage;
        private List<int> slots;
        private BuildingMemory building;
        private bool forceIfOccupied;

        public CreateBuildingsEventCommand(
            LineStorage<BattlefieldItem> battlefieldStorage,
            List<int> slots,
            BuildingMemory building,
            bool forceIfOccupied
        )
        {
            this.battlefieldStorage = battlefieldStorage;
            this.slots = slots;
            this.building = building;
            this.forceIfOccupied = forceIfOccupied;
        }

        public override async UniTask<bool> Perform(CancellationToken cancellationToken)
        {
            AddLog($"{ID} Creating buildings of type {building.Definition} in slots {slots.ToItemString()} of {battlefieldStorage}");

            var success = true;
            foreach (var slot in slots)
            {
                if (slot < 0 || slot >= battlefieldStorage.Items.Count)
                {
                    AddLog($"Failed to create unit of type {building.Definition} in slot {slot} of {battlefieldStorage}: slot {slot} out of battlefield index bounds {battlefieldStorage.Items.Count}");
                    success = false;
                    continue;
                }
                
                battlefieldStorage.Items[slot] ??= new BattlefieldItem();
                if (!forceIfOccupied && battlefieldStorage.Items[slot].Unit != null)
                {
                    AddLog($"Failed to create unit of type {building.Definition} in slot {slot} of {battlefieldStorage}: slot is occupied by {battlefieldStorage.Items[slot].Building.Definition}");
                    success = false;
                    continue;
                }
                
                battlefieldStorage.Items[slot].Building = building;
                AddLog($"Successfully created building of type {building.Definition} in slot {slot} of {battlefieldStorage}");
            }
            
            return success;
        }
    }
}