using System.Collections.Generic;
using Source.Logic.Data;
using Source.Utility;
using UnityEngine;

namespace Source.Logic.Events
{
    public class CreateUnitsEventCommand : EventCommand 
    {
        private ItemStorage<BattlefieldDataItem> storage;
        private List<int> slots;
        private UnitData unitData;

        public CreateUnitsEventCommand(
            ItemStorage<BattlefieldDataItem> storage,
            List<int> slots,
            UnitData unitData
        )
        {
            this.storage = storage;
            this.slots = slots;
            this.unitData = unitData;
        }

        public override bool Perform()
        {
            var success = true;
            foreach (var slot in slots)
            {
                if (storage.GetItemSlotReference(slot, out var storageItem))
                {
                    storageItem.Item ??= new BattlefieldDataItem();
                    storageItem.Item.UnitData = unitData;
                }
                else
                {
                    success = false;
                }
            }
            
            
            Debug.Log($"{(success ? "Created" : "Failed to create")} units of type {unitData.Name} in slots {slots.ToItemString()} of {storage}");

            return success;
        }
    }
}