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
            inputReader.InteractCanceledEvent += OnInteractCanceled;
        }

        private void OnDisable()
        {
            inputReader.PointerPositionEvent -= OnPointerPosition;
            inputReader.InteractPressedEvent -= OnInteractPressed;
            inputReader.InteractReleasedEvent -= OnInteractReleased;
            inputReader.InteractCanceledEvent -= OnInteractCanceled;
        }
        
        private void OnPointerPosition(Vector2 position, bool isMouse) => pointerPosition = position;
        private void OnInteractPressed() => clickAndDrag = true;
        private void OnInteractReleased() => clickAndDrag = false;
        private void OnInteractCanceled()
        {
            interactedInteractables.Clear();
        }

        private void Start()
        {
            hoveredInteractables = new ContinuousInteractableCollection(
                r => r.TryEnterState(InteractState.Hovered), 
                r => r.TryEnterState(InteractState.Hovered),
                (r) => r.TryExitState(InteractState.Hovered),
                null
                );
            interactedInteractables = new ContinuousInteractableCollection(
                r => r.TryEnterState(InteractState.Interacted),
                null,
                null,
                r => r.ResetState()
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
            private readonly HashSet<IInteractable> storedInteractables = new();
            private List<IInteractable> newInteractables = new();
            private List<IInteractable> oldInteractables = new();
            [CanBeNull] private readonly Action<IInteractable> enterAction;
            [CanBeNull] private readonly Action<IInteractable> stayAction;
            [CanBeNull] private readonly Action<IInteractable> exitAction;
            [CanBeNull] private readonly Action<IInteractable> resetAction;

            public ContinuousInteractableCollection(Action<IInteractable> enterAction, 
                Action<IInteractable> stayAction,
                Action<IInteractable> exitAction, 
                Action<IInteractable> resetAction)
            {
                this.enterAction = enterAction;
                this.stayAction = stayAction;
                this.exitAction = exitAction;
                this.resetAction = resetAction;
            }
            
            public void Tick(List<IInteractable> allInteractables)
            {
                newInteractables = allInteractables;

                if (stayAction != null)
                {
                    foreach (var interactable in storedInteractables)
                    {
                        stayAction(interactable);
                    }
                }
                
                if (enterAction != null)
                {
                    foreach (var interactable in newInteractables.Except(oldInteractables))
                    {
                        enterAction(interactable);
                        storedInteractables.Add(interactable);
                    }
                }

                if (exitAction != null)
                {
                    foreach (var interactable in oldInteractables.Except(newInteractables))
                    {
                        exitAction(interactable);
                        storedInteractables.Remove(interactable);
                    }
                }

                oldInteractables = newInteractables;
            }

            public void Clear()
            {
                if (resetAction != null)
                {
                    foreach (var interactable in storedInteractables)
                    {
                        Debug.Log($"Reset {interactable}");
                        resetAction(interactable);
                    }
                }
                
                storedInteractables.Clear();
                oldInteractables.Clear();
                newInteractables.Clear();
            }
        }
    }
}
