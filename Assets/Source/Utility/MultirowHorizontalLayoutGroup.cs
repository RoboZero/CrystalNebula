using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Utility
{
    [RequireComponent(typeof(VerticalLayoutGroup))]
    public class MultirowHorizontalLayoutGroup : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private HorizontalLayoutGroup layoutGroupPrefab;

        [Header("Settings")]
        [SerializeField] private List<float> maxWidthPattern;

        private float CurrentMaxWidth => maxWidthPattern[(layoutGroupWidths.Count - 1) % maxWidthPattern.Count];
        
        private readonly List<HorizontalLayoutGroup> horizontalLayoutGroups = new();
        private readonly List<float> layoutGroupWidths = new();

        public bool TryAddUIElement(RectTransform element)
        {
            var elementWidth = element.rect.width;
            // Debug.Log("Element width: " + elementWidth);

            if (maxWidthPattern.Count == 0)
            {
                return false;
            }

            if (layoutGroupWidths.Count == 0)
            {
                layoutGroupWidths.Add(0);
                horizontalLayoutGroups.Add(Instantiate(layoutGroupPrefab, this.transform));
            }

            if (elementWidth > CurrentMaxWidth)
            {
                return false;
            }

            var groupWidth = layoutGroupWidths.Peek();
            if (groupWidth + elementWidth <= CurrentMaxWidth)
            { 
                element.SetParent(horizontalLayoutGroups.Peek().transform);
                layoutGroupWidths[^1] += elementWidth;
            }
            else
            {
                layoutGroupWidths.Add(elementWidth);
                horizontalLayoutGroups.Add(Instantiate(layoutGroupPrefab, this.transform));
                element.SetParent(horizontalLayoutGroups.Peek().transform);
            }
            
            return true;
        }
    }
}