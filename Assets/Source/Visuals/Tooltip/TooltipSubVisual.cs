using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Visuals.Tooltip
{
    public class TooltipSubVisual : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI header;
        [SerializeField] private TextMeshProUGUI stats;
        [SerializeField] private TextMeshProUGUI description;

        [Header("Settings")]
        [SerializeField] private float fadeInTweenTime = 0.5f;
        [SerializeField] private float fadeOutTweenTime = 0.2f;

        private TooltipContent tooltipContent;

        public void SetContent(TooltipContent nextTooltipContent)
        {
            tooltipContent = nextTooltipContent;
            icon.sprite = nextTooltipContent.Icon;
            header.text = tooltipContent.Header;
            stats.text = "";
            foreach (var stat in tooltipContent.Stats)
            {
                stats.text += $"{stat.Name}: {stat.Value}\n";
            }
            description.text = tooltipContent.Description;
        }

        public void Show()
        {
            icon.gameObject.SetActive(tooltipContent.Icon != null);
            header.gameObject.SetActive(!string.IsNullOrEmpty(tooltipContent.Header));
            stats.gameObject.SetActive(tooltipContent.Stats.Count != 0);
            description.gameObject.SetActive(!string.IsNullOrEmpty(tooltipContent.Description));
            
            icon.DOFade(1, fadeInTweenTime);
            header.DOFade(1, fadeInTweenTime);
            stats.DOFade(1, fadeInTweenTime);
            description.DOFade(1, fadeInTweenTime);
        }

        public void Hide()
        {
            icon.DOFade(0, fadeOutTweenTime).OnComplete(() => { icon.gameObject.SetActive(false); });
            header.DOFade(0, fadeOutTweenTime).OnComplete(() => { header.gameObject.SetActive(false); });
            stats.DOFade(0, fadeOutTweenTime).OnComplete(() => { stats.gameObject.SetActive(false); });
            description.DOFade(0, fadeOutTweenTime).OnComplete(() => { description.gameObject.SetActive(false); });
        }

        public bool WithinLayoutPreferredBounds(LayoutElement layoutElement)
        {
            return header.preferredWidth < layoutElement.preferredWidth &&
                   stats.preferredWidth < layoutElement.preferredWidth &&
                   description.preferredWidth < layoutElement.preferredWidth;
        }
    }
}