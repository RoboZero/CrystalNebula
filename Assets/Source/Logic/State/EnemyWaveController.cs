using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Source.Logic.Events;
using Source.Logic.State.LineItems.Programs;
using Source.Logic.State.LineItems.Units;
using Source.Serialization;
using Source.Utility;
using Source.Visuals.BattlefieldStorage;
using UnityEngine;

namespace Source.Logic.State
{
    public class EnemyWaveController
    {
        public UnitMemoryDataSO CreatedUnitSO => createdUnitSO;
        public float ArriveDelayTime => arriveDelayTime;
        public float MoveDelayTime => moveDelayTime;
        
        public bool IsRunning = true;
        
        private EnemyWaves enemyWaves;
        private int ownerId;
        private GameResources gameResources;
        private EventTracker eventTracker;
        private GameState gameState;

        private Queue<UnitMemoryDataSO> nextUnitsToArrive = new();
        
        private UnitMemoryDataSO createdUnitSO;
        private bool waitingForNextWave;
        private float arriveDelayTime;
        private float moveDelayTime;
        
        // TODO: Move to player processor logical behavior and use real state. 
        private CommandProgram commandProgram;
        private float commandTickDelay = 1000;

        public EnemyWaveController(int ownerId, EnemyWaves enemyWaves, GameResources gameResources, EventTracker eventTracker, GameState gameState, CommandProgram commandProgram)
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
            if(arriveDelayTime > 0) 
                arriveDelayTime -= deltaTime;
            if(moveDelayTime > 0) 
                moveDelayTime -= deltaTime;
        }
        
        private async UniTask CommandRunner()
        {
            while (IsRunning)
            {
                moveDelayTime = commandTickDelay;
                await UniTask.Delay(TimeSpan.FromSeconds(commandTickDelay));
                commandProgram.Tick(eventTracker, gameState);
                Debug.Log("Enemy Controller ticked command program");
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
                await SpawnWaveImmediate(wave, deploymentSlots);
                arriveDelayTime = wave.timeUntilNextWave;
                await UniTask.Delay(TimeSpan.FromSeconds(wave.timeUntilNextWave));
                createdUnitSO = null;
            }
        }

        private UniTask SpawnWaveTrickle(EnemyWaves.Wave wave, List<int> deploymentSlots)
        {
            commandTickDelay = wave.commandDelay;

            foreach (var unit in wave.enemyUnitInOrder)
            {
                nextUnitsToArrive.Enqueue(unit);
            }

            return SpawnNextUnitsTrickle(wave, deploymentSlots);
        }

        private async UniTask SpawnWaveImmediate(EnemyWaves.Wave wave, List<int> deploymentSlots)
        {
            commandTickDelay = wave.commandDelay;

            var orderedSlots = new List<int>(deploymentSlots.OrderBy(i => i));
            deploymentSlots.Dump();
            
            foreach (var unit in wave.enemyUnitInOrder)
            {
                nextUnitsToArrive.Enqueue(unit);
                Debug.Log("Next unit queued: " + unit.name);
            }
            
            while (orderedSlots.Count > 0 && nextUnitsToArrive.Count > 0)
            {
                createdUnitSO = nextUnitsToArrive.Peek();
                arriveDelayTime = wave.delayBetweenSpawn;
                await UniTask.Delay(TimeSpan.FromSeconds(arriveDelayTime));

                var deploySlot = 0;
                for (var i = orderedSlots.Count - 1; i >= 0; i--)
                {
                    deploySlot = orderedSlots[i];
                    
                    if (gameState.BattlefieldStorage.Items[deploySlot].Unit == null)
                    {
                        break;
                    }
                }

                var unit = nextUnitsToArrive.Dequeue();
                Debug.Log($"Deploy slot: {deploySlot}, Unit: {unit}");
                await SpawnUnit(unit, new List<int>{deploySlot});
            }

            arriveDelayTime = wave.delayBetweenSpawn;
            await SpawnNextUnitsTrickle(wave, deploymentSlots);
        }
        
        private async UniTask SpawnNextUnitsTrickle(EnemyWaves.Wave wave, List<int> deploymentSlots)
        {
            var firstDeploySlot = deploymentSlots.OrderBy(i => i).First();
            var firstDeploySlots = new List<int> { firstDeploySlot };
            
            while (nextUnitsToArrive.Count > 0)
            {
                arriveDelayTime = wave.delayBetweenSpawn;
                createdUnitSO = nextUnitsToArrive.Peek();
                await UniTask.Delay(TimeSpan.FromSeconds(arriveDelayTime));
                
                if (gameState.BattlefieldStorage.Items[firstDeploySlot].Unit != null)
                {
                    arriveDelayTime = moveDelayTime + 0.01f;
                    await UniTask.Delay(TimeSpan.FromSeconds(arriveDelayTime));
                    continue;
                }
                
                var unit = nextUnitsToArrive.Dequeue();
                await SpawnUnit(unit, firstDeploySlots);
            }
        }

        private UniTask SpawnUnit(UnitMemoryDataSO unit, List<int> slots)
        {
            gameResources.TryLoadDefinition(this, unit, out var definition);
            var createdUnit = (UnitMemory) unit.CreateDefaultInstance(ownerId, definition);
                
            return eventTracker.AddEvent(new CreateBattlefieldUnitsEventCommand(
                eventTracker,
                gameState.BattlefieldStorage,
                slots,
                createdUnit, 
                false
            ));
        }
    }
}