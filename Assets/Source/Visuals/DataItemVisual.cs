using System;
using System.Collections.Generic;
using Source.Interactions;
using Source.Logic;
using UnityEngine;

namespace Source.Visuals
{
    public class DataItemVisual : StandardInteractable
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
            switch (CurrentState)
            {
                case InteractState.None:
                    tmpText.text = trackedDataItem.Text;
                    break;
                case InteractState.Hovered:
                    tmpText.text = "HOVERED";
                    break;
                case InteractState.Interacted:
                    tmpText.text = "SELECTED";
                    break;
            }
        }
    }
}
