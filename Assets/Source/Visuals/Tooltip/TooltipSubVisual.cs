using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Visuals.Tooltip
{
    public class TooltipSubVisual : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private TextMeshProUGUI header;
        [SerializeField] private TextMeshProUGUI content;
        
        [Header("Settings")]
        [SerializeField] private float fadeInTweenTime = 0.5f;
        [SerializeField] private float fadeOutTweenTime = 0.2f;

        private TooltipContent tooltipContent;

        public void SetContent(TooltipContent nextTooltipContent)
        {
            tooltipContent = nextTooltipContent;
            header.text = tooltipContent.Header;
            content.text = tooltipContent.Content;
            
            if(string.IsNullOrEmpty(tooltipContent.Header))
                header.gameObject.SetActive(false);
            if(string.IsNullOrEmpty(tooltipContent.Content))
                content.gameObject.SetActive(false);
        }

        public void Show()
        {
            header.DOFade(1, fadeInTweenTime);
            content.DOFade(1, fadeInTweenTime);
            
            header.gameObject.SetActive(true);
            content.gameObject.SetActive(true);
        }

        public void Hide()
        {
            header.DOFade(0, fadeOutTweenTime);
            content.DOFade(0, fadeOutTweenTime);
            
            header.gameObject.SetActive(false);
            content.gameObject.SetActive(false);
        }

        public bool WithinLayoutPreferredBounds(LayoutElement layoutElement)
        {
            return header.preferredWidth < layoutElement.preferredWidth &&
                   content.preferredWidth < layoutElement.preferredWidth;
        }
    }
}