using System.Collections.Generic;
using Source.Logic.Data;
using Source.Utility;
using UnityEngine;

namespace Source.Logic.Events
{
    public class CreateUnitsEventCommand : EventCommand 
    {
        private ItemStorage<BattlefieldItem> storage;
        private List<int> slots;
        private Unit unit;

        public CreateUnitsEventCommand(
            ItemStorage<BattlefieldItem> storage,
            List<int> slots,
            Unit unit
        )
        {
            this.storage = storage;
            this.slots = slots;
            this.unit = unit;
        }

        public override bool Perform()
        {
            var success = true;
            foreach (var slot in slots)
            {
                if (storage.GetItemSlotReference(slot, out var storageItem))
                {
                    storageItem.Item ??= new BattlefieldItem();
                    storageItem.Item.Unit = unit;
                }
                else
                {
                    success = false;
                }
            }
            
            Debug.Log($"{(success ? "Successfully created" : "Failed to create")} units of type {unit.Definition} in slots {slots.ToItemString()} of {storage}");

            return success;
        }
    }
}