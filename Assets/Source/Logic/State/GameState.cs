using System;
using System.Collections.Generic;
using Source.Logic.State.Battlefield;
using Source.Logic.State.LineItems;
using Source.Logic.State.ResearchGraphs;

namespace Source.Logic.State
{
    [Serializable]
    public class GameState
    {
        public Level Level;
        public List<Player> Players;
        public LineStorage<BattlefieldItem> BattlefieldStorage;
        //public List<EventCommand> RunningEventCommands;
    }
}