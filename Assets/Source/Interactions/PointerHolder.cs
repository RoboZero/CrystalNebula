using System;
using Source.Input;
using Source.Logic;
using Source.Logic.Events;
using Source.Visuals;
using UnityEngine;

namespace Source.Interactions
{
    public class PointerHolder : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private PlayerInteractions playerInteractions;
        [SerializeField] private DataItemStorageVisual dataItemStorageVisual;
        [SerializeField] private PlayerItemStorageVisual playerItemStorageVisual;
        [SerializeField] private EventTracker eventTracker;
        [SerializeField] private InputReaderSO inputReader;

        private void OnEnable()
        {
            inputReader.HoldPressedEvent += OnHoldPressed;
        }

        private void OnDisable()
        {
            inputReader.HoldPressedEvent -= OnHoldPressed;
        }
        
        private void OnHoldPressed()
        {
            Debug.Log("Player pressed hold. ");
            eventTracker.AddEvent(new StorageItemTransferEventCommand<DataItem>(
                    dataItemStorageVisual.ItemStorage,
                    dataItemStorageVisual.InteractedVisualIndices,
                    playerItemStorageVisual.ItemStorage,
                    playerItemStorageVisual.InteractedVisualIndices
                )
            );
        }
    }
}
