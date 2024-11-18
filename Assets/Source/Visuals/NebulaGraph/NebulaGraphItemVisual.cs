using Source.Interactions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Visuals
{
    public class NebulaGraphItemVisual : StandardInteractableVisual
    {
        [Header("Dependencies")]
        [SerializeField] private Image iconImage;
        
        private void Update()
        {
            switch (CurrentVisualState)
            {
                case InteractVisualState.None: 
                    iconImage.color = Color.white;
                    break;
                case InteractVisualState.Hovered:
                    iconImage.color = Color.yellow;
                    break;
                case InteractVisualState.Selected:
                    iconImage.color = Color.magenta;
                    break;
            }
        }
    }
}
