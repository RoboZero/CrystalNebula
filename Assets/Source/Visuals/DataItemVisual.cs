using System;
using System.Collections.Generic;
using Source.Interactions;
using Source.Logic;
using Source.Logic.State;
using UnityEngine;

namespace Source.Visuals
{
    [Serializable]
    public class DataItemVisual : StandardInteractableVisual
    {
        [SerializeField] private TMPro.TMP_Text tmpText;

        private LineItem trackedLineItem;
        private string originalText;

        public void SetDataItem(in LineItem lineItem)
        {
            trackedLineItem = lineItem;
        }

        private void Update()
        {
            switch (CurrentVisualState)
            {
                case InteractVisualState.None:
                    tmpText.text = trackedLineItem?.Description ?? "NONE";
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
