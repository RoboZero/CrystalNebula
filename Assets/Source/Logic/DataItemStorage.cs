using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Logic
{
    public class DataItemStorage : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int dataItemSize;
        
        public int Size { get; private set; }
        public int Capacity => allRecords.Count;

        private List<DataItemRecord> allRecords = new();

        private void Update()
        { 
            Resize(dataItemSize);
        }

        public bool GetRecord(int i, out DataItemRecord record)
        {
            if (i >= 0 && i < Size)
            {
                record = allRecords[i];
                return true;
            }

            record = new DataItemRecord();
            return false;
        }
        
        public void Resize(int newSize)
        {
            if (newSize > Capacity)
            {
                SetCapacity(newSize);
            }

            Size = newSize;
            for (var i = 0; i < Capacity; i++)
            {
                allRecords[i] = allRecords[i] with { IsActive = i < Size };
            }
        }
        
        private void SetCapacity(int newCapacity)
        {
            var itemDelta = newCapacity - allRecords.Count;
            switch (itemDelta)
            {
                case > 0:
                {
                    var lastCreationIndex = allRecords.Count + itemDelta;
                    for (var i = allRecords.Count; i < lastCreationIndex; i++)
                    {
                        var record = new DataItemRecord()
                        {
                            LineNumber = i,
                            DataItem = new DataItem(),
                            IsActive = false
                        };
                        allRecords.Add(record);
                    }

                    break;
                }
                case < 0:
                {
                    var lastRemovalIndex = Math.Max(allRecords.Count - 1 + itemDelta, 0);
                    for (var i = allRecords.Count - 1; i >= lastRemovalIndex; i--)
                    {
                        allRecords.RemoveAt(i);
                    }

                    break;
                }
            }
        }
        
        [Serializable]
        public record DataItemRecord
        {
            public int LineNumber;
            public DataItem DataItem;
            public bool IsActive;
        }
    }
}