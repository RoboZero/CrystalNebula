using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

// Useful Reference: https://www.youtube.com/watch?v=HXFoUGw7eKk
namespace Source.Visuals.Tooltip
{
    [ExecuteInEditMode]
    public class TooltipBehavior : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private InputSystemUIInputModule inputSystemUIInputModule;
        [SerializeField] private RectTransform tooltipTransform;
        [SerializeField] private Image background;
        [SerializeField] private TextMeshProUGUI header;
        [SerializeField] private TextMeshProUGUI content;
        [SerializeField] private LayoutElement layoutElement;

        [Header("Settings")]
        [SerializeField] private float fadeInTweenTime = 0.5f;
        [SerializeField] private float fadeOutTweenTime = 0.2f;
        [SerializeField] private float pivotTweenTimeX = 0.5f;
        [SerializeField] private float pivotTweenTimeY = 1f;

        private TooltipContent tooltipContent;
        private Sequence moveSequence;
        private Vector2 MousePositionUI => inputSystemUIInputModule.input.mousePosition;

        public void SetContent(TooltipContent nextContent)
        {
            tooltipContent = nextContent;

            if (tooltipContent == null) return;
            
            header.text = tooltipContent.Header;
            content.text = tooltipContent.Content;
            
            Resize();
        }

        public void Show()
        {
            if (tooltipContent == null) return;
            
            if (content == null)
            {
                background.gameObject.SetActive(false);
                header.gameObject.SetActive(false);
                content.gameObject.SetActive(false);
            }
            
            if(string.IsNullOrEmpty(tooltipContent.Header))
                header.gameObject.SetActive(false);
            if(string.IsNullOrEmpty(tooltipContent.Content))
                content.gameObject.SetActive(false);

            background.gameObject.SetActive(true);
            header.gameObject.SetActive(true);
            content.gameObject.SetActive(true);
            
            header.DOFade(1, fadeInTweenTime);
            content.DOFade(1, fadeInTweenTime);
            background.DOFade(1, fadeInTweenTime);
        }

        public void Hide()
        {
            header.DOFade(0, fadeOutTweenTime);
            content.DOFade(0, fadeOutTweenTime);
            background.DOFade(0, fadeOutTweenTime);
            
            header.gameObject.SetActive(false);
            content.gameObject.SetActive(false);
        }
        
        private void Update()
        { 
#if UNITY_EDITOR
            Resize();
#endif

            var position = MousePositionUI;
            var pivotX = position.x / Screen.width;
            var pivotY = position.y / Screen.height;
            
            float finalPivotX;
            float finalPivotY;
            
            //If mouse on left of screen move tooltip to right of cursor and vice vera
            if (pivotX < 0.5) 
                finalPivotX = -0.1f;
            else
                finalPivotX = 1.01f;
            
            //If mouse on lower half of screen move tooltip above cursor and vice versa
            if (pivotY < 0.5) 
                finalPivotY = 0;
            else
                finalPivotY = 1;

            var finalPivot = new Vector2(finalPivotX, finalPivotY);
            
            if (tooltipTransform.pivot != finalPivot)
            {
                moveSequence.Kill();
                moveSequence = DOTween.Sequence()
                    .Join(DOTween.To(() => tooltipTransform.pivot, x => tooltipTransform.pivot = x, new Vector2(finalPivotX, finalPivotY), pivotTweenTimeX))
                    .Join(DOTween.To(() => tooltipTransform.pivot, y => tooltipTransform.pivot = y, new Vector2(finalPivotX, finalPivotY), pivotTweenTimeY))
                    .SetRelative(false)
                    .Play();
            }
            
            transform.position = position;
        }

        private void Resize()
        {
            layoutElement.enabled = header.preferredWidth > layoutElement.preferredWidth || 
                                    content.preferredWidth > layoutElement.preferredWidth;
        }
    }
}
