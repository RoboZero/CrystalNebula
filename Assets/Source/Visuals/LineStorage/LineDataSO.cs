using Source.Utility;
using UnityEngine;

namespace Source.Visuals.LineStorage
{
    [CreateAssetMenu(fileName = "LineItemName", menuName = "Game/LineItem")]
    public class LineDataSO : DescriptionBaseSO
    {
        public Sprite Icon;
        public string Name;
    }
}