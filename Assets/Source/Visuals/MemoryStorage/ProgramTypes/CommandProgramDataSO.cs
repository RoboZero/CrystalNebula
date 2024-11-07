using Source.Logic.Events;
using Source.Logic.State.LineItems;
using Source.Logic.State.LineItems.Programs;
using Source.Serialization.Data;
using UnityEngine;

namespace Source.Visuals.MemoryStorage.ProgramTypes
{
    [CreateAssetMenu(fileName = "CommandProgramLineItem", menuName = "Game/Memory Item/Command Program")]
    public class CommandProgramDataSO : ProgramDataSO
    {
        public int Distance;
        public MoveUnitsInDirectionEventCommand.Direction Direction;

        public override MemoryItem CreateMemoryInstance(MemoryData memoryData)
        {
            return new CommandProgram()
            {
                OwnerId = memoryData.OwnerId,
                Definition = memoryData.Definition,
                CurrentProgress = memoryData.Progress,
                MaxProgress = MaxProgress,
                Distance = Distance,
                Direction = Direction
            };
        }
    }
}