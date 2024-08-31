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
        private int Capacity => items.Count;
        
        private List<LineNumber> lineNumbers = new();
        private List<DataItem> items = new();

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
                items[i].gameObject.SetActive(i < newSize);
                lineNumbers[i].gameObject.SetActive(i < newSize);
            }
        }
        
        public void SetCapacity(int newCapacity)
        {
            var itemDelta = newCapacity - items.Count;
            switch (itemDelta)
            {
                case > 0:
                {
                    var lastCreationIndex = items.Count + itemDelta;
                    for (var i = items.Count; i < lastCreationIndex; i++)
                    {
                        var item = Instantiate(dataItemPrefab, dataItemLayoutGroup.transform);
                        items.Add(item);

                        var lineNumber = Instantiate(lineNumberPrefab, lineNumberLayoutGroup.transform);
                        lineNumber.Value = lineNumbers.Count;
                        lineNumbers.Add(lineNumber);
                    }

                    break;
                }
                case < 0:
                {
                    var lastRemovalIndex = Math.Max(items.Count - 1 + itemDelta, 0);
                    for (var i = items.Count - 1; i >= lastRemovalIndex; i--)
                    {
                        Destroy(items[i].gameObject);
                        items.RemoveAt(i);
                    
                        Destroy(lineNumbers[i].gameObject);
                        lineNumbers.RemoveAt(i);
                    }

                    break;
                }
            }
        }
    }
}
