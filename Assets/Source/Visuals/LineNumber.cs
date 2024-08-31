using UnityEngine;

namespace Source.Visuals
{
    public class LineNumber : MonoBehaviour
    {
        public int Value { get; set; }
        
        [SerializeField] private TMPro.TMP_Text tmpText;

        void Update()
        {
            tmpText.text = Value.ToString();
        }
    }
}
