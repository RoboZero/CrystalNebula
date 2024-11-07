using System;
using System.Collections.Generic;
using System.Linq;
using Source.Input;
using Source.Logic.State;
using Source.Visuals.MemoryStorage;
using UnityEngine;

namespace Source.Visuals
{
    public class PersonalStorageBehavior : LineStorageBehavior
    {
        [Header("Dependencies")]
        [SerializeField] private InputReaderSO inputReader;
        [SerializeField] private RectTransform backgroundRectTransform;
        
        private void OnEnable()
        {
            inputReader.PointerPositionEvent += OnPointerPosition;
        }

        private void OnDisable()
        {
            inputReader.PointerPositionEvent -= OnPointerPosition;
        }

        private void OnPointerPosition(Vector2 position, bool isMouse)
        {
            if(RectTransformUtility.ScreenPointToWorldPointInRectangle(backgroundRectTransform, position, Camera.main, out var worldPoint))
                transform.position = new Vector3(worldPoint.x, worldPoint.y, 0);
        }
        
        protected override void UpdateStorageFromState(GameState gameState)
        {
            var player = gameState.Players.FirstOrDefault(player => player.Id == playerID);
            if (player == null)
            {
                Debug.LogWarning($"Failed to read from player storage: playerId {playerID} is invalid, gamestate players count {gameState.Players.Count}");
                return;
            }

            var personalStorageState = player.PersonalStorage;
            itemStorageSize = personalStorageState.Length;
            state = personalStorageState;
        }
    }
}
