using Source.Interactions;
using Source.Logic.Data;
using Source.Serialization;
using Source.Visuals.Battlefield;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Visuals.ProgramStorage
{
    public class MemoryGemItemVisual : StandardInteractableVisual
    {
        [Header("Dependencies")]
        [SerializeField] private Image iconImage;
        
        private GameResources gameResources;
        private MemoryItemData trackedDataItem;
        private MemoryDataSO memoryDataSO;

        public void SetGameResources(GameResources resources)
        {
            gameResources = resources;
        }
        
        public void SetDataItem(in MemoryItemData dataItem)
        {
            if (dataItem != null && dataItem != trackedDataItem)
            {
                if (dataItem.Memory != null)
                {
                    gameResources.TryLoadAsset(this, dataItem.Memory.Definition, out memoryDataSO);
                }
            }
            
            trackedDataItem = dataItem;
        }

        private void Update()
        {
            SetVisualToItem(trackedDataItem);
            
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
        
        private void SetVisualToItem(MemoryItemData dataItem)
        {
            iconImage.gameObject.SetActive(false);

            if (dataItem != null && gameResources != null)
            { 
                if (dataItem.Memory != null && memoryDataSO != null)
                {
                    iconImage.sprite = memoryDataSO.Icon;
                    iconImage.gameObject.SetActive(true);
                }
            }
        }
    }
}
