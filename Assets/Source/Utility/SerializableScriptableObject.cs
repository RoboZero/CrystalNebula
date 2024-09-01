using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace Source.Utility
{
    public class SerializableScriptableObject : ScriptableObject
    {
        [SerializeField, HideInInspector] private string guid;
        public string Guid => guid;

#if UNITY_EDITOR
        private void OnValidate()
        {
            var path = AssetDatabase.GetAssetPath(this);
            guid = AssetDatabase.AssetPathToGUID(path);
        }
#endif
    }
}