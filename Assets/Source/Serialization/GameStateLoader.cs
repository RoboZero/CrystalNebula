using System;
using System.Text;
using Source.Logic.State;
using Source.Serialization.Data;
using UnityEngine;

namespace Source.Serialization
{
    public class GameStateLoader : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private TextAsset gameStateJsonAsset;

        public GameState GameState => gameState;

        [SerializeField] private GameState gameState;
        private GameData gameData;
        private readonly JsonDataService jsonDataService = new();

        private void Awake()
        {
            Load(gameStateJsonAsset);
        }

        public void Load(TextAsset gameStateJsonAsset)
        { 
            gameData = jsonDataService.LoadData<GameData>(Encoding.UTF8.GetBytes(gameStateJsonAsset.text), false);
            var converter = new GameDataConverter();
            gameState = converter.Convert(gameData);
            
            Debug.Log($"Game state: {gameData}");
        }
        
        public void Load(string relativePath)
        {
            gameData = jsonDataService.LoadData<GameData>(relativePath, false);
            var converter = new GameDataConverter();
            gameState = converter.Convert(gameData);
        }

        public void Save(GameState gameState)
        {
            jsonDataService.SaveData("/GameState.json", gameState, false);
        }
    }
}