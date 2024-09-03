namespace Source.Interactions
{
    public enum InteractState
    {
        None,
        Hovered,
        Interacted,
        Held
    }
    
    public interface IInteractable
    {
        public InteractState CurrentState { get; }
        public bool TryEnterState(InteractState nextState);
        public bool TryExitState(InteractState fromState);
        public void ResetState();
    }
}
