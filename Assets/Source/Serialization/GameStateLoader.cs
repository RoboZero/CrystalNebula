using System;
using System.Text;
using Source.Logic.State;
using UnityEngine;

namespace Source.Serialization
{
    public class GameStateLoader : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private TextAsset gameStateJsonAsset;

        public GameState GameState => gameState;

        private GameState gameState;
        private readonly JsonDataService jsonDataService = new();

        private void Awake()
        {
            Load(gameStateJsonAsset);
        }

        private void Load(TextAsset gameStateJsonAsset)
        { 
            gameState = jsonDataService.LoadData<GameState>(Encoding.UTF8.GetBytes(gameStateJsonAsset.text), false);
            Debug.Log($"Game state: {gameState}");
        }
        
        public void Load(string relativePath)
        {
            gameState = jsonDataService.LoadData<GameState>(relativePath, false);
        }

        public void Save(GameState gameState)
        {
            jsonDataService.SaveData("/GameState.json", gameState, false);
        }
    }
}