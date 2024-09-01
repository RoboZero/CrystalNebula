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

        private List<RaycastResult> raycastResults;
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
            foreach(var result in raycastResults)
            {
                Debug.Log(result.gameObject.name);
                if (result.gameObject.TryGetComponent<DataItem>(out var dataItem))
                {
                    dataItem.Select();
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            raycastResults = RaycastUIFromPointer();
        }

        private List<RaycastResult> RaycastUIFromPointer()
        {
            var eventData = new PointerEventData(EventSystem.current)
            {
                position = pointerPosition
            };
            
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            var resultsInLayer = results.Where(r => ((1 << r.gameObject.layer) & interactableMask) != 0).ToList();
            return resultsInLayer;
        }
    }
}
