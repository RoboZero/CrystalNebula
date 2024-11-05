using System;
using System.Collections.Generic;
using Source.Interactions;
using Source.Logic;
using Source.Logic.State;
using Source.Serialization;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Visuals.Battlefield
{
    [RequireComponent(typeof(BattlefieldStorageBehavior))]
    public class BattlefieldStorageVisual : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private BattlefieldItemVisual memoryItemVisualPrefab;
        [SerializeField] private LayoutGroup lineNumberLayoutGroup;
        [SerializeField] private LayoutGroup dataItemLayoutGroup;
        
        [SerializeField] private GameResources gameResources;

        private BattlefieldStorageBehavior battlefieldStorageBehavior;

        private List<DataItemRecordVisual> trackedRecords = new();

        private void Awake()
        {
            battlefieldStorageBehavior = GetComponent<BattlefieldStorageBehavior>();
            
            memoryItemVisualPrefab.gameObject.SetActive(false);
        }

        private void Update()
        {
            battlefieldStorageBehavior.Tick();

            while (battlefieldStorageBehavior.State.Items.Count > trackedRecords.Count)
            {
                AddRecord(trackedRecords);
            }

            for (var i = 0; i < trackedRecords.Count; i++)
            {
                var item = battlefieldStorageBehavior.State.Items[i];
                UpdateRecordVisual(trackedRecords[i], i, item, true);
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
            
            dataItemVisual.SetLineNumber(records.Count);
            var record = new DataItemRecordVisual()
            {
                ItemVisual = dataItemVisual,
            };
            
            records.Add(record);
        }

        private void UpdateRecordVisual(in DataItemRecordVisual recordVisual, int lineNumber, BattlefieldItem item, bool isActive)
        {
            if (isActive)
            {
                recordVisual.ItemVisual.gameObject.name = "Slot " + lineNumber;
                recordVisual.ItemVisual.SetGameResources(gameResources);
                recordVisual.ItemVisual.SetDataItem(item);
                recordVisual.ItemVisual.SetSlot(lineNumber);
            }
            else
            {
                recordVisual.ItemVisual.SetDataItem(null);
                recordVisual.ItemVisual.SetSlot(-1);
                recordVisual.ItemVisual.ResetState();
            }
            
            recordVisual.ItemVisual.gameObject.SetActive(isActive);
        }

        private void DestroyRecord(DataItemRecordVisual record)
        {
            Destroy(record.ItemVisual.gameObject);
        }

        [Serializable]
        private struct DataItemRecordVisual
        {
            public BattlefieldItemVisual ItemVisual;
        }
    }
}
