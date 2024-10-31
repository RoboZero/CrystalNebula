using System;
using System.Collections.Generic;
using Source.Logic.Data;

namespace Source.Logic.State
{
    [Serializable]
    public class GameState
    {
        public Level Level;
        public List<Player> Players;
        public BattlefieldStorage BattlefieldStorage;
    }
}