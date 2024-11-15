using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Source.Utility;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

namespace Source.Serialization
{
    [CreateAssetMenu(fileName = "GameResources", menuName = "Game/GameResources")]
    public class GameResources : DescriptionBaseSO
    {
        private static readonly string TRUE_PROJECT_PATH = Application.dataPath;
        private static readonly string RELATIVE_PROJECT_PATH = "Assets/";
        private static readonly string RELATIVE_RESOURCE_PATH = "/Content/Resources/";
        private static string TrueResourcePath => TRUE_PROJECT_PATH + RELATIVE_RESOURCE_PATH;
        
        [ReadOnly]
        [SerializeField] private List<DefinitionToResource> resourceList = new();

        private Dictionary<string, DescriptionBaseSO> definitionToResource;
        private Dictionary<DescriptionBaseSO, string> resourceToDefinition;

        //TODO: Consider whether to ignore empty definitions
        public bool TryLoadAsset<T>(object loader, string definition, out T asset)
        {
            // TODO: Consider iterative creation for large resource lists.
            if (definitionToResource == null){
                definitionToResource = new Dictionary<string, DescriptionBaseSO>();
                resourceToDefinition = new Dictionary<DescriptionBaseSO, string>();

                foreach (var pair in resourceList)
                {
                    definitionToResource.Add(pair.Definition, pair.Resource);
                    resourceToDefinition.Add(pair.Resource, pair.Definition);
                }
            }
            
            if (definition == "")
            {
                Debug.Log($"Loader {loader} could not find asset with empty definition. ");
                asset = default;
                return false;
            }

            if (!definitionToResource.TryGetValue(definition, out var resource))
            {
                
                Debug.LogWarning($"Loader {loader} is unable to find resource with definition {definition}");
                asset = default;
                return false;
            }
            
            if (resource is not T resourceTyped)
            {
                Debug.LogWarning($"Loader {loader} is unable to load resource: {definition}");
                asset = default;
                return false;
            }

            Debug.Log($"Loader {loader} found asset of type {resourceTyped.GetType()}");
            asset = resourceTyped;
            return true;
        }

        public bool TryLoadDefinition(object loader, DescriptionBaseSO resource, out string definition)
        {
            if (!resourceToDefinition.TryGetValue(resource, out definition))
            {
                Debug.LogWarning($"Loader {loader} is unable to load definition from resource {resource}");
                definition = "";
                return false;
            }

            return true;
        }
        
        public static string BuildDefinitionPath(string directoryName, string assetNameNoExtension)
        {
            return $"{directoryName}/{assetNameNoExtension}";
        }
        
        // TODO: Switch to AssetBundles for mods: https://stackoverflow.com/questions/46106849/unity3d-assetdatabase-loadassetatpath-vs-resource-load
#if UNITY_EDITOR
        [ContextMenu("Load assets")]
        private void LoadAssets()
        {
            resourceList.Clear();

            Debug.Log($"Loading resources at project path: {TRUE_PROJECT_PATH}");

            foreach (var directoryPath in Directory.EnumerateDirectories(TrueResourcePath))
            {
                foreach (var assetPath in Directory.EnumerateFiles(directoryPath, "*.asset", SearchOption.TopDirectoryOnly))
                {
                    var directoryName = Path.GetFileName(directoryPath);
                    var assetNameNoExtension = Path.GetFileNameWithoutExtension(assetPath);
                    var localPath = RELATIVE_PROJECT_PATH + Path.GetRelativePath(RELATIVE_PROJECT_PATH, assetPath);

                    Debug.Log($"Local Path: {localPath}");
                    var definitionName = BuildDefinitionPath(directoryName, assetNameNoExtension);
                    var resource = AssetDatabase.LoadAssetAtPath<DescriptionBaseSO>(localPath);
                    
                    resourceList.Add(new DefinitionToResource
                    {
                        Definition = definitionName,
                        Resource = resource
                    });
                    
                    definitionToResource.Add(definitionName, resource);
                    resourceToDefinition.Add(resource, definitionName);
                }
            }
        }
#endif
        [Serializable]
        public class DefinitionToResource
        {
            public string Definition;
            public DescriptionBaseSO Resource;
        }
    }
}
