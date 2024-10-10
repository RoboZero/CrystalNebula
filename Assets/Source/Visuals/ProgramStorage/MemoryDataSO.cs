using Source.Utility;
using UnityEngine;

namespace Source.Visuals.ProgramStorage
{
    [CreateAssetMenu(fileName = "MemoryItemName", menuName = "Game/MemoryItem")]
    public class MemoryDataSO : DescriptionBaseSO
    {
        public Sprite Icon;
        public string Name;
    }
}