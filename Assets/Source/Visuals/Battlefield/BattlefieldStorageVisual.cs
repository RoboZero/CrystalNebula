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
    [RequireComponent(typeof(BattlefieldStorageBehavior))]
    public class BattlefieldStorageVisual : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private LineNumberVisual lineNumberVisualPrefab;
        [SerializeField] private BattlefieldItemVisual memoryItemVisualPrefab;
        [SerializeField] private LayoutGroup lineNumberLayoutGroup;
        [SerializeField] private LayoutGroup dataItemLayoutGroup;
        
        [SerializeField] private GameResources gameResources;

        private BattlefieldStorageBehavior battlefieldStorageBehavior;

        public List<int> InteractedVisualIndices => interactedVisualIndices;
        
        private List<int> interactedVisualIndices = new();
        private List<DataItemRecordVisual> trackedRecords = new();

        private void Awake()
        {
            battlefieldStorageBehavior = GetComponent<BattlefieldStorageBehavior>();
            
            lineNumberVisualPrefab.gameObject.SetActive(false);
            memoryItemVisualPrefab.gameObject.SetActive(false);
        }

        private void Update()
        {
            battlefieldStorageBehavior.Tick();

            while (battlefieldStorageBehavior.State.Items.Count > trackedRecords.Count)
            {
                AddRecord(trackedRecords);
            }

            interactedVisualIndices.Clear();
            for (var i = 0; i < trackedRecords.Count; i++)
            {
                var item = battlefieldStorageBehavior.State.Items[i];
                UpdateRecordVisual(trackedRecords[i], i, item, true);
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

        private void UpdateRecordVisual(in DataItemRecordVisual recordVisual, int lineNumber, BattlefieldItem item, bool isActive)
        {
            if (isActive)
            {
                recordVisual.ItemVisual.SetGameResources(gameResources);
                recordVisual.ItemVisual.SetDataItem(item);
                recordVisual.LineNumberVisual.Value = lineNumber;
            }
            else
            {
                recordVisual.ItemVisual.SetDataItem(null);
                recordVisual.ItemVisual.ResetState();
            }
            
            recordVisual.ItemVisual.gameObject.SetActive(isActive);
            recordVisual.LineNumberVisual.gameObject.SetActive(isActive);
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
