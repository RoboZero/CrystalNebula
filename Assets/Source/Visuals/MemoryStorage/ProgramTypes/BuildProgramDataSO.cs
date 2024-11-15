using Source.Logic.Events.Overrides;
using Source.Logic.State.LineItems;
using Source.Logic.State.LineItems.Programs;
using Source.Serialization;
using Source.Serialization.Data;
using UnityEngine;

namespace Source.Visuals.MemoryStorage.ProgramTypes
{
    [CreateAssetMenu(fileName = "BuildProgramLineItem", menuName = "Game/Memory Item/Build Program")]
    public class BuildProgramDataSO : ProgramDataSO
    {
        [Header("Building Program Dependencies")]
        public GameResources GameResources;
        public MemoryDataSO MemoryDataSO;
        public CreateMemoryEventOverrides CreateMemoryEventOverrides;

        public override MemoryItem CreateDefaultInstance(int ownerId, string definition)
        {
            if (!GameResources.TryLoadDefinition(this, MemoryDataSO, out var createdDefinition))
            {
                createdDefinition = "";
            }
            
            return new BuildProgram()
            {
                OwnerId = ownerId,
                Definition = definition,
                CurrentRunProgress = 0,
                MaxRunProgress = MaxProgress,
                DataSize = DataSize,
                MemoryItem = MemoryDataSO.CreateDefaultInstance(ownerId, createdDefinition),
                CreateMemoryEventOverrides = CreateMemoryEventOverrides
            };
        }

        public override MemoryItem CreateMemoryInstance(MemoryData memoryData)
        {
            var instance = CreateDefaultInstance(memoryData.OwnerId, memoryData.Definition);
            instance.CurrentRunProgress = memoryData.Progress;
            return instance;
        }
    }
}