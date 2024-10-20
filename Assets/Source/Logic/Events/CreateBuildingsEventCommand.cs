using System.Collections.Generic;
using Source.Logic.Data;
using Source.Utility;
using UnityEngine;

namespace Source.Logic.Events
{
    public class CreateBuildingsEventCommand : EventCommand 
    {
        private ItemStorage<BattlefieldItem> storage;
        private List<int> slots;
        private Building building;

        public CreateBuildingsEventCommand(
            ItemStorage<BattlefieldItem> storage,
            List<int> slots,
            Building building
        )
        {
            this.storage = storage;
            this.slots = slots;
            this.building = building;
        }

        public override bool Perform()
        {
            Debug.Log($"Creating buildings of type {building.Definition} in slots {slots.ToItemString()} of {storage}");

            var success = true;
            foreach (var slot in slots)
            {
                if (storage.GetItemSlotReference(slot, out var storageItem))
                {
                    storageItem.Item ??= new BattlefieldItem();
                    storageItem.Item.Building = building;
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