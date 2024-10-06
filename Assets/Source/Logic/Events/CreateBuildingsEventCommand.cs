using System.Collections.Generic;
using Source.Logic.Data;
using Source.Utility;
using UnityEngine;

namespace Source.Logic.Events
{
    public class CreateBuildingsEventCommand : EventCommand 
    {
        private ItemStorage<BattlefieldDataItem> storage;
        private List<int> slots;
        private BuildingData buildingData;

        public CreateBuildingsEventCommand(
            ItemStorage<BattlefieldDataItem> storage,
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
            Debug.Log($"Creating buildings of type {buildingData.Name} in slots {slots.ToItemString()} of {storage}");

            var success = true;
            foreach (var slot in slots)
            {
                if (storage.GetItemSlotReference(slot, out var storageItem))
                {
                    storageItem.Item ??= new BattlefieldDataItem();
                    storageItem.Item.BuildingData = buildingData;
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