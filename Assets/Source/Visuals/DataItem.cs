using System;
using System.Collections.Generic;
using Source.Interactions;
using UnityEngine;

namespace Source.Visuals
{
    public class DataItem : StandardInteractable
    {
        [SerializeField] private string text;
        [SerializeField] private TMPro.TMP_Text tmpText;

        private string originalText;

        private void Start()
        {
            originalText = text;
        }

        private void Update()
        {
            tmpText.text = text;

            switch (CurrentState)
            {
                case InteractState.None:
                    text = originalText;
                    break;
                case InteractState.Hovered:
                    text = "HOVERED";
                    break;
                case InteractState.Interacted:
                    text = "SELECTED";
                    break;
            }
        }
    }
}
