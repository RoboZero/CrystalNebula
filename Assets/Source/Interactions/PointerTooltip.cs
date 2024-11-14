using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Source.Utility;
using Source.Visuals.Tooltip;
using UnityEngine;

namespace Source.Interactions
{
    public class PointerTooltip : PointerRaycaster
    {
        [Header("Dependencies")]
        [SerializeField] private TooltipVisual tooltipVisual;
        
        [Header("Settings")]
        [SerializeField] private float delayUntilShow = 0.5f;
        
        private Tween showTween;
        
        private void Update()
        {
            var raycastResults = RaycastUIFromPointer();
            var target = raycastResults
                .Select(result => result.gameObject.GetComponent<ITooltipTarget>())
                .FirstOrDefault(result => result != null);

            if (target != null && showTween == null)
            {
                Debug.Log($"Pointer Tooltip target: {target}");
                showTween = DOVirtual.DelayedCall(delayUntilShow, () =>
                {
                    target.UpdateContent(tooltipVisual);
                });
            }
            else if (target == null && showTween != null)
            {
                showTween.Kill();
                showTween = null;
                tooltipVisual.RemoveAllContent();
            }
        }
    }
}