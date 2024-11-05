using Source.Interactions;
using Source.Logic;
using Source.Logic.State;
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
        [SerializeField] private Image selectorIcon;
        [SerializeField] private TMP_Text lineNumberText;
        [SerializeField] private Image buildingImage;
        [SerializeField] private Image buildingPlatformImage;
        [SerializeField] private GameObject buildingStatsHolder;
        [SerializeField] private TMP_Text buildingHealthText;
        [SerializeField] private TMP_Text buildingPowerText;
        [SerializeField] private TMP_Text buildingUtilityText;
        [SerializeField] private Image unitImage;
        [SerializeField] private Image unitPlatformImage;
        [SerializeField] private GameObject unitStatsHolder;
        [SerializeField] private TMP_Text unitHealthText;
        [SerializeField] private TMP_Text unitPowerText;
        [SerializeField] private TMP_Text unitUtilityText;

        public BattlefieldItem TrackedItem => trackedItem;
        public int TrackedSlot => trackedSlot;
        
        private GameResources gameResources;
        private BattlefieldItem trackedItem;
        private int trackedSlot;
        private int assignedLineNumber;
        private string originalText;

        private string unitDataDefinition;
        private UnitDataSO unitDataSO;
        private string buildingDataDefinition;
        private BuildingDataSO buildingDataSO;

        public void SetGameResources(GameResources resources)
        {
            gameResources = resources;
        }

        public void SetLineNumber(int lineNumber)
        {
            assignedLineNumber = lineNumber;
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
            lineNumberText.gameObject.SetActive(true);
            lineNumberText.text = assignedLineNumber.ToString();
            SetVisualToItem(trackedItem);
            
            switch (CurrentVisualState)
            {
                case InteractVisualState.None:
                    selectorIcon.gameObject.SetActive(false);
                    buildingUtilityText.text = "";
                    unitUtilityText.text = "";
                    break;
                case InteractVisualState.Hovered:
                    selectorIcon.gameObject.SetActive(true);
                    selectorIcon.color = Color.yellow;
                    //unitUtilityText.text = "STATE: HOVERED";
                    buildingUtilityText.text = trackedItem != null && 
                                               trackedItem.Building != null && 
                                               buildingDataSO != null ? buildingDataSO.Abbreviation : "";
                    unitUtilityText.text = trackedItem != null && 
                                           trackedItem.Unit != null && 
                                           unitDataSO != null ? unitDataSO.Abbreviation : "";
                    break;
                case InteractVisualState.Selected:
                    selectorIcon.gameObject.SetActive(true);
                    selectorIcon.color = Color.blue;
                    //unitUtilityText.text = "STATE: SELECTED";
                    buildingUtilityText.text = trackedItem != null && 
                                               trackedItem.Building != null && 
                                               buildingDataSO != null ? buildingDataSO.Abbreviation : "";
                    unitUtilityText.text = trackedItem != null && 
                                           trackedItem.Unit != null && 
                                           unitDataSO != null ? unitDataSO.Abbreviation : "";
                    break;
            }
        }
        
        private void SetVisualToItem(BattlefieldItem item)
        {
            buildingImage.gameObject.SetActive(false);
            buildingPlatformImage.gameObject.SetActive(false);
            buildingStatsHolder.gameObject.SetActive(false);
            buildingHealthText.gameObject.SetActive(false);
            buildingPowerText.gameObject.SetActive(false);
            unitImage.gameObject.SetActive(false);
            unitPlatformImage.gameObject.SetActive(false);
            unitStatsHolder.gameObject.SetActive(false);
            unitHealthText.gameObject.SetActive(false);
            unitPowerText.gameObject.SetActive(false);

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
                    unitHealthText.text = item.Unit.Health.ToString();
                    unitPowerText.text = item.Unit.Power.ToString();
                    unitImage.gameObject.SetActive(true);
                    unitPlatformImage.gameObject.SetActive(true);
                    unitStatsHolder.gameObject.SetActive(true);
                    unitHealthText.gameObject.SetActive(true);
                    unitPowerText.gameObject.SetActive(true);
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
                    buildingHealthText.text = item.Building.Health.ToString();
                    buildingPowerText.text = item.Building.Power.ToString();
                    buildingImage.gameObject.SetActive(true);
                    buildingPlatformImage.gameObject.SetActive(true);
                    buildingStatsHolder.gameObject.SetActive(true);
                    buildingHealthText.gameObject.SetActive(true);
                    buildingPowerText.gameObject.SetActive(true);
                }
            }
        }
    }
}
