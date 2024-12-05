using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Source.Logic.Events;
using Source.Logic.State.LineItems.Programs;
using Source.Logic.State.LineItems.Units;
using Source.Serialization;
using Source.Visuals.BattlefieldStorage;
using UnityEngine;

namespace Source.Logic.State
{
    public class EnemyController
    {
        public UnitMemoryDataSO CreatedUnitSO => createdUnitSO;
        public float DelayTime => delayTime;
        
        public bool IsRunning = true;
        
        private EnemyWaves enemyWaves;
        private int ownerId;
        private GameResources gameResources;
        private EventTracker eventTracker;
        private GameState gameState;
        
        private UnitMemoryDataSO createdUnitSO;
        private float delayTime;
        
        // TODO: Move to player processor logical behavior and use real state. 
        private CommandProgram commandProgram;
        private float tickDelay = 1000;

        public EnemyController(int ownerId, EnemyWaves enemyWaves, GameResources gameResources, EventTracker eventTracker, GameState gameState, CommandProgram commandProgram)
        {
            this.ownerId = ownerId;
            this.enemyWaves = enemyWaves;
            this.gameResources = gameResources;
            this.eventTracker = eventTracker;
            this.gameState = gameState;
            this.commandProgram = commandProgram;
            WaveSpawner();
            CommandRunner();
        }

        public void Tick(float deltaTime)
        {
            delayTime -= deltaTime;
        }
        
        private async UniTask CommandRunner()
        {
            while (IsRunning)
            {
                commandProgram.Tick(eventTracker, gameState);
                Debug.Log("Enemy Controller ticked command program");
                await UniTask.Delay(TimeSpan.FromSeconds(tickDelay));
            }
        }

        private async UniTask WaveSpawner()
        {
            var deploymentSlots = new List<int>();
            for (var i = 0; i < gameState.BattlefieldStorage.Length; i++)
            { 
                var item = gameState.BattlefieldStorage.Items[i];
                if (item.DeploymentZoneOwnerId == ownerId)
                {
                    deploymentSlots.Add(i);
                }
            }
            
            foreach (var wave in enemyWaves.Waves)
            {
                await SpawnWave(wave, deploymentSlots);
                delayTime = wave.timeUntilNextWave;
                await UniTask.Delay(TimeSpan.FromSeconds(wave.timeUntilNextWave));
                createdUnitSO = null;
            }
        }

        private async UniTask SpawnWave(EnemyWaves.Wave wave, List<int> deploymentSlots)
        {
            tickDelay = wave.commandDelay;

            var orderedSlots = new List<int>() { deploymentSlots.OrderBy(i => i).First() };
            
            foreach (var unit in wave.enemyUnitInOrder)
            {
                delayTime = wave.delayBetweenSpawn;
                createdUnitSO = unit;
                
                gameResources.TryLoadDefinition(this, unit, out var definition);
                var createdUnit = (UnitMemory) unit.CreateDefaultInstance(ownerId, definition);
                Debug.Log("Enemy Controller spawned unit");
                
                await UniTask.Delay(TimeSpan.FromSeconds(wave.delayBetweenSpawn));
                eventTracker.AddEvent(new CreateBattlefieldUnitsEventCommand(
                    eventTracker,
                    gameState.BattlefieldStorage,
                    orderedSlots,
                    createdUnit, 
                    false
                ));
            }
        }
    }
}