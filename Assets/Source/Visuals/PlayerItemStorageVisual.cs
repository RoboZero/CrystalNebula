using System;
using System.Collections.Generic;
using Source.Input;
using Source.Logic;
using Source.Serialization;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Visuals
{
    public class PlayerItemStorageVisual : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private GameStateLoader gameStateLoader;
        [SerializeField] private PlayerInteractions playerInteractions;
        [SerializeField] private InputReaderSO inputReader;
        [SerializeField] private RectTransform backgroundRectTransform;
        
        [SerializeField] private LineNumberVisual lineNumberVisualPrefab;
        [SerializeField] private MemoryItemVisual memoryItemVisualPrefab;
        [SerializeField] private LayoutGroup lineNumberLayoutGroup;
        [SerializeField] private LayoutGroup dataItemLayoutGroup;

        [Header("Settings")]
        [SerializeField] private int itemStorageCapacity = 3;

        public ItemStorage<DataItem> ItemStorage => itemStorage;
        public List<int> InteractedVisualIndices => interactedVisualIndices;
        private List<DataItemRecordVisual> trackedRecords = new();


        private ItemStorage<DataItem> itemStorage = new();
        private List<int> interactedVisualIndices = new();

        private void OnEnable()
        {
            inputReader.PointerPositionEvent += OnPointerPosition;
        }

        private void OnDisable()
        {
            inputReader.PointerPositionEvent -= OnPointerPosition;
        }

        private void OnPointerPosition(Vector2 position, bool isMouse)
        {
            if(RectTransformUtility.ScreenPointToWorldPointInRectangle(backgroundRectTransform, position, Camera.main, out var worldPoint))
                transform.position = new Vector3(worldPoint.x, worldPoint.y, 0);
        }

        // Update is called once per frame
        void Update()
        {
            itemStorage.Resize(itemStorageCapacity); 
            
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

            /*float paddingSize = 0f;
            Vector2 backgroundSize = new Vector2(
                storedItemsLayoutGroup.preferredWidth + paddingSize * 2,
                storedItemsLayoutGroup.preferredHeight + paddingSize * 2
            );
            backgroundRectTransform.sizeDelta = backgroundSize;
            */
        }
        
        private void AddRecord(in List<DataItemRecordVisual> records)
        {
            var dataItemVisual = Instantiate(memoryItemVisualPrefab, dataItemLayoutGroup.transform);
            var lineNumberVisual = Instantiate(lineNumberVisualPrefab, lineNumberLayoutGroup.transform);
            
            var record = new DataItemRecordVisual()
            {
                MemoryItemVisual = dataItemVisual,
                LineNumberVisual = lineNumberVisual
            };
            
            records.Add(record);
        }
        
        private void UpdateRecordVisual(in DataItemRecordVisual recordVisual, in ItemStorage<DataItem>.ItemSlot slot)
        {
            if (slot.IsActive)
            {
                recordVisual.MemoryItemVisual.SetDataItem(slot.Item);
                recordVisual.LineNumberVisual.Value = slot.LineNumber;
            }
            else
            {
                recordVisual.MemoryItemVisual.SetDataItem(null);
                recordVisual.MemoryItemVisual.ResetState();
            }
            
            recordVisual.MemoryItemVisual.gameObject.SetActive(slot.IsActive);
            recordVisual.LineNumberVisual.gameObject.SetActive(slot.IsActive);
        }

        private void UpdateVisualIndices(int index, in DataItemRecordVisual recordVisual)
        {
            interactedVisualIndices.Add(index);
        }
        
        [Serializable]
        private struct DataItemRecordVisual
        {
            public LineNumberVisual LineNumberVisual;
            public MemoryItemVisual MemoryItemVisual;
        }
    }
}
