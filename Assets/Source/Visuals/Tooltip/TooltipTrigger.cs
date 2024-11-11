using UnityEngine;
using UnityEngine.EventSystems;

namespace Source.Visuals.Tooltip
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TooltipContent tooltipContent;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            TooltipManager.Show(tooltipContent);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipManager.Hide();
        }
    }
}