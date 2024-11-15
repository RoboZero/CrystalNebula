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

        public override MemoryItem CreateDefaultInstance(int ownerId, string definition)
        {
            return new CommandProgram()
            {
                OwnerId = ownerId,
                Definition = definition,
                CurrentRunProgress = 0,
                MaxRunProgress = MaxProgress,
                DataSize = DataSize,
                Distance = Distance,
                Direction = Direction
            };
        }

        public override MemoryItem CreateMemoryInstance(MemoryData memoryData)
        {
            var instance = (CommandProgram) CreateDefaultInstance(memoryData.OwnerId, memoryData.Definition);
            instance.CurrentRunProgress = memoryData.Progress;
            return instance;
        }
    }
}