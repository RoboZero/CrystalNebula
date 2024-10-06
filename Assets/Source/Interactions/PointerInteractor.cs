using System.Collections.Generic;
using System.Linq;
using Source.Input;
using Source.Logic;
using Source.Logic.Events;
using Source.Utility;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Source.Interactions
{
    public class PointerInteractor : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private EventTracker eventTracker;
        [SerializeField] private PlayerInteractions playerInteractions;
        [SerializeField] private InputReaderSO inputReader;
        
        [Header("Settings")]
        [SerializeField] private LayerMask interactableMask;

        private Vector2 pointerPosition;
        private bool clickAndDrag;

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
            playerInteractions.Interacted.Clear();
        }

        void Update()
        {
            var raycastResults = RaycastUIFromPointer();
            var allInteractables = raycastResults
                .Select(result => result.gameObject.GetComponent<IInteractableVisual>())
                .Where(result => result != null)
                .ToList();
            
            Debug.Log("All interactables pointer is over: " + allInteractables.ToItemString());
            
            playerInteractions.Hovered.Tick(allInteractables);
            if(clickAndDrag)
                playerInteractions.Interacted.Tick(allInteractables);
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
