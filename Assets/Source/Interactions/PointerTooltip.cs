using System.Linq;
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

        private ContinuousCollection<ITooltipTarget> tooltipTargets;

        private void Start()
        {
            tooltipTargets = new ContinuousCollection<ITooltipTarget>(
                (target) =>
                {
                    DOVirtual.DelayedCall(delayUntilShow, () =>
                    {
                        Debug.Log($"Pointer Tooltip target: {target} NOW SHOWING");

                        tooltipVisual.RemoveAllContent();
                        foreach(var content in target.GetContent())
                            tooltipVisual.AddContent(content);
                    });
                },
                null,
                (target) =>
                {
                    tooltipVisual.RemoveAllContent();
                },
                null
            );
        }

        private void Update()
        {
            var raycastResults = RaycastUIFromPointer();
            var targets = raycastResults
                .Select(result => result.gameObject.GetComponent<ITooltipTarget>())
                .Where(result => result != null)
                .ToList();
            
            tooltipTargets.Tick(targets);
        }
    }
}