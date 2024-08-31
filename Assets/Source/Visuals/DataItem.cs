using System;
using UnityEngine;

namespace Source.Visuals
{
    public class DataItem : MonoBehaviour
    {
        [SerializeField] private String text;
        [SerializeField] private TMPro.TMP_Text tmpText;

        void Update()
        {
            tmpText.text = text;
        }

        public void Select()
        {
            text = "Selected";
        }
    }
}
