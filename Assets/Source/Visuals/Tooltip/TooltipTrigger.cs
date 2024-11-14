using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Source.Visuals.Tooltip
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TooltipContent tooltipContent;

        private HashSet<TooltipContent> tooltipContents = new();

        public void OnPointerEnter(PointerEventData eventData)
        {
            tooltipContents.Add(tooltipContent);
            TooltipManager.Show(tooltipContents);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tooltipContents.Remove(tooltipContent);
            TooltipManager.Hide();
        }
    }
}