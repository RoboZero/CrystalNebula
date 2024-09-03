using System;
using System.Collections.Generic;
using Source.Utility;
using Unity.VisualScripting;
using UnityEngine;

namespace Source.Interactions
{
    public abstract class StandardInteractable : MonoBehaviour, IInteractable
    {
        public InteractState CurrentState => interactStates.Peek();
        private List<InteractState> interactStates = new();

        private void Awake()
        {
            interactStates.Add(InteractState.None);
        }

        public bool TryEnterState(InteractState nextState)
        {
            switch (CurrentState)
            {
                case InteractState.None:
                    if (nextState == InteractState.Hovered)
                        interactStates.Add(InteractState.Hovered);
                    return true;
                case InteractState.Hovered:
                    if (nextState == InteractState.Interacted)
                        interactStates.Add(InteractState.Interacted);
                    return true;
            }

            return false;
        }

        public bool TryExitState(InteractState fromState)
        {
            if (CurrentState != fromState) return false;
            
            switch (CurrentState)
            {
                case InteractState.Hovered:
                    interactStates.TryRemoveLast();
                    return true;
                case InteractState.Interacted:
                    interactStates.TryRemoveLast();
                    return true;
            }

            return false;
        }

        public void ResetState()
        {
            interactStates.Clear();
            interactStates.Add(InteractState.None);
        }
    }
}
