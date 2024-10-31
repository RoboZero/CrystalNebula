using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Source.Logic;
using Source.Logic.Data;
using Source.Logic.Events;
using Source.Serialization;
using Source.Serialization.Data;
using UnityEngine;

namespace Source.Visuals
{
    // Debug
    public class ProcessorBehavior : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private GameStateLoader gameStateLoader;
        [SerializeField] private EventTracker eventTracker;

        private float time;
        
        private void Update()
        {
            var processor = gameStateLoader.GameState.Players[0].Processors[0];

            int ownerId = 1;

            time += Time.deltaTime;
            if (time >= 3)
            {
                var battlefield = gameStateLoader.GameState.BattlefieldStorage;
                
                /*
                    battlefield.Items.Insert(index, new BattlefieldItem());
                 */

                StringBuilder logBuilder = new StringBuilder();

                var fromSlots = new List<int>();
                for (var index = 0; index < battlefield.Items.Count; index++)
                {
                    var item = battlefield.Items[index];
                    if (item != null && item.Unit != null)
                    {
                        if (item.Unit.OwnerId == ownerId)
                        {
                            fromSlots.Add(index);
                        }
                    }
                }

                eventTracker.AddEvent(new MoveUnitsInDirectionEventCommand(
                    eventTracker,
                    battlefield,
                    fromSlots,
                    MoveUnitsInDirectionEventCommand.Direction.Right,
                    1,
                    null
                ));


                time = 0;
            }
        }
    }
}