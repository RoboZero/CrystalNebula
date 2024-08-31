using System;
using UnityEngine;

namespace Source.Visuals
{
    public class DataItem : MonoBehaviour
    {
        [SerializeField] private String text;
        [SerializeField] private TMPro.TMP_Text tmpText;

        // Update is called once per frame
        void Update()
        {
            tmpText.text = text;
        }
    }
}
