using System;
using System.Collections.Generic;
using Source.Logic.Events;
using Source.Logic.State.Battlefield;
using Source.Logic.State.LineItems;

namespace Source.Logic.State
{
    [Serializable]
    public class GameState
    {
        public Level Level;
        public List<Player> Players;
        public LineStorage<BattlefieldItem> BattlefieldStorage;
    }
}