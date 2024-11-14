using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Visuals.Tooltip
{
    /*
     * Singleton as there's no reason to have multiple tooltips in this case.
     */
    public class TooltipManager : MonoBehaviour
    {
        public static TooltipManager Current;

        [Header("Dependencies")]
        [SerializeField] private TooltipVisual tooltip;

        private void Awake()
        {
            Current = this;
        }

        public static void AddContent(TooltipContent tooltipContent)
        {
            Current.tooltip.AddContent(tooltipContent);
        }

        public static void RemoveContent(TooltipContent tooltipContent)
        {
            Current.tooltip.RemoveContent(tooltipContent);
        }
    }
}