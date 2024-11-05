using Source.Interactions;
using Source.Utility;
using UnityEngine;

namespace Source.Logic.State
{
    public class PlayerInteractions : MonoBehaviour
    {
        public ContinuousCollection<IInteractableVisual> Hovered;
        public ContinuousCollection<IInteractableVisual> Interacted;

        private void Awake()
        {
            Hovered = new ContinuousCollection<IInteractableVisual>(
                r => r.TryEnterState(InteractVisualState.Hovered), 
                r => r.TryEnterState(InteractVisualState.Hovered),
                (r) => r.TryExitState(InteractVisualState.Hovered),
                null
                );
            Interacted = new ContinuousCollection<IInteractableVisual>(
                r => r.TryEnterState(InteractVisualState.Selected),
                null,
                null,
                r => r.ResetState()
                );
        }
    }
}
