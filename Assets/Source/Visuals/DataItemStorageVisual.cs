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
        [SerializeField] private DataItemStorage dataItemStorage;
        [SerializeField] private LineNumberVisual lineNumberVisualPrefab;
        [SerializeField] private DataItemVisual dataItemVisualPrefab;
        [SerializeField] private LayoutGroup lineNumberLayoutGroup;
        [SerializeField] private LayoutGroup dataItemLayoutGroup;

        private readonly List<DataItemRecordVisual> trackedRecords = new();

        private void Awake()
        {
            lineNumberVisualPrefab.gameObject.SetActive(false);
            dataItemVisualPrefab.gameObject.SetActive(false);
        }
        
        private void Update()
        {
            while (dataItemStorage.Capacity > trackedRecords.Count)
            {
                AddRecord(trackedRecords);
            }

            for (var i = 0; i < trackedRecords.Count; i++)
            {
                if(dataItemStorage.GetRecord(i, out var recordData))
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

        private void SetRecordData(DataItemRecordVisual recordVisual, in DataItemStorage.DataItemRecord recordData)
        {
            if (recordData.IsActive)
            {
                recordVisual.DataItemVisual.SetDataItem(recordData.DataItem);
                recordVisual.LineNumberVisual.Value = recordData.LineNumber;
            }
            else
            {
                recordVisual.DataItemVisual.SetDataItem(new DataItem());
                recordVisual.DataItemVisual.ResetState();
            }
            
            recordVisual.DataItemVisual.gameObject.SetActive(recordData.IsActive);
            recordVisual.LineNumberVisual.gameObject.SetActive(recordData.IsActive);
        }
        
        private void DestroyRecord(DataItemRecordVisual record)
        {
            Destroy(record.LineNumberVisual.gameObject);
            Destroy(record.DataItemVisual.gameObject);
        }

        private struct DataItemRecordVisual
        {
            public LineNumberVisual LineNumberVisual;
            public DataItemVisual DataItemVisual;
        }
    }
}
