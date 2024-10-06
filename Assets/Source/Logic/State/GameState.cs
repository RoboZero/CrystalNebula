using Source.Logic.Data;

namespace Source.Logic.State
{
    public class GameState
    {
        public LevelData Level;
        public PlayerData[] Players;
        public BattlefieldData BattlefieldStorage;
    }
}