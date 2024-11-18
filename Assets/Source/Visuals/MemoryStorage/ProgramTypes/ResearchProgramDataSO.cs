using Source.Logic.Events.Overrides;
using Source.Logic.State.LineItems;
using Source.Logic.State.LineItems.Programs;
using Source.Serialization;
using Source.Serialization.Data;
using UnityEngine;

namespace Source.Visuals.MemoryStorage.ProgramTypes
{
    [CreateAssetMenu(fileName = "ResearchProgramLineItem", menuName = "Game/Memory Item/Research Program")]
    public class ResearchProgramDataSO : ProgramDataSO
    {
        [Header("Research Program Dependencies")]
        public GameResources GameResources;
        public CreateMemoryEventOverrides CreateMemoryEventOverrides;
        
        public override MemoryItem CreateDefaultInstance(int ownerId, string definition)
        {
            return new ResearchProgram()
            {
                OwnerId = ownerId,
                Definition = definition,
                CurrentRunProgress = 0,
                MaxRunProgress = MaxProgress,
                DataSize = DataSize,
                GameResources = GameResources,
                CreateMemoryEventOverrides = CreateMemoryEventOverrides
            };
        }

        public override MemoryItem CreateMemoryInstance(MemoryData memoryData)
        {
            var instance = (ResearchProgram) CreateDefaultInstance(memoryData.OwnerId, memoryData.Definition);
            return instance;
        }
    }
}