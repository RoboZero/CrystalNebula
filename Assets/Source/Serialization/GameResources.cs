using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Source.Utility;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

namespace Source.Serialization
{
    [CreateAssetMenu(fileName = "GameResources", menuName = "Game/GameResources")]
    public class GameResources : DescriptionBaseSO
    {
        public Dictionary<string, Object> Resources => resources;
        
        private Dictionary<string, Object> resources = new();

        [SerializeField] private List<IdToResource> resourceList = new();
        
        [ContextMenu("Load assets")]
        private void Load()
        {
            string resourcesPath = Application.dataPath + "/Content/Resources/Buildings/";
            Debug.Log($"Application path: {resourcesPath}, files: {Directory.GetFiles(resourcesPath).ToList().ToItemString()}");
            /*foreach (var pair in resourceList)
            {
                resources.Add(pair.id, pair.resource);
            }*/
        }

        [Serializable]
        public class IdToResource
        {
            public string id;
            public Object resource;
        }
    }
}
