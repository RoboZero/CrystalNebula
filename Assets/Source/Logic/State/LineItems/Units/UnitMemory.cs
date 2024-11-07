using Source.Logic.Events;
using UnityEngine;

namespace Source.Logic.State.LineItems.Units
{
    public class UnitLineItem : MemoryItem
    {
        protected override void Run(EventTracker eventTracker, GameState gameState)
        {
            Debug.Log("Unit ran!");
        }
    }
}