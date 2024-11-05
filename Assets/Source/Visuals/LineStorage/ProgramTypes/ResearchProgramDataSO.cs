using Source.Logic.State.LineItems;
using Source.Logic.State.LineItems.Programs;
using Source.Serialization.Data;
using UnityEngine;

namespace Source.Visuals.LineStorage.ProgramTypes
{
    [CreateAssetMenu(fileName = "ResearchProgramLineItem", menuName = "Game/Memory Item/Research Program")]
    public class ResearchProgramDataSO : ProgramDataSO
    {
        public override Memory CreateMemoryInstance(MemoryData memoryData)
        {
            return new ResearchProgram()
            {
                OwnerId = memoryData.OwnerId,
                Definition = memoryData.Definition,
                CurrentProgress = memoryData.Progress,
                MaxProgress = this.MaxProgress,
            };
        }
    }
}