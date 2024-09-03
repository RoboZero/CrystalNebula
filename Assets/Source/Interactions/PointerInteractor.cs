using System.Collections.Generic;
using System.Linq;
using Source.Input;
using Source.Utility;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Source.Interactions
{
    public class PointerInteractor : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader;
        [SerializeField] private LayerMask interactableMask;

        private ContinuousCollection<IInteractable> hovered;
        private ContinuousCollection<IInteractable> interacted;
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
            interacted.Clear();
        }

        private void Start()
        {
            hovered = new ContinuousCollection<IInteractable>(
                r => r.TryEnterState(InteractState.Hovered), 
                r => r.TryEnterState(InteractState.Hovered),
                (r) => r.TryExitState(InteractState.Hovered),
                null
                );
            interacted = new ContinuousCollection<IInteractable>(
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
            hovered.Tick(allInteractables);
            if(clickAndDrag)
                interacted.Tick(allInteractables);
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
