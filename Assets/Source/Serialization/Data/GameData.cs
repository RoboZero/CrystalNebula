using System.Collections.Generic;

namespace Source.Serialization.Data
{
    public class GameData
    {
        public LevelData Level;
        public List<PlayerData> Players;
        public BattlefieldStorageData BattlefieldStorage;
    }
}