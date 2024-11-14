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
        [SerializeField] private TooltipBehavior tooltip;

        private void Awake()
        {
            Current = this;
        }

        public static void Show(IEnumerable<TooltipContent> tooltipContent)
        {
            foreach (var tooltip in tooltipContent)
            {
                Current.tooltip.SetContent(tooltip);
                Current.tooltip.Show();
            }
        }

        public static void Hide()
        {
            Current.tooltip.Hide();
        }
    }
}