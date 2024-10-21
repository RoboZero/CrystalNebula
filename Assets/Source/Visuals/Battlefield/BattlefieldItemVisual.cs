using Source.Interactions;
using Source.Logic;
using Source.Logic.Data;
using Source.Serialization;
using Source.Utility;
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

        public BattlefieldItem TrackedItem => trackedItem;
        public int TrackedSlot => trackedSlot;
        
        private GameResources gameResources;
        private BattlefieldItem trackedItem;
        private int trackedSlot;
        private string originalText;

        private string unitDataDefinition;
        private UnitDataSO unitDataSO;
        private string buildingDataDefinition;
        private BuildingDataSO buildingDataSO;

        public void SetGameResources(GameResources resources)
        {
            gameResources = resources;
        }
        
        public void SetDataItem(BattlefieldItem item)
        {
            if (item == null)
            {
                unitDataSO = null;
                buildingDataSO = null;
                trackedItem = null;
                return;
            }

            if (item != trackedItem)
            {
                unitDataDefinition = "";
                buildingDataDefinition = "";
            }

            trackedItem = item;
        }

        public void SetSlot(int slot)
        { 
            trackedSlot = slot;
        }

        private void Update()
        {
            SetVisualToItem(trackedItem);
            
            switch (CurrentVisualState)
            {
                case InteractVisualState.None:
                    utilityText.text = trackedItem != null && 
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
        
        private void SetVisualToItem(BattlefieldItem item)
        {
            buildingImage.gameObject.SetActive(false);
            unitImage.gameObject.SetActive(false);
            healthText.gameObject.SetActive(false);
            powerText.gameObject.SetActive(false);

            if (item == null || gameResources == null) return;

            if (item.Unit != null)
            {
                if (item.Unit.Definition != null && item.Unit.Definition != unitDataDefinition)
                {
                    gameResources.TryLoadAsset(this, item.Unit.Definition, out unitDataSO);
                    unitDataDefinition = item.Unit.Definition;
                }

                if (unitDataSO != null)
                {
                    unitImage.sprite = unitDataSO.Sprite;
                    healthText.text = item.Unit.Health.ToString();
                    powerText.text = item.Unit.Power.ToString();
                    unitImage.gameObject.SetActive(true);
                    healthText.gameObject.SetActive(true);
                    powerText.gameObject.SetActive(true);
                }
            }

            if (item.Building != null)
            {
                if (item.Building.Definition != null && item.Building.Definition != buildingDataDefinition)
                {
                    gameResources.TryLoadAsset(this, item.Building.Definition, out buildingDataSO);
                    buildingDataDefinition = item.Building.Definition;
                }
                
                if (buildingDataSO != null)
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
