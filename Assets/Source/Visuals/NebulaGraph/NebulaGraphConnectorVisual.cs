using Source.Interactions;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Visuals.NebulaGraph
{
    public class NebulaGraphConnectorVisual : StandardInteractableVisual
    {
        [Header("Dependencies")]
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Image iconImage;

        public void SetHeight(float height)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
        }
        
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
