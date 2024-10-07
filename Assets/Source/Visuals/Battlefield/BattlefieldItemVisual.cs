using Source.Interactions;
using Source.Logic;
using Source.Logic.Data;
using Source.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Visuals.Battlefield
{
    public class BattlefieldItemVisual : StandardInteractableVisual
    {
        [Header("Dependencies")]
        [SerializeField] private Image buildingImage;
        [SerializeField] private Image unitImage;
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private TMP_Text powerText;
        [SerializeField] private TMP_Text utilityText;
        
        private GameResources gameResources;
        private BattlefieldItemData trackedDataItem;
        private string originalText;
        private UnitDataSO unitDataSO;
        private BuildingDataSO buildingDataSO;

        public void SetGameResources(GameResources resources)
        {
            gameResources = resources;
        }
        
        // TODO: Pass scriptable objects to visuals with more data, updates in real time. 
        public void SetDataItem(in BattlefieldItemData dataItem)
        {
            if (dataItem != null && dataItem != trackedDataItem)
            {
                if (dataItem.Unit != null)
                {
                    gameResources.TryLoadAsset(this, dataItem.Unit.Definition, out unitDataSO);
                }

                if (dataItem.Building != null)
                {
                    gameResources.TryLoadAsset(this, dataItem.Building.Definition, out buildingDataSO);
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
                    utilityText.text = trackedDataItem != null && 
                                       unitDataSO != null ? unitDataSO.Name : "";
                    break;
                case InteractVisualState.Hovered:
                    utilityText.text = "STATE: HOVERED";
                    break;
                case InteractVisualState.Selected:
                    utilityText.text = "STATE: SELECTED";
                    break;
            }
        }
        
        private void SetVisualToItem(BattlefieldItemData dataItem)
        {
            buildingImage.gameObject.SetActive(false);
            unitImage.gameObject.SetActive(false);
            healthText.gameObject.SetActive(false);
            powerText.gameObject.SetActive(false);

            if (dataItem != null && gameResources != null)
            { 
                if (dataItem.Unit != null && unitDataSO != null)
                {
                    unitImage.sprite = unitDataSO.Sprite;
                    healthText.text = dataItem.Unit.Health.ToString();
                    powerText.text = dataItem.Unit.Power.ToString();
                    unitImage.gameObject.SetActive(true);
                    healthText.gameObject.SetActive(true);
                    powerText.gameObject.SetActive(true);
                }
                
                if (dataItem.Building != null && buildingDataSO != null)
                {
                    buildingImage.sprite = buildingDataSO.Sprite;
                    buildingImage.gameObject.SetActive(true);
                    healthText.gameObject.SetActive(true);
                    powerText.gameObject.SetActive(true);
                }
            }
        }
    }
}
