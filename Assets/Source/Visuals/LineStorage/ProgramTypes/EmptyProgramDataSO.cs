using Source.Logic.State.LineItems;
using Source.Serialization.Data;
using UnityEngine;

namespace Source.Visuals.LineStorage.ProgramTypes
{
    [CreateAssetMenu(fileName = "EmptyProgramLineItem", menuName = "Game/Memory Item/Empty Program")]
    public class EmptyProgramDataSO : ProgramDataSO
    {
        public override Memory CreateMemoryInstance(MemoryData memoryData)
        {
            return new Memory();
        }
    }
}