using System;
using System.Collections.Generic;
using Source.Logic;
using Source.Utility;
using UnityEngine;

namespace Source.Interactions
{
    public abstract class StandardInteractableVisual : MonoBehaviour, IInteractableVisual
    {
        public InteractVisualState CurrentVisualState => interactStates.Peek();
        private List<InteractVisualState> interactStates = new();

        private void Awake()
        {
            interactStates.Add(InteractVisualState.None);
        }

        public bool TryEnterState(InteractVisualState nextVisualState)
        {
            switch (CurrentVisualState)
            {
                case InteractVisualState.None:
                    if (nextVisualState == InteractVisualState.Hovered)
                        interactStates.Add(InteractVisualState.Hovered);
                    return true;
                case InteractVisualState.Hovered:
                    if (nextVisualState == InteractVisualState.Interacted)
                        interactStates.Add(InteractVisualState.Interacted);
                    return true;
            }

            return false;
        }

        public bool TryExitState(InteractVisualState fromVisualState)
        {
            if (CurrentVisualState != fromVisualState) return false;
            
            switch (CurrentVisualState)
            {
                case InteractVisualState.Hovered:
                    interactStates.TryRemoveLast();
                    return true;
                case InteractVisualState.Interacted:
                    interactStates.TryRemoveLast();
                    return true;
            }

            return false;
        }

        public void ResetState()
        {
            interactStates.Clear();
            interactStates.Add(InteractVisualState.None);
        }
    }
}
