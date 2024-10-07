using System;
using System.Collections.Generic;
using Source.Interactions;
using Source.Logic;
using Source.Logic.Data;
using Source.Logic.State;
using Source.Serialization;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Visuals.Battlefield
{
    //TODO: Be able to import and export a battlefield. 
    public class BattlefieldStorageVisual : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private LineNumberVisual lineNumberVisualPrefab;
        [SerializeField] private BattlefieldItemVisual memoryItemVisualPrefab;
        [SerializeField] private LayoutGroup lineNumberLayoutGroup;
        [SerializeField] private LayoutGroup dataItemLayoutGroup;
        
        [SerializeField] private GameStateLoader gameStateLoader;
        [SerializeField] private GameResources gameResources;
        
        [Header("Settings")]
        [SerializeField] private int itemStorageSize;

        public ItemStorage<BattlefieldItemData> ItemStorage => itemStorage;
        public List<int> InteractedVisualIndices => interactedVisualIndices;
        
        private ItemStorage<BattlefieldItemData> itemStorage = new();
        private List<int> interactedVisualIndices = new();
        private List<DataItemRecordVisual> trackedRecords = new();

        private void Awake()
        {
            lineNumberVisualPrefab.gameObject.SetActive(false);
            memoryItemVisualPrefab.gameObject.SetActive(false);
        }

        private void Update()
        {
            if(gameStateLoader.GameState != null)
                UpdateStorageFromState(gameStateLoader.GameState);
            
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

        private void UpdateStorageFromState(GameState gameState)
        {
            itemStorageSize = gameState.BattlefieldStorage.Length;

            foreach (var battlefieldStorageItem in gameState.BattlefieldStorage.Items)
            {
                //Debug.Log($"Battlefield Item: {battlefieldStorageItem.Location}, {battlefieldStorageItem.Unit?.Definition}, {battlefieldStorageItem.Building?.Definition}");
                itemStorage.GetItemSlotReference(battlefieldStorageItem.Location, out var itemSlot);
                itemSlot.Item ??= new BattlefieldItemData();
                itemSlot.Item.Location = battlefieldStorageItem.Location;
                itemSlot.Item.Unit = battlefieldStorageItem.Unit;
                itemSlot.Item.Building = battlefieldStorageItem.Building;
            }
        }
        
        private void AddRecord(in List<DataItemRecordVisual> records)
        {
            var dataItemVisual = Instantiate(memoryItemVisualPrefab, dataItemLayoutGroup.transform);
            var lineNumberVisual = Instantiate(lineNumberVisualPrefab, lineNumberLayoutGroup.transform);
            
            var record = new DataItemRecordVisual()
            {
                ItemVisual = dataItemVisual,
                LineNumberVisual = lineNumberVisual
            };
            
            records.Add(record);
        }

        private void UpdateRecordVisual(in DataItemRecordVisual recordVisual, in ItemStorage<BattlefieldItemData>.ItemSlot slot)
        {
            if (slot.IsActive)
            {
                recordVisual.ItemVisual.SetGameResources(gameResources);
                recordVisual.ItemVisual.SetDataItem(slot.Item);
                recordVisual.LineNumberVisual.Value = slot.LineNumber;
            }
            else
            {
                recordVisual.ItemVisual.SetDataItem(null);
                recordVisual.ItemVisual.ResetState();
            }
            
            recordVisual.ItemVisual.gameObject.SetActive(slot.IsActive);
            recordVisual.LineNumberVisual.gameObject.SetActive(slot.IsActive);
        }

        private void UpdateVisualIndices(int index, in DataItemRecordVisual recordVisual)
        {
            if (recordVisual.ItemVisual.CurrentVisualState == InteractVisualState.Selected)
            {
                interactedVisualIndices.Add(index);
            }
        }
        
        private void DestroyRecord(DataItemRecordVisual record)
        {
            Destroy(record.LineNumberVisual.gameObject);
            Destroy(record.ItemVisual.gameObject);
        }

        [Serializable]
        private struct DataItemRecordVisual
        {
            public LineNumberVisual LineNumberVisual;
            public BattlefieldItemVisual ItemVisual;
        }
    }
}
