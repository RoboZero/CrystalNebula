using System;
using Source.Interactions;
using Source.Logic;
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
        private LineItem trackedItem;
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
        
        public void SetDataItem(in LineItem item)
        {
            if (item != null && item != trackedItem)
            {
                if (item.Memory != null && item.Memory.Definition != null)
                {
                    gameResources.TryLoadAsset(this, item.Memory.Definition, out lineDataSo);
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
            
            if (item != null && item.Memory != null && lineDataSo != null)
            {
                iconImage.sprite = lineDataSo.Icon;
                iconImage.gameObject.SetActive(true);
            } else if(blankGemSO != null)
            {
                iconImage.sprite = blankGemSO.Icon;
                iconImage.gameObject.SetActive(true);
            }
        }
    }
}
