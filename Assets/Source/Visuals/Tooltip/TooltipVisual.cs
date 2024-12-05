using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

// Useful Reference: https://www.youtube.com/watch?v=HXFoUGw7eKk
namespace Source.Visuals.Tooltip
{
    [ExecuteInEditMode]
    public class TooltipVisual : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private InputSystemUIInputModule inputSystemUIInputModule;
        [SerializeField] private RectTransform tooltipTransform;
        [SerializeField] private LayoutElement layoutElement;
        [SerializeField] private Image background;
        [SerializeField] private TooltipSubVisual tooltipSubVisualPrefab;

        [Header("Settings")]
        [SerializeField] private float percentScreenWidthToAdjust = 0.5f;
        [SerializeField] private float percentScreenHeightToAdjust = 0.5f;
        [SerializeField] private float fadeInTweenTime = 0.5f;
        [SerializeField] private float fadeOutTweenTime = 0.2f;
        [SerializeField] private float pivotTweenTimeX = 0.5f;
        [SerializeField] private float pivotTweenTimeY = 1f;

        private List<TooltipSubVisual> tooltipSubVisuals = new();
        private HashSet<TooltipContent> tooltipContents = new();
        private Sequence moveSequence;
        private Vector2 MousePositionUI => inputSystemUIInputModule.input.mousePosition;

        private bool isShowing;
        private Tween hideTween;
        private Tween showTween;

        public void AddContent(TooltipContent tooltipContent)
        {
            if (tooltipContent == null) return;
            
            var success = tooltipContents.Add(tooltipContent);

            if (success)
            {
                UpdateVisibility();
            }
        }

        public bool RemoveContent(TooltipContent tooltipContent)
        {
            var success = tooltipContents.Remove(tooltipContent);
            
            if (success)
            {
                UpdateVisibility();
            }
            return success;
        }

        public void RemoveAllContent()
        {
            tooltipContents.Clear();
            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            //Debug.Log($"Tooltip contents count: {tooltipContents.Count}");
            if (tooltipContents.Count > 0)
            {
                UpdateContent();
                if (isShowing) return;
                
                ShowTween();
                isShowing = true;
            }
            else
            {
                if (!isShowing) return;
                
                HideTween();
                isShowing = false;
            }
        }
        
        private void UpdateContent()
        {
            for (var i = tooltipSubVisuals.Count - 1; i >= 0; i--)
            {
                Destroy(tooltipSubVisuals[i].gameObject);
                tooltipSubVisuals.RemoveAt(i);
            }

            foreach (var tooltipContent in tooltipContents)
            {
                var subTooltip = Instantiate(tooltipSubVisualPrefab, layoutElement.transform);
                subTooltip.SetContent(tooltipContent);
                tooltipSubVisuals.Add(subTooltip);
            }

            Resize();
        }
        
        private void ShowTween()
        {
            showTween?.Kill();
            hideTween?.Complete();

            var contentIndex = 0;
            foreach (var tooltipContent in tooltipContents)
            {
                tooltipSubVisuals[contentIndex].Show();
                contentIndex++;
            }

            background.enabled = true;
            showTween = background.DOFade(1, fadeInTweenTime);
        }

        private void HideTween()
        {
            Debug.Log("Hiding");
            showTween?.Kill();
            hideTween?.Kill();
            
            foreach (var tooltipSubVisual in tooltipSubVisuals)
            {
                tooltipSubVisual.Hide();
            }

            hideTween = background.DOFade(0, fadeOutTweenTime)
                .OnComplete(() =>
                {
                    background.enabled = false;
                });
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
            var layoutEnabled = false;
            
            foreach (var tooltipSubVisual in tooltipSubVisuals)
            {
                if (!tooltipSubVisual.WithinLayoutPreferredBounds(layoutElement))
                {
                    layoutEnabled = true;
                }
            }

            layoutElement.enabled = layoutEnabled;
        }
    }
}
