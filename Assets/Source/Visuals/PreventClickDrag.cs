using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Source.Visuals
{
    public class PreventClickDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler 
    {
        [SerializeField] private ScrollRect scrollRect;

        public void OnBeginDrag(PointerEventData data) {
            // do your stuff here
            this.scrollRect.StopMovement();
            scrollRect.enabled = false;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            scrollRect.enabled = true;
        }
    }
}
