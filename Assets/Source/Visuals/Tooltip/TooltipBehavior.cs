using System.Collections.Generic;
using DG.Tweening;
using Source.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

// Useful Reference: https://www.youtube.com/watch?v=HXFoUGw7eKk
namespace Source.Visuals.Tooltip
{
    [ExecuteInEditMode]
    public class TooltipBehavior : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private InputSystemUIInputModule inputSystemUIInputModule;
        [SerializeField] private RectTransform tooltipTransform;
        [SerializeField] private Image background;
        [SerializeField] private TextMeshProUGUI header;
        [SerializeField] private TextMeshProUGUI content;
        [SerializeField] private LayoutElement layoutElement;

        [Header("Settings")]
        [SerializeField] private float percentScreenWidthToAdjust = 0.5f;
        [SerializeField] private float percentScreenHeightToAdjust = 0.5f;
        [SerializeField] private float fadeInTweenTime = 0.5f;
        [SerializeField] private float fadeOutTweenTime = 0.2f;
        [SerializeField] private float pivotTweenTimeX = 0.5f;
        [SerializeField] private float pivotTweenTimeY = 1f;

        private ContinuousCollection<TooltipContent> t;
        private HashSet<TooltipContent> tooltipContents = new();
        private Sequence moveSequence;
        private Vector2 MousePositionUI => inputSystemUIInputModule.input.mousePosition;
        
        public void AddContent(TooltipContent tooltipContent)
        {
            if (tooltipContent != null)
                tooltipContents.Add(tooltipContent);
            
            if (tooltipContents.Count > 0)
                Show();
        }

        public bool RemoveContent(TooltipContent tooltipContent)
        {
            var result = tooltipContents.Remove(tooltipContent);
            if (tooltipContents.Count == 0)
                Hide();
            return result;
        }

        public void RemoveAllContent()
        {
            if (tooltipContents.Count == 0) return;
            tooltipContents.Clear();
            Hide();
        }

        private void Show()
        {
            foreach (var tooltipContent in tooltipContents)
            {
                header.text = tooltipContent.Header;
                content.text = tooltipContent.Content;
                
                if(string.IsNullOrEmpty(tooltipContent.Header))
                    header.gameObject.SetActive(false);
                if(string.IsNullOrEmpty(tooltipContent.Content))
                    content.gameObject.SetActive(false);
            }

            background.gameObject.SetActive(true);
            header.gameObject.SetActive(true);
            content.gameObject.SetActive(true);
            Resize();
            
            header.DOFade(1, fadeInTweenTime);
            content.DOFade(1, fadeInTweenTime);
            background.DOFade(1, fadeInTweenTime);
        }

        private void Hide()
        {
            header.DOFade(0, fadeOutTweenTime);
            content.DOFade(0, fadeOutTweenTime);
            background.DOFade(0, fadeOutTweenTime);
            
            header.gameObject.SetActive(false);
            content.gameObject.SetActive(false);
        }
        
        private void Update()
        { 
#if UNITY_EDITOR
            Resize();
#endif

            var position = MousePositionUI;
            var pivotX = position.x / Screen.width;
            var pivotY = position.y / Screen.height;
            
            float finalPivotX;
            float finalPivotY;
            
            //If mouse on left of screen move tooltip to right of cursor and vice vera
            if (pivotX < percentScreenWidthToAdjust) 
                finalPivotX = -0.1f;
            else
                finalPivotX = 1.01f;
            
            //If mouse on lower half of screen move tooltip above cursor and vice versa
            if (pivotY < percentScreenHeightToAdjust) 
                finalPivotY = 0;
            else
                finalPivotY = 1;

            var finalPivot = new Vector2(finalPivotX, finalPivotY);
            
            if (tooltipTransform.pivot != finalPivot)
            {
                moveSequence.Kill();
                moveSequence = DOTween.Sequence()
                    .Join(DOTween.To(() => tooltipTransform.pivot, x => tooltipTransform.pivot = x, new Vector2(finalPivotX, finalPivotY), pivotTweenTimeX))
                    .Join(DOTween.To(() => tooltipTransform.pivot, y => tooltipTransform.pivot = y, new Vector2(finalPivotX, finalPivotY), pivotTweenTimeY))
                    .SetRelative(false)
                    .Play();
            }
            
            transform.position = position;
        }

        private void Resize()
        {
            layoutElement.enabled = header.preferredWidth > layoutElement.preferredWidth || 
                                    content.preferredWidth > layoutElement.preferredWidth;
        }
    }
}
