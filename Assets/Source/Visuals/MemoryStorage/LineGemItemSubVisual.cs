using Source.Interactions;
using Source.Logic.State.LineItems;
using Source.Serialization;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Visuals.MemoryStorage
{
    public class LineGemSubItemVisual : MonoBehaviour
    {
        public MemoryItem TrackedItem => trackedItem;
        
        [Header("Dependencies")]
        [SerializeField] private Image progressImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image foregroundImage;
        
        private MemoryItem trackedItem;
        private MemoryDataSO memoryDataSO;
        
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
                    backgroundImage.color = Color.white; 
                    break;
                case InteractVisualState.Hovered:
                    backgroundImage.color = Color.yellow;
                    break;
                case InteractVisualState.Selected:
                    backgroundImage.color = Color.blue;
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