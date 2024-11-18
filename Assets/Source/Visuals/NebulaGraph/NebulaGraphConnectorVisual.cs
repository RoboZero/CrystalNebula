using Source.Interactions;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Visuals
{
    public class NebulaGraphConnectorVisual : StandardInteractableVisual
    {
        [Header("Dependencies")]
        [SerializeField] private Image iconImage;
        
        private void Update()
        {
            switch (CurrentVisualState)
            {
                case InteractVisualState.None:
                    iconImage.color = Color.grey;
                    break;
                case InteractVisualState.Hovered:
                    iconImage.color = Color.yellow;
                    break;
                case InteractVisualState.Selected:
                    iconImage.color = Color.cyan;
                    break;
            }
        }
    }
}
