using System;
using System.Collections.Generic;
using Source.Logic.Data;

namespace Source.Logic.State
{
    [Serializable]
    public class GameState
    {
        public LevelData Level;
        public List<PlayerData> Players;
        public BattlefieldStorageData BattlefieldStorage;
    }
}