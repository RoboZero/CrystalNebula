using System;
using System.Collections.Generic;
using System.Globalization;
using Cysharp.Threading.Tasks;
using Source.Logic.Events;
using Source.Logic.State.LineItems;
using Source.Serialization;
using Source.Utility;
using TMPro;
using UnityEngine;

namespace Source.Visuals.MemoryStorage
{
    public class LineGemStorageVisual : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private TextMeshProUGUI storageNameText;
        [SerializeField] private TextMeshProUGUI transferSpeedText;
        [SerializeField] private LineStorageBehavior trackedLineGemStorageBehavior;
        [SerializeField] private LineGemItemVisual lineGemItemVisualPrefab;
        [SerializeField] private MultirowHorizontalLayoutGroup dataItemLayoutGroup;

        [SerializeField] private GameResources gameResources;

        [Header("Settings")]
        [SerializeField] private bool showEmptyGems = true;
        
        private List<LineGemItemVisual> trackedRecords = new();
        

        private void Awake()
        {
            lineGemItemVisualPrefab.gameObject.SetActive(false);
        }

        private void Update()
        {
            // TODO: Visual should not update memory storage, could be updated multiple times per frame. 
            trackedLineGemStorageBehavior.Tick();

            if (storageNameText != null)
            {
                storageNameText.text = trackedLineGemStorageBehavior.State.StorageName;
            }
            
            if (transferSpeedText != null)
            {
                var transferSpeed = trackedLineGemStorageBehavior.State.DataPerSecondTransfer.ToString(CultureInfo.InvariantCulture);
                transferSpeedText.text = transferSpeed;
            }
                
            while (trackedLineGemStorageBehavior.State.Items.Count > trackedRecords.Count)
            {
                AddRecord(trackedRecords);
            }

            for (var i = 0; i < trackedRecords.Count; i++)
            {
                if (i < trackedLineGemStorageBehavior.State.Items.Count)
                {
                    var item = trackedLineGemStorageBehavior.State.Items[i];
                    UpdateRecordVisual(trackedRecords[i], i, item, showEmptyGems || (item != null));
                }
                else
                {
                    UpdateRecordVisual(trackedRecords[i], i, null, false);
                }
            }
        }

        public LineGemItemVisual GetItemVisual(int slot)
        {
            if (trackedRecords.InBounds(slot))
            {
                return trackedRecords[slot];
            }

            return null;
        }
        
        private void AddRecord(in List<LineGemItemVisual> records)
        {
            var dataItemVisual = Instantiate(lineGemItemVisualPrefab, dataItemLayoutGroup.transform);
            if (dataItemVisual.TryGetComponent<RectTransform>(out var rectTransform))
            {
                dataItemLayoutGroup.TryAddUIElement(rectTransform);
            }
            else
            {
                Debug.LogError("Unable to add memory gem visual to layout group. Make RectTransform", this);
            }
            records.Add(dataItemVisual);
        }
        
        private void UpdateRecordVisual(in LineGemItemVisual recordVisual, int lineNumber, MemoryItem item, bool isActive)
        {
            if (isActive)
            {
                recordVisual.SetGameResources(gameResources);
                recordVisual.SetStorage(trackedLineGemStorageBehavior.State);
                recordVisual.SetDataItem(item);
                recordVisual.SetSlot(lineNumber);
            }
            else
            {
                recordVisual.SetDataItem(null);
                recordVisual.SetStorage(null);
                recordVisual.ResetState();
                recordVisual.SetSlot(-1);
            }
            
            recordVisual.gameObject.SetActive(isActive);
        }

        private void DestroyRecord(LineGemItemVisual record)
        {
            Destroy(record.gameObject);
        }
    }
}
