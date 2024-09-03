using System;
using Source.Input;
using Source.Logic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Visuals
{
    public class PlayerItemStorageVisual : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private PlayerInteractions playerInteractions;
        [SerializeField] private InputReader inputReader;
        [SerializeField] private RectTransform backgroundRectTransform;
        [SerializeField] private LayoutGroup storedItemsLayoutGroup;

        private void OnEnable()
        {
            inputReader.PointerPositionEvent += OnPointerPosition;
        }

        private void OnDisable()
        {
            inputReader.PointerPositionEvent -= OnPointerPosition;
        }

        private void OnPointerPosition(Vector2 position, bool isMouse)
        {
            if(RectTransformUtility.ScreenPointToWorldPointInRectangle(backgroundRectTransform, position, Camera.main, out var worldPoint))
                transform.position = new Vector3(worldPoint.x, worldPoint.y, 0);
        }

        // Update is called once per frame
        void Update()
        {
            /*float paddingSize = 0f;
            Vector2 backgroundSize = new Vector2(
                storedItemsLayoutGroup.preferredWidth + paddingSize * 2,
                storedItemsLayoutGroup.preferredHeight + paddingSize * 2
            );
            backgroundRectTransform.sizeDelta = backgroundSize;
            */
        }
    }
}
