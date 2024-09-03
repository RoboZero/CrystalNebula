using System;
using System.Collections.Generic;
using Source.Interactions;
using Source.Logic;
using UnityEngine;

namespace Source.Visuals
{
    public class MemoryItemVisual : StandardInteractableVisual
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
                    tmpText.text = trackedDataItem?.Text ?? "";
                    break;
                case InteractVisualState.Hovered:
                    tmpText.text = "HOVERED";
                    break;
                case InteractVisualState.Interacted:
                    tmpText.text = "SELECTED";
                    
                    // TODO: Determine how to sync visual interact state with data properly
                    // After a transfer event, visual should reset. 
                    if (trackedDataItem != null)
                        trackedDataItem.Text = "LOL";
                    else
                        ResetState();

                    break;
            }
        }
    }
}
