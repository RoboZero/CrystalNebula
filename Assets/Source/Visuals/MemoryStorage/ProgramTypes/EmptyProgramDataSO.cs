using Source.Logic.State.LineItems;
using Source.Logic.State.LineItems.Programs;
using Source.Serialization.Data;
using UnityEngine;

namespace Source.Visuals.MemoryStorage.ProgramTypes
{
    [CreateAssetMenu(fileName = "EmptyProgramLineItem", menuName = "Game/Memory Item/Empty Program")]
    public class EmptyProgramDataSO : ProgramDataSO
    {
        public override MemoryItem CreateMemoryInstance(MemoryData memoryData)
        {
            return new EmptyProgram();
        }
    }
}