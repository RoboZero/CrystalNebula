using System;
using System.Collections.Generic;
using Source.Logic;
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
        [SerializeField] private int dataItemSize;

        private ItemStorage<DataItem> itemStorage = new();
        private List<DataItemRecordVisual> trackedRecords = new();

        private void Awake()
        {
            lineNumberVisualPrefab.gameObject.SetActive(false);
            dataItemVisualPrefab.gameObject.SetActive(false);
        }
        
        private void Update()
        {
            itemStorage.Resize(dataItemSize);
            
            while (itemStorage.Capacity > trackedRecords.Count)
            {
                AddRecord(trackedRecords);
            }

            for (var i = 0; i < trackedRecords.Count; i++)
            {
                itemStorage.GetRecord(i, out var recordData);
                SetRecordData(trackedRecords[i], recordData);
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
                DataItemVisual = dataItemVisual,
                LineNumberVisual = lineNumberVisual
            };
            
            records.Add(record);
        }

        private void SetRecordData(DataItemRecordVisual recordVisual, in ItemStorage<DataItem>.ItemRecord record)
        {
            if (record.IsActive)
            {
                recordVisual.DataItemVisual.SetDataItem(record.Item);
                recordVisual.LineNumberVisual.Value = record.LineNumber;
            }
            else
            {
                recordVisual.DataItemVisual.SetDataItem(null);
                recordVisual.DataItemVisual.ResetState();
            }
            
            recordVisual.DataItemVisual.gameObject.SetActive(record.IsActive);
            recordVisual.LineNumberVisual.gameObject.SetActive(record.IsActive);
        }
        
        private void DestroyRecord(DataItemRecordVisual record)
        {
            Destroy(record.LineNumberVisual.gameObject);
            Destroy(record.DataItemVisual.gameObject);
        }

        [Serializable]
        private struct DataItemRecordVisual
        {
            public LineNumberVisual LineNumberVisual;
            public DataItemVisual DataItemVisual;
        }
    }
}
