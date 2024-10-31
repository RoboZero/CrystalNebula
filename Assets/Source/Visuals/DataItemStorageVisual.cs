using System;
using System.Collections.Generic;
using Source.Interactions;
using Source.Logic;
using Source.Logic.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Visuals
{
    public class DataItemStorageVisual : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private LineNumberVisual lineNumberVisualPrefab;
        [SerializeField] private DataItemVisual dataItemVisualPrefab;
        [SerializeField] private LayoutGroup lineNumberLayoutGroup;
        [SerializeField] private LayoutGroup dataItemLayoutGroup;
        
        [Header("Settings")]
        [SerializeField] private int itemStorageSize;

        public ItemStorage<DataItem> ItemStorage => itemStorage;
        public List<int> InteractedVisualIndices => interactedVisualIndices;
        
        private ItemStorage<DataItem> itemStorage = new();
        private List<int> interactedVisualIndices = new();
        private List<DataItemRecordVisual> trackedRecords = new();

        private void Awake()
        {
            lineNumberVisualPrefab.gameObject.SetActive(false);
            dataItemVisualPrefab.gameObject.SetActive(false);
        }
        
        private void Update()
        {
            itemStorage.Resize(itemStorageSize);
            
            while (itemStorage.Capacity > trackedRecords.Count)
            {
                AddRecord(trackedRecords);
            }

            interactedVisualIndices.Clear();
            for (var i = 0; i < trackedRecords.Count; i++)
            {
                itemStorage.GetItemSlotReference(i, out var itemSlot);
                UpdateRecordVisual(trackedRecords[i], itemSlot);
                UpdateVisualIndices(i, trackedRecords[i]);
            }
        }

        private void OnDestroy()
        {
            foreach (var record in trackedRecords)
            {
                DestroyRecord(record);
            }
        }

        private void AddRecord(in List<DataItemRecordVisual> records)
        {
            var dataItemVisual = Instantiate(dataItemVisualPrefab, dataItemLayoutGroup.transform);
            var lineNumberVisual = Instantiate(lineNumberVisualPrefab, lineNumberLayoutGroup.transform);
            
            var record = new DataItemRecordVisual()
            {
                dataItemVisual = dataItemVisual,
                LineNumberVisual = lineNumberVisual
            };
            
            records.Add(record);
        }

        private void UpdateRecordVisual(in DataItemRecordVisual recordVisual, in ItemStorage<DataItem>.ItemSlot slot)
        {
            if (slot.IsActive)
            {
                recordVisual.dataItemVisual.SetDataItem(slot.Item);
                recordVisual.LineNumberVisual.Value = slot.LineNumber;
            }
            else
            {
                recordVisual.dataItemVisual.SetDataItem(null);
                recordVisual.dataItemVisual.ResetState();
            }
            
            recordVisual.dataItemVisual.gameObject.SetActive(slot.IsActive);
            recordVisual.LineNumberVisual.gameObject.SetActive(slot.IsActive);
        }

        private void UpdateVisualIndices(int index, in DataItemRecordVisual recordVisual)
        {
            if (recordVisual.dataItemVisual.CurrentVisualState == InteractVisualState.Selected)
            {
                interactedVisualIndices.Add(index);
            }
        }
        
        private void DestroyRecord(DataItemRecordVisual record)
        {
            Destroy(record.LineNumberVisual.gameObject);
            Destroy(record.dataItemVisual.gameObject);
        }

        [Serializable]
        private struct DataItemRecordVisual
        {
            public LineNumberVisual LineNumberVisual;
            public DataItemVisual dataItemVisual;
        }
    }
}
