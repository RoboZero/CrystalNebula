using System.Collections.Generic;
using System.Linq;
using Source.Input;
using Source.Logic;
using Source.Logic.Events;
using Source.Logic.State;
using Source.Utility;
using Source.Visuals;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Source.Interactions
{
    public class PointerInteractor : PointerRaycaster
    {
        [Header("Dependencies")]
        [SerializeField] private PlayerInteractions playerInteractions;

        protected override void OnEnable()
        {
            base.OnEnable();
            inputReader.InteractPressedEvent += OnInteractPressed;
            inputReader.InteractReleasedEvent += OnInteractReleased;
            inputReader.InteractCanceledEvent += OnInteractCanceled;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            inputReader.InteractPressedEvent -= OnInteractPressed;
            inputReader.InteractReleasedEvent -= OnInteractReleased;
            inputReader.InteractCanceledEvent -= OnInteractCanceled;
        }
        
        private void OnInteractPressed() => inputReader.ClickAndDrag = true;
        private void OnInteractReleased() => inputReader.ClickAndDrag = false;
        private void OnInteractCanceled()
        { 
            playerInteractions.Interacted.Clear();
        }

        private void Update()
        {
            var raycastResults = RaycastUIFromPointer();
            var allInteractables = raycastResults
                .Select(result => result.gameObject.GetComponent<IInteractableVisual>())
                .Where(result => result != null)
                .ToList();

            var hoveredIsSame = playerInteractions.Hovered.Tick(allInteractables);
            var interactedIsSame = true;
            if(inputReader.ClickAndDrag)
                interactedIsSame = playerInteractions.Interacted.Tick(allInteractables);
            
            if(!hoveredIsSame)
                Debug.Log("Pointer hovering over" + allInteractables.ToItemString());
            if(!interactedIsSame)
                Debug.Log("Pointer interacting with" + allInteractables.ToItemString());
        }
    }
}
