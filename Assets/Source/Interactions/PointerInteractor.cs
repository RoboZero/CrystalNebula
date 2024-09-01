using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Source.Input;
using Source.Visuals;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Source.Interactions
{
    public class Pointer : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader;
        [SerializeField] private LayerMask interactableMask;

        private ContinuousInteractableCollection hoveredInteractables;
        private ContinuousInteractableCollection interactedInteractables;
        private Vector2 pointerPosition;
        private bool clickAndDrag = false;

        private void OnEnable()
        {
            inputReader.PointerPositionEvent += OnPointerPosition;
            inputReader.InteractPressedEvent += OnInteractPressed;
            inputReader.InteractReleasedEvent += OnInteractReleased;
        }

        private void OnDisable()
        {
            inputReader.PointerPositionEvent -= OnPointerPosition;
            inputReader.InteractPressedEvent -= OnInteractPressed;
            inputReader.InteractReleasedEvent -= OnInteractReleased;
        }
        
        private void OnPointerPosition(Vector2 position, bool isMouse) => pointerPosition = position;
        private void OnInteractPressed() => clickAndDrag = true;
        private void OnInteractReleased() => clickAndDrag = false;

        private void Start()
        {
            hoveredInteractables = new ContinuousInteractableCollection(
                r => r.TryEnterState(InteractState.Hovered), 
                (r) => r.TryExitState(InteractState.Hovered)
                );
            interactedInteractables = new ContinuousInteractableCollection(
                r => r.TryEnterState(InteractState.Interacted),
                null
                );
        }

        // Update is called once per frame
        void Update()
        {
            var raycastResults = RaycastUIFromPointer();
            var allInteractables = raycastResults.Select(result => result.gameObject.GetComponent<IInteractable>()).ToList();
            hoveredInteractables.Tick(allInteractables);
            if(clickAndDrag)
                interactedInteractables.Tick(allInteractables);
        }

        private IEnumerable<RaycastResult> RaycastUIFromPointer()
        {
            var eventData = new PointerEventData(EventSystem.current)
            {
                position = pointerPosition
            };
            
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            var resultsInLayer = results.Where(r => ((1 << r.gameObject.layer) & interactableMask) != 0);
            return resultsInLayer;
        }

        private class ContinuousInteractableCollection
        {
            private List<IInteractable> newInteractables = new();
            private List<IInteractable> oldInteractables = new();
            [CanBeNull] private Action<IInteractable> enterAction;
            [CanBeNull] private Action<IInteractable> exitAction;

            public ContinuousInteractableCollection(Action<IInteractable> enterAction, Action<IInteractable> exitAction)
            {
                this.enterAction = enterAction;
                this.exitAction = exitAction;
            }
            
            public void Tick(List<IInteractable> allInteractables)
            {
                newInteractables = allInteractables;

                if (enterAction != null)
                {
                    foreach (var interactable in newInteractables.Except(oldInteractables))
                    {
                        enterAction(interactable);
                    }
                }

                if (exitAction != null)
                {
                    foreach (var interactable in oldInteractables.Except(newInteractables))
                    {
                        exitAction(interactable);
                    }
                }

                oldInteractables = newInteractables;
            }
        }
    }
}
