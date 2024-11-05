using Source.Interactions;
using Source.Logic.State.LineItems;
using Source.Serialization;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Visuals.LineStorage
{
    public class LineGemItemVisual : StandardInteractableVisual
    {
        [Header("Dependencies")]
        [SerializeField] private Image progressImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image foregroundImage;
        
        private GameResources gameResources;
        private LineItem trackedItem;
        private MemoryDataSO memoryDataSO;

        private MemoryDataSO emptyGemSO;

        private void Start()
        {
            if (gameResources != null)
            {
                var emptySprite = GameResources.BuildDefinitionPath(GameResourceConstants.PROGRAMS_PATH, GameResourceConstants.EMPTY_MEMORY_DATA_DEFINITION);
                gameResources.TryLoadAsset(this, emptySprite, out emptyGemSO);
            }
        }

        public void SetGameResources(GameResources resources)
        {
            gameResources = resources;
        }
        
        public void SetDataItem(in LineItem item)
        {
            if (item != null && item != trackedItem)
            {
                if (item.Memory != null && item.Memory.Definition != null)
                {
                    gameResources.TryLoadAsset(this, item.Memory.Definition, out memoryDataSO);
                }
            }
            
            trackedItem = item;
        }

        private void Update()
        {
            SetVisualToItem(trackedItem);
            
            switch (CurrentVisualState)
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
        
        private void SetVisualToItem(LineItem item)
        {
            backgroundImage.gameObject.SetActive(false);

            if (gameResources == null) return;
            
            if (item != null && item.Memory != null && memoryDataSO != null)
            {
                backgroundImage.sprite = memoryDataSO.MemoryBackgroundIcon;
                backgroundImage.gameObject.SetActive(true);

                if (memoryDataSO.MemoryForegroundIcon != null)
                {
                    foregroundImage.sprite = memoryDataSO.MemoryForegroundIcon;
                    foregroundImage.gameObject.SetActive(true);
                }

                progressImage.fillAmount = ((float) item.Memory.CurrentProgress) / item.Memory.MaxProgress;
                progressImage.gameObject.SetActive(true);
            } else if(emptyGemSO != null)
            {
                backgroundImage.sprite = emptyGemSO.MemoryBackgroundIcon;
                backgroundImage.gameObject.SetActive(true);
                foregroundImage.gameObject.SetActive(false);
                progressImage.gameObject.SetActive(false);
            }
        }
    }
}
