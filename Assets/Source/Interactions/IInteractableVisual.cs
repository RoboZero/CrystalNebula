using System;

namespace Source.Interactions
{
    public enum InteractVisualState
    {
        None,
        Hovered,
        Selected
    }
    
    public interface IInteractableVisual
    {
        public InteractVisualState CurrentVisualState { get; }
        public bool TryEnterState(InteractVisualState nextVisualState);
        public bool TryExitState(InteractVisualState fromVisualState);
        public void ResetState();
    }
}
