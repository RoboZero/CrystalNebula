using Source.Interactions;
using Source.Logic.State;
using Source.Logic.State.LineItems;
using Source.Serialization;
using Source.Visuals.Levels;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Visuals.MemoryStorage
{
    public class LineGemSubItemVisual : MonoBehaviour
    {
        public MemoryItem TrackedItem => trackedItem;
        public MemoryDataSO MemoryDataSO => memoryDataSO;
        
        [Header("Dependencies")]
        [SerializeField] private Image progressImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image foregroundImage;

        private Level trackedLevel;
        private LevelDataSO levelDataSO;
        private MemoryItem trackedItem;
        private MemoryDataSO memoryDataSO;
        
        private Color noneColor = Color.white;
        private Color hoveredColor = Color.yellow;
        private Color interactedColor = Color.blue;
        
        public void IsRaycastTarget(bool isRaycastTarget)
        {
            progressImage.raycastTarget = isRaycastTarget;
            backgroundImage.raycastTarget = isRaycastTarget;
            foregroundImage.raycastTarget = isRaycastTarget;
        }

        public void SetLevel(Level level, LevelDataSO levelData)
        {
            trackedLevel = level;
            levelDataSO = levelData;
        }
        
        // TODO: Remove resource retrieval every frame
        public void SetDataItem(MemoryItem item, GameResources gameResources)
        {
            if (item != null && item != trackedItem)
            {
                if (gameResources != null && item.Definition != null)
                {
                    gameResources.TryLoadAsset(this, item.Definition, out memoryDataSO);
                }
            }
            
            trackedItem = item;
        }

        public void UpdateInteractionVisual(InteractVisualState currentVisualState)
        {
            if (trackedItem == null) return;
            
            switch (currentVisualState)
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

        public void UpdateVisual(float transferProgressPercent)
        {
            if (trackedItem != null && memoryDataSO != null)
            {
                backgroundImage.sprite = memoryDataSO.MemoryBackgroundIcon;
                backgroundImage.gameObject.SetActive(true);
                backgroundImage.fillAmount = transferProgressPercent;

                if (memoryDataSO.MemoryForegroundIcon != null)
                {
                    foregroundImage.sprite = memoryDataSO.MemoryForegroundIcon;
                    foregroundImage.gameObject.SetActive(true);
                }
                foregroundImage.fillAmount = transferProgressPercent;
                
                if (trackedLevel != null && levelDataSO != null)
                {
                    var colorScheme = levelDataSO.ColorSchemeAssociationsSO.GetColorScheme(trackedItem.OwnerId);
                    progressImage.color = colorScheme.MemoryProgressColor;
                    noneColor = colorScheme.NoInteractionColor;
                    hoveredColor = colorScheme.HoveredColor;
                    interactedColor = colorScheme.InteractedColor;
                }
                
                var totalProgressFillPercent = ((float)trackedItem.CurrentRunProgress) / trackedItem.MaxRunProgress;
                progressImage.fillAmount = Mathf.Min(transferProgressPercent, totalProgressFillPercent);
                progressImage.gameObject.SetActive(true);
            } 
            else
            {
                backgroundImage.gameObject.SetActive(false);
                foregroundImage.gameObject.SetActive(false);
                progressImage.gameObject.SetActive(false);
            }
        }
    }
}