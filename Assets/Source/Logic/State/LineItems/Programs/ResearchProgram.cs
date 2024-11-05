using Source.Logic.Events;
using UnityEngine;

namespace Source.Logic.State.LineItems.Programs
{
    public class ResearchProgram : Memory
    {
        protected override void Run(EventTracker eventTracker, GameState gameState)
        {
            Debug.Log("Research ran!");
        }
    }
}