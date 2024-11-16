using System;
using System.Collections.Generic;
using System.Linq;
using Source.Input;
using Source.Logic.State;
using Source.Logic.State.LineItems;
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
            var player = gameState.Players.FirstOrDefault(player => player.Id == playerId);
            if (player == null)
            {
                Debug.LogWarning($"Failed to read from player storage: playerId {playerId} is invalid, gamestate players count {gameState.Players.Count}");
                return;
            }

            var personalStorageState = player.PersonalStorage;
            itemStorageSize = personalStorageState.Length;
            state = personalStorageState;
            level = gameState.Level;

            ShiftItemsUp(personalStorageState);
        }

        private void ShiftItemsUp(LineStorage<MemoryItem> personalStorageState)
        {
            // Shift all items up to 0 if empty so transfer pairs can be made
            // (Observed that if item was in slot 2 and you only selected one battlefield slot, never used slot 2.
            for (var i = personalStorageState.Items.Count - 1; i >= 1; i--)
            {
                if (personalStorageState.Items[i - 1] == null)
                {
                    personalStorageState.Items[i - 1] = personalStorageState.Items[i];
                    personalStorageState.Items[i] = null;
                }
            }
        }
    }
}
