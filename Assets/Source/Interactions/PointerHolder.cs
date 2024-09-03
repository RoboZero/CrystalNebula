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
        [SerializeField] private MemoryStorageVisual memoryStorageVisual;
        [SerializeField] private PlayerItemStorageVisual playerItemStorageVisual;
        [SerializeField] private EventTracker eventTracker;
        [SerializeField] private InputReader inputReader;

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
            eventTracker.AddEvent(new StorageItemTransferEventCommand<DataItem>(
                    memoryStorageVisual.ItemStorage,
                    memoryStorageVisual.InteractedVisualIndices,
                    playerItemStorageVisual.ItemStorage,
                    playerItemStorageVisual.InteractedVisualIndices
                )
            );
        }
    }
}
