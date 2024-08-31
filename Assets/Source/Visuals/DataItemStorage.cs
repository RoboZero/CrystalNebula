using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Visuals
{
    public class DataItemStorage : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private DataItem dataItemPrefab;
        [SerializeField] private LayoutGroup dataItemLayoutGroup;
        
        [Header("Settings")]
        [SerializeField] private int dataItemSize;

        private List<DataItem> items = new();

        // Update is called once per frame
        void Update()
        {
            Resize(dataItemSize);
        }

        public void Resize(int newSize)
        {
            var itemDelta = newSize - items.Count;
            if (itemDelta > 0)
            {
                for (var i = 0; i < itemDelta; i++)
                {
                    var item = Instantiate(dataItemPrefab, dataItemLayoutGroup.transform);
                    items.Add(item);
                }
            } 
            else if (itemDelta < 0)
            {
                var lastRemovalIndex = Math.Max(items.Count - 1 + itemDelta, 0);
                for (var i = items.Count - 1; i >= lastRemovalIndex; i--)
                {
                    Destroy(items[i].gameObject);
                    items.RemoveAt(i);
                }
            }
        }
    }
}
