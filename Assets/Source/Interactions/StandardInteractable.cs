using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Source.Interactions
{
    public abstract class StandardInteractable : MonoBehaviour, IInteractable
    {
        public InteractState CurrentState => interactStates.Peek();
        private readonly Stack<InteractState> interactStates = new();

        private void Awake()
        {
            interactStates.Push(InteractState.None);
        }

        public bool TryEnterState(InteractState nextState)
        {
            switch (CurrentState)
            {
                case InteractState.None:
                    if (nextState == InteractState.Hovered)
                        interactStates.Push(InteractState.Hovered);
                    return true;
                case InteractState.Hovered:
                    if (nextState == InteractState.Interacted)
                        interactStates.Push(InteractState.Interacted);
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
                    interactStates.Pop();
                    return true;
                case InteractState.Interacted:
                    interactStates.Pop();
                    return true;
            }

            return false;
        }
    }
}
