using System;
using System.Collections.Generic;

namespace Source.Logic.State
{
    [Serializable]
    public class GameState
    {
        public Level Level;
        public Dictionary<int, Player> Players;
        public BattlefieldStorage BattlefieldStorage;
    }
}