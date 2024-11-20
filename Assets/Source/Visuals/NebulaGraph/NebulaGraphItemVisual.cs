using Source.Interactions;
using Source.Logic.State;
using Source.Logic.State.LineItems;
using Source.Serialization;
using Source.Visuals.Levels;
using Source.Visuals.MemoryStorage;
using Source.Visuals.Tooltip;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Visuals.NebulaGraph
{
    public class NebulaGraphItemVisual : StandardInteractableVisual, ITooltipTarget
    {
        [Header("Dependencies")]
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image foregroundImage;

        private GameResources gameResources;
        private Level trackedLevel;
        private LevelDataSO levelDataSO;
        private MemoryItem trackedItem;
        private MemoryDataSO memoryDataSO;
        
        private readonly TooltipContent memoryTooltipContent = new();
        private Color noneColor = Color.white;
        private Color hoveredColor = Color.yellow;
        private Color interactedColor = Color.blue;

        public void SetMemoryItem(MemoryItem item, MemoryDataSO memoryData)
        {
            trackedItem = item;
            memoryDataSO = memoryData;
        }
        
        public void SetLevel(Level level, LevelDataSO levelData)
        {
            trackedLevel = level;
            levelDataSO = levelData;
        }
        
        private void Update()
        {
            UpdateVisual();
            
            switch (CurrentVisualState)
            {
                case InteractVisualState.None:
                    backgroundImage.color = noneColor;
                    break;
                case InteractVisualState.Hovered:
                    backgroundImage.color = hoveredColor;
                    break;
                case InteractVisualState.Selected:
                    backgroundImage.color = interactedColor;
                    break;
            }
        }

        private void UpdateVisual()
        {
            if (trackedItem != null && memoryDataSO != null)
            {
                backgroundImage.sprite = memoryDataSO.MemoryBackgroundIcon;
                foregroundImage.sprite = memoryDataSO.MemoryForegroundIcon;
                
                if (trackedLevel != null && levelDataSO != null)
                {
                    var colorScheme = levelDataSO.ColorSchemeAssociationsSO.GetColorScheme(trackedItem.OwnerId);
                    noneColor = colorScheme.NoInteractionColor;
                    hoveredColor = colorScheme.HoveredColor;
                    interactedColor = colorScheme.InteractedColor;
                }
            } 
        }
        
        public void UpdateContent(TooltipVisual tooltipVisual)
        {
            if (trackedItem != null && memoryDataSO != null)
            {
                memoryDataSO.FillTooltipContent(trackedItem, memoryTooltipContent);
                tooltipVisual.AddContent(memoryTooltipContent);
            }
        }
    }
}
