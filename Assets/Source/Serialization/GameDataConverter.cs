using Source.Logic.State;
using Source.Serialization.Data;

namespace Source.Serialization
{
    public class GameDataConverter
    {
        public GameState Convert(GameData gameData, GameResources gameResources)
        {
            var converter = new GameConvertDataToState(gameResources);

            return converter.Convert(gameData);
        }

        public GameData Convert(GameState gameState)
        {
            var converter = new GameConvertStateToData();

            return converter.Convert(gameState);
        }
    }
}