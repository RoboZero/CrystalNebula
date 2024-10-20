using System;
using Source.Logic;
using Source.Logic.Data;
using Source.Serialization;
using UnityEngine;

namespace Source.Visuals
{
    // Debug
    public class ProcessorBehavior : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private GameStateLoader gameStateLoader;

        private float time;
        
        private void Update()
        {
            var processor = gameStateLoader.GameState.Players[0].Processors[0];

            time += Time.deltaTime;
            if (time >= processor.ClockSpeed)
            {
                var battlefield = gameStateLoader.GameState.BattlefieldStorage;
                for (var index = 0; index < battlefield.Items.Count; index++)
                {
                    var item = battlefield.Items[index];
                    battlefield.Items.Insert(index, new BattlefieldItem());
                }


                time = 0;
            }
        }
    }
}