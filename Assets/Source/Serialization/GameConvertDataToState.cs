using System.Collections.Generic;
using System.Linq;
using Source.Logic.State;
using Source.Logic.State.LineItems;
using Source.Serialization.Data;
using Source.Visuals.Battlefield;
using Source.Visuals.LineStorage;
using UnityEngine;

namespace Source.Serialization
{
    // TODO: Load from game resources HERE so scriptable objects are already distributed
    public class GameConvertDataToState
    {
        private GameResources gameResources;

        public GameConvertDataToState(GameResources gameResources)
        {
            this.gameResources = gameResources;
        }

        public GameState Convert(GameData gameData)
        {
            var players = gameData.Players.Select(ConvertPlayer).ToList();

            var playerDictionary = new Dictionary<int, Player>();
            foreach (var player in players)
            {
                playerDictionary.Add(player.Id, player);
            }
            
            var gameState = new GameState
            {
                Level = ConvertLevel(gameData.Level),
                BattlefieldStorage = ConvertBattlefieldStorage(gameData.BattlefieldStorage),
                Players = playerDictionary
            };
            return gameState;
        }

        private Level ConvertLevel(LevelData levelData)
        {
            return new Level()
            {
                Definition = levelData.Definition
            };
        }

        private BattlefieldStorage ConvertBattlefieldStorage(BattlefieldStorageData battlefieldStorage)
        {
            var battlefieldItems = new List<BattlefieldItem>(new BattlefieldItem[battlefieldStorage.Length]);

            for (var i = 0; i < battlefieldStorage.Length; i++)
            {
                battlefieldItems[i] = new BattlefieldItem();
            }
            
            foreach (var item in battlefieldStorage.Items)
            {
                battlefieldItems[item.Location] = ConvertBattlefieldItem(item);
            }

            return new BattlefieldStorage()
            {
                Items = battlefieldItems
            };
        }

        private BattlefieldItem ConvertBattlefieldItem(BattlefieldItemData battlefieldItemData)
        {
            Building building = null;
            if (battlefieldItemData.Building != null)
            {
                if (gameResources.TryLoadAsset(this, battlefieldItemData.Building.Definition, out BuildingDataSO buildingDataSO))
                {
                    building = buildingDataSO.CreateInstance(battlefieldItemData.Building);
                }
            }

            Unit unit = null;
            if (battlefieldItemData.Unit != null)
            {
                if (gameResources.TryLoadAsset(this, battlefieldItemData.Unit.Definition, out UnitDataSO unitDataSO))
                {
                    unit = unitDataSO.CreateInstance(battlefieldItemData.Unit);
                }
            }

            return new BattlefieldItem()
            {
                Building = building,
                Unit = unit
            };
        }

        private Player ConvertPlayer(PlayerData playerData)
        {
            var processors = playerData.Processors.Select(ConvertProcessor).ToList();

            return new Player()
            {
                Id = playerData.Id,
                PersonalStorage = ConvertLineStorage("Personal", playerData.PersonalStorage),
                Processors = processors,
                MemoryStorage = ConvertLineStorage("Memory", playerData.MemoryStorage),
                DiskStorage = ConvertLineStorage("Disk", playerData.DiskStorage),
            };
        }

        private Processor ConvertProcessor(ProcessorData processorData)
        {
            return new Processor()
            {
                Definition = processorData.Definition,
                ProcessorStorage = ConvertLineStorage("Processor", processorData.ProcessorStorage),
                ClockSpeed = processorData.ClockSpeed,
            };
        }

        private LineStorage ConvertLineStorage(string storageName, LineStorageData lineStorage)
        {
            var lineStorageItems = new List<LineItem>(new LineItem[lineStorage.Length]);

            for (var index = 0; index < lineStorageItems.Count; index++)
            {
                lineStorageItems[index] = new LineItem();
            }

            foreach (var storedItem in lineStorage.Items)
            {
                if (gameResources.TryLoadAsset(this, storedItem.Memory.Definition, out MemoryDataSO memoryDataSO))
                {
                    lineStorageItems[storedItem.Location] = new LineItem()
                    {
                        Description = storedItem.Description,
                        Memory = memoryDataSO.CreateMemoryInstance(storedItem.Memory),
                    };
                }
                else
                {
                    Debug.LogError($"Unable to load {nameof(memoryDataSO)} asset for stored data item: {storedItem}");
                }
            }

            return new LineStorage()
            {
                StorageName = storageName,
                Length = lineStorage.Length,
                Items = lineStorageItems
            };
        }
    }
}