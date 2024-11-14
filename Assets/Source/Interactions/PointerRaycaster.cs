using System.Collections.Generic;
using System.Linq;
using Source.Input;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Source.Interactions
{
    public class PointerRaycaster : MonoBehaviour
    {
        [Header("Raycaster Dependencies")]
        [SerializeField] protected InputReaderSO inputReader;
        
        [Header("Raycaster Settings")]
        [SerializeField] private LayerMask interactableMask;
        
        private Vector2 pointerPosition;
        
        protected virtual void OnEnable()
        {
            inputReader.PointerPositionEvent += OnPointerPosition;
        }

        protected virtual void OnDisable()
        {
            inputReader.PointerPositionEvent -= OnPointerPosition;
        }
        
        private void OnPointerPosition(Vector2 position, bool isMouse) => pointerPosition = position;


        protected IEnumerable<RaycastResult> RaycastUIFromPointer()
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