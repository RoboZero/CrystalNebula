using UnityEngine;

namespace Source.Utility
{
    public class DescriptionBaseSO : SerializableScriptableObject
    {
        [TextArea] public string description;
    }
}
