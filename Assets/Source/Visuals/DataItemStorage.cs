using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Visuals
{
    public class DataItemStorage : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private LineNumber lineNumberPrefab;
        [SerializeField] private DataItem dataItemPrefab;
        [SerializeField] private LayoutGroup lineNumberLayoutGroup;
        [SerializeField] private LayoutGroup dataItemLayoutGroup;
        
        [Header("Settings")]
        [SerializeField] private int dataItemSize;

        private int Size => dataItemSize;
        private int Capacity => records.Count;

        private List<DataItemUiRecord> records = new();

        private void Awake()
        {
            lineNumberPrefab.gameObject.SetActive(false);
            dataItemPrefab.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            Resize(dataItemSize);
        }

        public void Resize(int newSize)
        {
            if (newSize > Capacity)
            {
                SetCapacity(newSize);
            }

            for (var i = 0; i < Capacity; i++)
            {
                SetActiveDataItemUiRecord(records[i], i < newSize);
            }
        }
        
        public void SetCapacity(int newCapacity)
        {
            var itemDelta = newCapacity - records.Count;
            switch (itemDelta)
            {
                case > 0:
                {
                    var lastCreationIndex = records.Count + itemDelta;
                    for (var i = records.Count; i < lastCreationIndex; i++)
                    {
                        records.Add(CreateDataItemUiRecord(i));
                    }

                    break;
                }
                case < 0:
                {
                    var lastRemovalIndex = Math.Max(records.Count - 1 + itemDelta, 0);
                    for (var i = records.Count - 1; i >= lastRemovalIndex; i--)
                    {
                        DestroyDataItemUiRecord(records[i]);
                        records.RemoveAt(i);
                    }

                    break;
                }
            }
        }

        private void SetActiveDataItemUiRecord(DataItemUiRecord record, bool isActive)
        {
            if(!isActive)
                record.DataItem.ResetState();
            record.DataItem.gameObject.SetActive(isActive);
            record.LineNumber.gameObject.SetActive(isActive);
        }

        private DataItemUiRecord CreateDataItemUiRecord(int index)
        {
            var item = Instantiate(dataItemPrefab, dataItemLayoutGroup.transform);
            var lineNumber = Instantiate(lineNumberPrefab, lineNumberLayoutGroup.transform);
            lineNumber.Value = index;

            var record = new DataItemUiRecord()
            {
                DataItem = item,
                LineNumber = lineNumber
            };
            
            return record;
        }

        private void DestroyDataItemUiRecord(DataItemUiRecord record)
        {
            Destroy(record.LineNumber.gameObject);
            Destroy(record.DataItem.gameObject);
        }

        private struct DataItemUiRecord
        {
            public LineNumber LineNumber;
            public DataItem DataItem;
        }
    }
}
