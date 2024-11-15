using System;
using System.Collections.Generic;
using Source.Interactions;
using Source.Logic.State.LineItems;
using Source.Serialization;
using Source.Visuals.Tooltip;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Visuals.MemoryStorage
{
    public class LineGemItemVisual : StandardInteractableVisual, ITooltipTarget
    {
        [Header("Dependencies")]
        [SerializeField] private Image progressImage;
        [SerializeField] private Image emptyGemImage;
        [SerializeField] private LineGemSubItemVisual currentSubVisual;
        [SerializeField] private LineGemSubItemVisual transferSubVisual;

        public LineStorage<MemoryItem> TrackedLineStorage => trackedLineStorage;

        public MemoryItem TrackedItem => currentSubVisual.TrackedItem;
        public int TrackedSlot => trackedSlot;
        
        // TODO: Reevaluate if each item needs all info Storage gives
        private GameResources gameResources;
        private LineStorage<MemoryItem> trackedLineStorage;
        private int trackedSlot;
        private float trackedTransferProgressPercent = 1;
        private bool showEmptyGem;
        private bool isTransferring;

        private readonly TooltipContent memoryTooltipContent = new();

        public void IsTransferring(bool isTransferring)
        {
            this.isTransferring = isTransferring;
        }

        public void SetShowEmptyGem(bool showEmptyGem)
        {
            this.showEmptyGem = showEmptyGem;

            emptyGemImage.gameObject.SetActive(this.showEmptyGem);
        }

        public void SetGameResources(GameResources resources)
        {
            gameResources = resources;
        }

        public void SetStorage(LineStorage<MemoryItem> lineStorage)
        {
            trackedLineStorage = lineStorage;
        }
        
        public void SetSlot(int slot)
        { 
            trackedSlot = slot;
        }
        
        public void SetTransferProgressPercent(float transferProgressPercent)
        {
            trackedTransferProgressPercent = transferProgressPercent;
        }

        public void SetCurrentDataItem(MemoryItem item)
        {
            currentSubVisual.SetDataItem(item, gameResources);

            if (item == null || currentSubVisual.MemoryDataSO == null)
            {
                return;
            }

            memoryTooltipContent.Header = currentSubVisual.MemoryDataSO.MemoryName;
            memoryTooltipContent.Description = currentSubVisual.MemoryDataSO.MemoryDescription;
        }

        public void SetTransferDataItem(MemoryItem item)
        {
            transferSubVisual.SetDataItem(item, gameResources);
        }

        private void Update()
        {
            currentSubVisual.UpdateVisual(isTransferring ? 1 - trackedTransferProgressPercent : 1);
            currentSubVisual.UpdateInteractionVisual(CurrentVisualState);
            transferSubVisual.UpdateVisual(isTransferring ? trackedTransferProgressPercent : 0);
            transferSubVisual.UpdateInteractionVisual(CurrentVisualState);

            switch (CurrentVisualState)
            {
                case InteractVisualState.None:
                    emptyGemImage.color = Color.white; 
                    break;
                case InteractVisualState.Hovered:
                    emptyGemImage.color = Color.yellow;
                    break;
                case InteractVisualState.Selected:
                    emptyGemImage.color = Color.blue;
                    break;
            }
        }

        public void UpdateContent(TooltipVisual tooltipVisual)
        {
            if (TrackedItem != null && currentSubVisual.MemoryDataSO != null)
            {
                currentSubVisual.MemoryDataSO.FillTooltipContent(TrackedItem, memoryTooltipContent);
                tooltipVisual.AddContent(memoryTooltipContent);
            }
        }
    }
}
