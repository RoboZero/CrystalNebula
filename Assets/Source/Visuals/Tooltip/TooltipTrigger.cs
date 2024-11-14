using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Source.Visuals.Tooltip
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Dependencies")]
        [SerializeField] private TooltipContent tooltipContent;

        public void OnPointerEnter(PointerEventData eventData)
        {
            TooltipManager.AddContent(tooltipContent);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipManager.RemoveContent(tooltipContent);
        }
    }
}