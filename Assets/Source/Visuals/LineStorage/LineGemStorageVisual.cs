using System.Collections.Generic;
using Source.Interactions;
using Source.Logic.State.LineItems;
using Source.Serialization;
using Source.Utility;
using UnityEngine;

namespace Source.Visuals.LineStorage
{
    public class LineGemStorageVisual : MonoBehaviour
    {
        [Header("Dependencies")]
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

            while (trackedLineGemStorageBehavior.State.Items.Count > trackedRecords.Count)
            {
                AddRecord(trackedRecords);
            }

            for (var i = 0; i < trackedRecords.Count; i++)
            {
                var item = trackedLineGemStorageBehavior.State.Items[i];
                UpdateRecordVisual(trackedRecords[i], i, item, showEmptyGems || (item.Memory != null));
            }
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
        
        private void UpdateRecordVisual(in LineGemItemVisual recordVisual, int lineNumber, LineItem item, bool isActive)
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
