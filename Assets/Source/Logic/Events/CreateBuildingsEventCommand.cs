using System.Collections.Generic;
using Source.Logic.Data;
using Source.Utility;
using UnityEngine;

namespace Source.Logic.Events
{
    public class CreateBuildingsEventCommand : EventCommand 
    {
        private ItemStorage<BattlefieldItemData> storage;
        private List<int> slots;
        private BuildingData buildingData;

        public CreateBuildingsEventCommand(
            ItemStorage<BattlefieldItemData> storage,
            List<int> slots,
            BuildingData buildingData
        )
        {
            this.storage = storage;
            this.slots = slots;
            this.buildingData = buildingData;
        }

        public override bool Perform()
        {
            Debug.Log($"Creating buildings of type {buildingData.Definition} in slots {slots.ToItemString()} of {storage}");

            var success = true;
            foreach (var slot in slots)
            {
                if (storage.GetItemSlotReference(slot, out var storageItem))
                {
                    storageItem.Item ??= new BattlefieldItemData();
                    storageItem.Item.Building = buildingData;
                }
                else
                {
                    success = false;
                }
            }

            return success;
        }
    }
}