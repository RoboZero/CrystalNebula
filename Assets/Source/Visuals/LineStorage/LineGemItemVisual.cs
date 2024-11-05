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
        [SerializeField] private Image iconImage;
        
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
                    iconImage.color = Color.white; 
                    break;
                case InteractVisualState.Hovered:
                    iconImage.color = Color.yellow;
                    break;
                case InteractVisualState.Selected:
                    iconImage.color = Color.blue;
                    break;
            }
        }
        
        private void SetVisualToItem(LineItem item)
        {
            iconImage.gameObject.SetActive(false);

            if (gameResources == null) return;
            
            if (item != null && item.Memory != null && memoryDataSO != null)
            {
                iconImage.sprite = memoryDataSO.MemoryBackgroundIcon;
                iconImage.gameObject.SetActive(true);
            } else if(emptyGemSO != null)
            {
                iconImage.sprite = emptyGemSO.MemoryBackgroundIcon;
                iconImage.gameObject.SetActive(true);
            }
        }
    }
}
