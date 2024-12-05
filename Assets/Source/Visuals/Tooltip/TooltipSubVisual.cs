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

        private Sequence showSequence;
        private Sequence hideSequence;

        public void SetContent(TooltipContent nextTooltipContent)
        {
            if (nextTooltipContent == null)
            {
                tooltipContent = null;
                Hide();
                return;
            }
            
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
            hideSequence.Complete();
            
            icon.gameObject.SetActive(tooltipContent.Icon != null);
            header.gameObject.SetActive(!string.IsNullOrEmpty(tooltipContent.Header));
            stats.gameObject.SetActive(tooltipContent.Stats.Count != 0);
            description.gameObject.SetActive(!string.IsNullOrEmpty(tooltipContent.Description));

            showSequence = DOTween.Sequence()
                .Join(icon.DOFade(1, fadeInTweenTime))
                .Join(header.DOFade(1, fadeInTweenTime))
                .Join(stats.DOFade(1, fadeInTweenTime))
                .Join(description.DOFade(1, fadeInTweenTime))
                .Play();
        }

        public void Hide()
        {
            showSequence.Kill();
            
            hideSequence = DOTween.Sequence()
                .Join(icon.DOFade(0, fadeOutTweenTime).OnComplete(() => { icon.gameObject.SetActive(false); }))
                .Join(header.DOFade(0, fadeOutTweenTime).OnComplete(() => { header.gameObject.SetActive(false); }))
                .Join(stats.DOFade(0, fadeOutTweenTime).OnComplete(() => { stats.gameObject.SetActive(false); }))
                .Join(description.DOFade(0, fadeOutTweenTime).OnComplete(() => { description.gameObject.SetActive(false); }))
                .Play();
        }

        public bool WithinLayoutPreferredBounds(LayoutElement layoutElement)
        {
            return header.preferredWidth < layoutElement.preferredWidth &&
                   stats.preferredWidth < layoutElement.preferredWidth &&
                   description.preferredWidth < layoutElement.preferredWidth;
        }
    }
}