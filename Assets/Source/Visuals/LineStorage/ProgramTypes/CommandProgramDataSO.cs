using Source.Logic.State.LineItems;
using Source.Logic.State.LineItems.Programs;
using Source.Serialization.Data;
using UnityEngine;

namespace Source.Visuals.LineStorage.ProgramTypes
{
    [CreateAssetMenu(fileName = "CommandProgramLineItem", menuName = "Game/Memory Item/Command Program")]
    public class CommandProgramDataSO : ProgramDataSO
    {
        public override Memory CreateMemoryInstance(MemoryData memoryData)
        {
            return new CommandProgram()
            {
                OwnerId = memoryData.OwnerId,
                Definition = memoryData.Definition,
                CurrentProgress = memoryData.Progress,
                MaxProgress = this.MaxProgress,
            };
        }
    }
}