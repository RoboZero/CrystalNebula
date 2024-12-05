using System;
using System.Collections.Generic;
using Source.Interactions;
using Source.Logic.State;
using Source.Logic.State.LineItems;
using Source.Serialization;
using Source.Visuals.Levels;
using Source.Visuals.Tooltip;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Visuals.MemoryStorage
{
    public class LineGemItemVisual : StandardInteractableVisual, ITooltipTarget
    {
        [Header("Dependencies")]
        [SerializeField] private Image raycastTargetImage;
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
        private Level trackedLevel;
        private LevelDataSO levelDataSO;
        private int trackedSlot;
        private float trackedTransferProgressPercent = 1;
        private bool showEmptyGem;
        private bool isTransferring;

        private readonly HashSet<TooltipContent> tooltipContents = new();
        private readonly TooltipContent memoryTooltipContent = new();
        private Color noneColor = Color.white;
        private Color hoveredColor = Color.yellow;
        private Color interactedColor = Color.blue;

        public void IsRaycastTarget(bool isRaycastTarget)
        {
            raycastTargetImage.raycastTarget = isRaycastTarget;
            currentSubVisual.IsRaycastTarget(isRaycastTarget);
            transferSubVisual.IsRaycastTarget(isRaycastTarget);
        }

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

        public void SetLevel(Level level)
        {
            if (level != null && level != trackedLevel)
            {
                if (gameResources != null && level.Definition != null)
                {
                    gameResources.TryLoadAsset(this, level.Definition, out levelDataSO);
                }
            }
            
            currentSubVisual.SetLevel(level, levelDataSO);
            trackedLevel = level;
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
            UpdateVisual();

            switch (CurrentVisualState)
            {
                case InteractVisualState.None:
                    emptyGemImage.color = noneColor;
                    break;
                case InteractVisualState.Hovered:
                    emptyGemImage.color = hoveredColor;
                    break;
                case InteractVisualState.Selected:
                    emptyGemImage.color = interactedColor;
                    break;
            }
        }

        private void UpdateVisual()
        {
            if (currentSubVisual.TrackedItem != null && trackedLevel != null && levelDataSO != null)
            {
                var colorScheme = levelDataSO.ColorSchemeAssociationsSO.GetColorScheme(currentSubVisual.TrackedItem.OwnerId);
                
                progressImage.color = colorScheme.MemoryProgressColor;
                noneColor = colorScheme.NoInteractionColor;
                hoveredColor = colorScheme.HoveredColor;
                interactedColor = colorScheme.InteractedColor;
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

        public HashSet<TooltipContent> GetContent()
        {
            tooltipContents.Clear();
            
            if (TrackedItem != null && currentSubVisual.MemoryDataSO != null)
            {
                currentSubVisual.MemoryDataSO.FillTooltipContent(TrackedItem, memoryTooltipContent);
                tooltipContents.Add(memoryTooltipContent);
            }

            return tooltipContents;
        }
    }
}
