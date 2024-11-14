using System.Collections.Generic;
using Source.Interactions;
using Source.Logic.State.Battlefield;
using Source.Logic.State.LineItems;
using Source.Serialization;
using Source.Visuals.Tooltip;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Visuals.BattlefieldStorage
{
    public class BattlefieldItemVisual : StandardInteractableVisual, ITooltipTarget
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

        public LineStorage<BattlefieldItem> TrackedBattlefieldStorage => trackedBattlefieldStorage;
        public BattlefieldItem TrackedItem => trackedItem;
        public int TrackedSlot => trackedSlot;
        
        private GameResources gameResources;
        private LineStorage<BattlefieldItem> trackedBattlefieldStorage;
        private BattlefieldItem trackedItem;
        private int trackedSlot;
        private int assignedLineNumber;
        private string originalText;

        private string unitDataDefinition;
        private UnitMemoryDataSO unitMemoryDataSO;
        private string buildingDataDefinition;
        private BuildingMemoryDataSO buildingMemoryDataSO;

        private readonly TooltipContent unitTooltipContent = new();
        private readonly TooltipContent buildingTooltipContent = new();

        public void SetGameResources(GameResources resources)
        {
            gameResources = resources;
        }
        
        public void SetStorage(LineStorage<BattlefieldItem> battlefieldStorage)
        {
            trackedBattlefieldStorage = battlefieldStorage;
        }

        public void SetLineNumber(int lineNumber)
        {
            assignedLineNumber = lineNumber;
        }
        
        public void SetDataItem(BattlefieldItem item)
        {
            if (item == null)
            {
                unitMemoryDataSO = null;
                buildingMemoryDataSO = null;
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
                                               buildingMemoryDataSO != null ? buildingMemoryDataSO.Abbreviation : "";
                    unitUtilityText.text = trackedItem != null && 
                                           trackedItem.Unit != null && 
                                           unitMemoryDataSO != null ? unitMemoryDataSO.Abbreviation : "";
                    break;
                case InteractVisualState.Selected:
                    selectorIcon.gameObject.SetActive(true);
                    selectorIcon.color = Color.blue;
                    //unitUtilityText.text = "STATE: SELECTED";
                    buildingUtilityText.text = trackedItem != null && 
                                               trackedItem.Building != null && 
                                               buildingMemoryDataSO != null ? buildingMemoryDataSO.Abbreviation : "";
                    unitUtilityText.text = trackedItem != null && 
                                           trackedItem.Unit != null && 
                                           unitMemoryDataSO != null ? unitMemoryDataSO.Abbreviation : "";
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

            if (item == null || gameResources == null)
            {
                return;
            }

            if (item.Unit != null)
            {
                if (item.Unit.Definition != null && item.Unit.Definition != unitDataDefinition)
                {
                    gameResources.TryLoadAsset(this, item.Unit.Definition, out unitMemoryDataSO);
                    unitDataDefinition = item.Unit.Definition;
                }

                if (unitMemoryDataSO != null)
                {
                    unitImage.sprite = unitMemoryDataSO.Sprite;
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
                    gameResources.TryLoadAsset(this, item.Building.Definition, out buildingMemoryDataSO);
                    buildingDataDefinition = item.Building.Definition;
                }
                
                if (buildingMemoryDataSO != null)
                {
                    buildingImage.sprite = buildingMemoryDataSO.Sprite;
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

        public void UpdateContent(TooltipVisual tooltipVisual)
        {
            if (trackedItem.Unit != null && unitMemoryDataSO != null)
            {
                unitMemoryDataSO.FillTooltipContent(trackedItem.Unit, unitTooltipContent);
                tooltipVisual.AddContent(unitTooltipContent);
            }

            if (trackedItem.Building != null && buildingMemoryDataSO != null)
            {
                buildingMemoryDataSO.FillTooltipContent(trackedItem.Building, buildingTooltipContent);
                tooltipVisual.AddContent(buildingTooltipContent);
            }
        }
    }
}
