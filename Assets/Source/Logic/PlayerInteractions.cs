using System;
using Source.Interactions;
using Source.Utility;
using UnityEngine;

namespace Source.Logic
{
    public class PlayerInteractions : MonoBehaviour
    {
        public ContinuousCollection<IInteractable> Hovered;
        public ContinuousCollection<IInteractable> Interacted;
        public ContinuousCollection<IHoldableItem> Held;

        private void Start()
        {
            Hovered = new ContinuousCollection<IInteractable>(
                r => r.TryEnterState(InteractState.Hovered), 
                r => r.TryEnterState(InteractState.Hovered),
                (r) => r.TryExitState(InteractState.Hovered),
                null
                );
            Interacted = new ContinuousCollection<IInteractable>(
                r => r.TryEnterState(InteractState.Interacted),
                null,
                null,
                r => r.ResetState()
                );
            Held = new ContinuousCollection<IHoldableItem>(
                r => r.TryEnterState(InteractState.Held), 
                null,
                r => r.TryExitState(InteractState.Held),
                r => r.ResetState()
                );
        }
    }
}
