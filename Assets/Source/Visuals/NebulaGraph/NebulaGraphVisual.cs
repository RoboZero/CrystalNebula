using System;
using System.Collections.Generic;
using Source.Interactions;
using Source.Logic.State.ResearchGraphs;
using Source.Serialization;
using Source.Visuals.Levels;
using Source.Visuals.MemoryStorage;
using UnityEngine;

namespace Source.Visuals.NebulaGraph
{
    public class NebulaGraphVisual : StandardInteractableVisual
    {
        [Header("Dependencies")]
        [SerializeField] private GameResources gameResources;
        [SerializeField] private Transform startingLocation;
        [SerializeField] private Transform itemGroup;
        [SerializeField] private Transform connectorGroup;
        [SerializeField] private NebulaGraphBehavior nebulaGraphBehavior;
        [SerializeField] private NebulaGraphItemVisual nebulaGraphItemVisualPrefab;
        [SerializeField] private NebulaGraphConnectorVisual nebulaGraphConnectorVisualPrefab;

        [Header("Settings")]
        [SerializeField] private float maxProgress = 50;
        [SerializeField] private float minConnectorDistance = 10;
        [SerializeField] private float maxConnectorDistance = 30;
        [SerializeField] private float maxRotationAngle = 270;

        private ResearchGraph trackedResearchGraph;
        private List<NebulaGraphItemVisual> nebulaGraphItemVisuals = new();
        private List<NebulaGraphConnectorVisual> nebulaGraphConnectorVisuals = new();

        public void SetGraph(ResearchGraph researchGraph)
        {
            if (researchGraph != trackedResearchGraph && researchGraph != null)
            {
                GenerateGraphVisual(researchGraph);
            }
            
            trackedResearchGraph = researchGraph;
        }

        private void Start()
        {
            var position = startingLocation.position;
            itemGroup.position = position;
            connectorGroup.position = position;
        }

        private void Update()
        {
            // TODO: Visual should not update behavior, could be updated multiple times per frame. 
            nebulaGraphBehavior.Tick();
            SetGraph(nebulaGraphBehavior.State);

            switch (CurrentVisualState)
            {
                case InteractVisualState.None: 
                    break;
                case InteractVisualState.Hovered:
                    break;
                case InteractVisualState.Selected:
                    break;
            }
        }

        private void GenerateGraphVisual(ResearchGraph researchGraph)
        {
            var definitionToPosition = new Dictionary<string, Vector3>();
            var definitionToMemoryData = new Dictionary<string, MemoryDataSO>();
            var definitionSet = new HashSet<string>();
            var definitionQueue = new Queue<string>();

            if (researchGraph.StartingDefinition == null)
            {
                Debug.LogWarning($"Cannot generate Nebula graph. Starting Definition {researchGraph.StartingDefinition} is NULL");
                return;
            }
            
            definitionQueue.Enqueue(researchGraph.StartingDefinition);
            definitionToPosition.Add(researchGraph.StartingDefinition, Vector3.zero);

            while (definitionQueue.Count > 0)
            {
                var definition = definitionQueue.Dequeue();
                var position = definitionToPosition[definition];

                var visual = CreateNebulaItemVisual(definition, position, definitionToMemoryData);
                nebulaGraphItemVisuals.Add(visual);

                if (!researchGraph.Edges.TryGetValue(definition, out var researchEdges)) continue;

                var sectors = researchEdges.Count + 1;
                var moveDirection = startingLocation.right;
                var rotationAngle = maxRotationAngle / sectors;
                
                for (var index = 0; index < researchEdges.Count; index++)
                {
                    var researchEdge = researchEdges[index];
                    if (definitionSet.Contains(researchEdge.Definition)) continue;

                    var memory = GetMemory(researchEdge.Definition, definitionToMemoryData);
                    var instance = memory.CreateDefaultInstance(nebulaGraphBehavior.PlayerId, researchEdge.Definition);
                    var height = Mathf.Lerp(minConnectorDistance, maxConnectorDistance, instance.MaxRunProgress / maxProgress);
                    var offset = Quaternion.AngleAxis(rotationAngle * (index + 1), Vector3.forward) * moveDirection * height;
                    var offsetPosition = position + offset;
                    definitionToPosition.Add(researchEdge.Definition, offsetPosition);

                    var connectorVisual = CreateNebulaConnectorVisual(position, offsetPosition);
                    nebulaGraphConnectorVisuals.Add(connectorVisual);
                    
                    definitionQueue.Enqueue(researchEdge.Definition);
                    definitionSet.Add(researchEdge.Definition);
                }
            }
        }

        private NebulaGraphItemVisual CreateNebulaItemVisual(string definition, Vector3 localPosition, Dictionary<string, MemoryDataSO> definitionToMemoryData)
        {
            var visual = Instantiate(nebulaGraphItemVisualPrefab, itemGroup);
            visual.transform.localPosition = localPosition;
            if (gameResources.TryLoadAsset(this, nebulaGraphBehavior.Level.Definition, out LevelDataSO levelDataSO))
            {
                visual.SetLevel(nebulaGraphBehavior.Level, levelDataSO);
            }

            var memory = GetMemory(definition, definitionToMemoryData);
            if (memory != null)
            {
                var instance = memory.CreateDefaultInstance(nebulaGraphBehavior.PlayerId, definition);
                visual.SetMemoryItem(instance, memory);
            }
            
            return visual;
        }

        private MemoryDataSO GetMemory(string definition, Dictionary<string, MemoryDataSO> definitionToMemoryData)
        {
            if (gameResources.TryLoadAsset(this, definition, out MemoryDataSO memoryDataSO))
            {
                definitionToMemoryData.TryAdd(definition, memoryDataSO);
                return memoryDataSO;
            }
            
            return null;
        }

        private NebulaGraphConnectorVisual CreateNebulaConnectorVisual(Vector3 fromPosition, Vector3 toPosition)
        {
            var distance = Vector3.Magnitude(toPosition - fromPosition);
            var direction = Vector3.Normalize(toPosition - fromPosition);
            var visual = Instantiate(nebulaGraphConnectorVisualPrefab, connectorGroup);
            visual.transform.localPosition = fromPosition;
            visual.SetHeight(distance);
            visual.transform.up = direction;
            return visual;
        }
    }
}
