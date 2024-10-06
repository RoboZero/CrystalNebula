using Source.Interactions;
using Source.Logic;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Visuals.Battlefield
{
    public class BattlefieldItemVisual : StandardInteractableVisual
    {
        [SerializeField] private Image buildingImage;
        [SerializeField] private Image unitImage;
        [SerializeField] private TMPro.TMP_Text healthText;
        [SerializeField] private TMPro.TMP_Text powerText;
        [SerializeField] private TMPro.TMP_Text utilityText;

        private BattlefieldDataItem trackedDataItem;
        private string originalText;

        // TODO: Pass scriptable objects to visuals with more data, updates in real time. 
        public void SetDataItem(in BattlefieldDataItem dataItem)
        {
            trackedDataItem = dataItem;
        }

        private void Update()
        {
            buildingImage.gameObject.SetActive(false);
            unitImage.gameObject.SetActive(false);
            healthText.gameObject.SetActive(false);
            powerText.gameObject.SetActive(false);
            
            var isTracking = trackedDataItem != null;
            
            if (isTracking)
            {
                if (trackedDataItem.UnitData != null)
                {
                    unitImage.sprite = trackedDataItem.UnitData.Sprite;
                    healthText.text = trackedDataItem.UnitData.Health.ToString();
                    powerText.text = trackedDataItem.UnitData.Power.ToString();
                    unitImage.gameObject.SetActive(true);
                    healthText.gameObject.SetActive(true);
                    powerText.gameObject.SetActive(true);
                }

                if (trackedDataItem.BuildingData != null)
                {
                    buildingImage.sprite = trackedDataItem.BuildingData.Sprite;
                    buildingImage.gameObject.SetActive(true);
                    healthText.gameObject.SetActive(true);
                    powerText.gameObject.SetActive(true);
                }
            }

            switch (CurrentVisualState)
            {
                case InteractVisualState.None:
                    utilityText.text = trackedDataItem != null && trackedDataItem.UnitData != null ? trackedDataItem.UnitData.Name : "";
                    break;
                case InteractVisualState.Hovered:
                    utilityText.text = "STATE: HOVERED";
                    break;
                case InteractVisualState.Selected:
                    utilityText.text = "STATE: SELECTED";
                    break;
            }
        }
    }
}
