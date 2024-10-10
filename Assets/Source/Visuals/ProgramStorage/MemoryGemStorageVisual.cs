using System;
using System.Collections.Generic;
using Source.Interactions;
using Source.Logic;
using Source.Logic.Data;
using Source.Serialization;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Visuals.ProgramStorage
{
    public class MemoryGemStorageVisual : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private MemoryStorage trackedMemoryStorage;
        [SerializeField] private MemoryGemItemVisual memoryGemItemVisualPrefab;
        [SerializeField] private LayoutGroup dataItemLayoutGroup;
        
        [SerializeField] private GameResources gameResources;

        private List<int> interactedVisualIndices = new();
        private List<MemoryGemItemVisual> trackedRecords = new();

        private void Awake()
        {
            memoryGemItemVisualPrefab.gameObject.SetActive(false);
        }

        private void Update()
        {
            // TODO: Visual should not update memory storage, could be updated multiple times per frame. 
            trackedMemoryStorage.Tick();
            
            while (trackedMemoryStorage.ItemStorage.Capacity > trackedRecords.Count)
            {
                AddRecord(trackedRecords);
            }

            interactedVisualIndices.Clear();
            for (var i = 0; i < trackedRecords.Count; i++)
            {
                trackedMemoryStorage.ItemStorage.GetItemSlotReference(i, out var itemSlot);
                UpdateRecordVisual(trackedRecords[i], itemSlot);
                UpdateVisualIndices(i, trackedRecords[i]);
            }
        }
        
        private void AddRecord(in List<MemoryGemItemVisual> records)
        {
            var dataItemVisual = Instantiate(memoryGemItemVisualPrefab, dataItemLayoutGroup.transform);
            records.Add(dataItemVisual);
        }
        
        private void UpdateRecordVisual(in MemoryGemItemVisual recordVisual, in ItemStorage<MemoryItemData>.ItemSlot slot)
        {
            if (slot.IsActive)
            {
                recordVisual.SetGameResources(gameResources);
                recordVisual.SetDataItem(slot.Item);
            }
            else
            {
                recordVisual.SetDataItem(null);
                recordVisual.ResetState();
            }
            
            recordVisual.gameObject.SetActive(slot.IsActive);
        }

        private void UpdateVisualIndices(int index, in MemoryGemItemVisual recordVisual)
        {
            if (recordVisual.CurrentVisualState == InteractVisualState.Selected)
            {
                interactedVisualIndices.Add(index);
            }
        }
        
        private void DestroyRecord(MemoryGemItemVisual record)
        {
            Destroy(record.gameObject);
        }
    }
}
