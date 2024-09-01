using System;
using System.Collections.Generic;
using System.Linq;
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

        private List<IInteractable> hoveredInteractables = new();
        private List<IInteractable> previouslyHoveredInteractables = new();
        private Vector2 pointerPosition;

        private void OnEnable()
        {
            inputReader.PointerPositionEvent += OnPointerPosition;
            inputReader.InteractEvent += OnInteractPressed;
        }

        private void OnDisable()
        {
            inputReader.PointerPositionEvent -= OnPointerPosition;
            inputReader.InteractEvent -= OnInteractPressed;
        }
        
        private void OnPointerPosition(Vector2 position, bool isMouse) => pointerPosition = position;

        private void OnInteractPressed()
        {
            foreach(var interactable in hoveredInteractables)
            {
                interactable.Interact();
            }
        }

        // Update is called once per frame
        void Update()
        {
            var raycastResults = RaycastUIFromPointer();
            hoveredInteractables = raycastResults.Select(result => result.gameObject.GetComponent<IInteractable>()).ToList();
            Debug.Log(hoveredInteractables);
            
            foreach (var interactable in hoveredInteractables.Except(previouslyHoveredInteractables))
            {
                interactable.EnterHover();
                //Debug.Log($"{interactable} Enter");
            }

            foreach (var interactable in previouslyHoveredInteractables.Except(hoveredInteractables))
            {
                interactable.ExitHover();
                //Debug.Log($"{interactable} Exit");
            }

            previouslyHoveredInteractables = hoveredInteractables;
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
    }
}
