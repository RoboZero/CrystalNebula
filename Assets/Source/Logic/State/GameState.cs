using System;
using Source.Logic.Data;

namespace Source.Logic.State
{
    [Serializable]
    public class GameState
    {
        public LevelData Level;
        public PlayerData[] Players;
        public BattlefieldStorageData BattlefieldStorage;
    }
}