using System;
using Source.Interactions;
using Source.Logic.Data;
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
        private LineItemData trackedDataItem;
        private LineDataSO lineDataSo;

        private LineDataSO blankGemSO;

        private void Start()
        {
            if (gameResources != null)
            {
                var blankSprite = GameResources.BuildDefinitionPath("Programs", "Blank");
                gameResources.TryLoadAsset(this, blankSprite, out blankGemSO);
            }
        }

        public void SetGameResources(GameResources resources)
        {
            gameResources = resources;
        }
        
        public void SetDataItem(in LineItemData dataItem)
        {
            if (dataItem != null && dataItem != trackedDataItem)
            {
                if (dataItem.Memory != null)
                {
                    gameResources.TryLoadAsset(this, dataItem.Memory.Definition, out lineDataSo);
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
        
        private void SetVisualToItem(LineItemData dataItem)
        {
            iconImage.gameObject.SetActive(false);

            if (dataItem != null && gameResources != null)
            {
                if (dataItem.Memory != null && lineDataSo != null)
                {
                    iconImage.sprite = lineDataSo.Icon;
                    iconImage.gameObject.SetActive(true);
                }
            } else if(blankGemSO != null)
            {
                iconImage.sprite = blankGemSO.Icon;
                iconImage.gameObject.SetActive(true);
            }
        }
    }
}
