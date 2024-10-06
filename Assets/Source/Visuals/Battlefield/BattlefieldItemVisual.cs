using Source.Interactions;
using Source.Logic;
using Source.Serialization;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Visuals.Battlefield
{
    public class BattlefieldItemVisual : StandardInteractableVisual
    {
        [Header("Dependencies")]
        [SerializeField] private Image buildingImage;
        [SerializeField] private Image unitImage;
        [SerializeField] private TMPro.TMP_Text healthText;
        [SerializeField] private TMPro.TMP_Text powerText;
        [SerializeField] private TMPro.TMP_Text utilityText;
        
        private GameResources gameResources;
        private BattlefieldDataItem trackedDataItem;
        private string originalText;
        private UnitDataSO unitDataSO;
        private BuildingDataSO buildingDataSO;

        public void SetGameResources(GameResources resources)
        {
            gameResources = resources;
        }
        
        // TODO: Pass scriptable objects to visuals with more data, updates in real time. 
        public void SetDataItem(in BattlefieldDataItem dataItem)
        {
            if (dataItem != null && dataItem != trackedDataItem)
            {
                if (dataItem.UnitData != null)
                {
                    unitDataSO = gameResources.TryLoadAsset<UnitDataSO>(this, dataItem.UnitData.Definition);
                }

                if (dataItem.BuildingData != null)
                {
                    unitDataSO = gameResources.TryLoadAsset<UnitDataSO>(this, dataItem.BuildingData.Definition);
                }
            }
            
            trackedDataItem = dataItem;
        }

        private void Update()
        {
            RefreshVisualWithTrackedItem(trackedDataItem);
            
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
        
        private void RefreshVisualWithTrackedItem(BattlefieldDataItem trackedDataItem)
        {
            buildingImage.gameObject.SetActive(false);
            unitImage.gameObject.SetActive(false);
            healthText.gameObject.SetActive(false);
            powerText.gameObject.SetActive(false);

            if (trackedDataItem != null && gameResources != null)
            { 
                if (trackedDataItem.UnitData != null && unitDataSO != null)
                {
                    unitImage.sprite = unitDataSO.Sprite;
                    healthText.text = trackedDataItem.UnitData.Health.ToString();
                    powerText.text = trackedDataItem.UnitData.Power.ToString();
                    unitImage.gameObject.SetActive(true);
                    healthText.gameObject.SetActive(true);
                    powerText.gameObject.SetActive(true);
                }
                
                if (trackedDataItem.BuildingData != null && buildingDataSO != null)
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
