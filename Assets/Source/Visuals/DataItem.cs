using System;
using UnityEngine;

namespace Source.Visuals
{
    public class DataItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private String text;
        [SerializeField] private TMPro.TMP_Text tmpText;

        private String originalText;

        private void Start()
        {
            originalText = text;
        }

        void Update()
        {
            tmpText.text = text;
        }

        public void EnterHover()
        {
            text = "HOVER";
        }

        public void ExitHover()
        {
            text = originalText;
        }

        public void Interact()
        {
            text = "SELECTED";
        }
    }
}
