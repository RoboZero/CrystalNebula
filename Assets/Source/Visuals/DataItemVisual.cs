using System;
using System.Collections.Generic;
using Source.Interactions;
using Source.Logic;
using Source.Logic.Data;
using UnityEngine;

namespace Source.Visuals
{
    [Serializable]
    public class DataItemVisual : StandardInteractableVisual
    {
        [SerializeField] private TMPro.TMP_Text tmpText;

        private DataItem trackedDataItem;
        private string originalText;

        public void SetDataItem(in DataItem dataItem)
        {
            trackedDataItem = dataItem;
        }

        private void Update()
        {
            switch (CurrentVisualState)
            {
                case InteractVisualState.None:
                    tmpText.text = trackedDataItem?.Description ?? "NONE";
                    break;
                case InteractVisualState.Hovered:
                    tmpText.text = "HOVERED";
                    break;
                case InteractVisualState.Selected:
                    tmpText.text = "SELECTED";
                    break;
            }
        }
    }
}
