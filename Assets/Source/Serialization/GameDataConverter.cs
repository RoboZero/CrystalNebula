using System.Collections.Generic;
using System.Linq;
using Source.Logic;
using Source.Logic.Data;
using Source.Logic.State;
using Source.Serialization.Data;
using LineStorage = Source.Logic.Data.LineStorage;

namespace Source.Serialization
{
    public class GameDataConverter
    {
        public GameState Convert(GameData gameData)
        {
            var converter = new ConvertDataToState();

            return converter.Convert(gameData);
        }

        public GameData Convert(GameState gameState)
        {
            var converter = new ConvertStateToData();

            return converter.Convert(gameState);
        }

        // TODO: Load from game resources HERE so scriptable objects are already distributed
        private class ConvertDataToState
        {
            public GameState Convert(GameData gameData)
            {
                var converter = new ConvertDataToState();
            
                var players = gameData.Players.Select(ConvertPlayer).ToList();

                var gameState = new GameState
                {
                    Level = ConvertLevel(gameData.Level),
                    BattlefieldStorage = ConvertBattlefieldStorage(gameData.BattlefieldStorage),
                    Players = players
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
                return new BattlefieldItem()
                {
                    Building = battlefieldItemData.Building != null
                        ? new Building()
                        {
                            OwnerId = battlefieldItemData.Building.OwnerId,
                            Definition = battlefieldItemData.Building.Definition,
                            Health = battlefieldItemData.Building.Health,
                            Power = battlefieldItemData.Building.Power
                        }
                        : null,
                    Unit = battlefieldItemData.Unit != null
                        ? new Unit()
                        {
                            OwnerId = battlefieldItemData.Unit.OwnerId,
                            Definition = battlefieldItemData.Unit.Definition,
                            Health = battlefieldItemData.Unit.Health,
                            Power = battlefieldItemData.Unit.Power
                        }
                        : null
                };
            }
            private Player ConvertPlayer(PlayerData playerData)
            {
                var processors = playerData.Processors.Select(ConvertProcessor).ToList();

                return new Player()
                {
                    Id = playerData.Id,
                    Processors = processors,
                    DiskStorage = ConvertLineStorage(playerData.DiskStorage),
                    MemoryStorage = ConvertLineStorage(playerData.MemoryStorage)
                };
            }
            private Processor ConvertProcessor(ProcessorData processorData)
            {
                return new Processor()
                {
                    Definition = processorData.Definition,
                    ProcessorStorage = ConvertLineStorage(processorData.ProcessorStorage),
                    ClockSpeed = processorData.ClockSpeed,
                };
            }
            private LineStorage ConvertLineStorage(LineStorageData lineStorage)
            {
                var diskStorageItems = new List<LineItem>(new LineItem[lineStorage.Length]);

                foreach (var storedItem in lineStorage.Items)
                {
                    diskStorageItems[storedItem.Location] = new LineItem()
                    {
                        Description = storedItem.Description,
                        Memory = new Memory()
                        {
                            OwnerId = storedItem.Memory.OwnerId,
                            Definition = storedItem.Memory.Definition,
                            Progress = storedItem.Memory.Progress
                        }
                    };
                }

                return new LineStorage()
                {
                    Length = lineStorage.Length,
                    Items = diskStorageItems
                };
            }
        }
        private class ConvertStateToData
        {
            public GameData Convert(GameState gameState)
            {
                var players = gameState.Players.Select(ConvertPlayer).ToList();

                var gameData = new GameData
                {
                    Level = ConvertLevel(gameState.Level),
                    BattlefieldStorage = ConvertBattlefieldStorage(gameState.BattlefieldStorage),
                    Players = players,
                };

                return gameData;
            }
            private LevelData ConvertLevel(Level level)
            {
                return new LevelData()
                {
                    Definition = level.Definition
                };
            }
            private BattlefieldStorageData ConvertBattlefieldStorage(BattlefieldStorage battlefieldStorage)
            {
                var battlefieldItems = (
                    from item in battlefieldStorage.Items 
                    where item != null 
                    select ConvertBattlefieldItem(item))
                    .ToList();

                return new BattlefieldStorageData()
                {
                    Items = battlefieldItems
                };
            }
            private BattlefieldItemData ConvertBattlefieldItem(BattlefieldItem battlefieldItem)
            {
                return new BattlefieldItemData()
                {
                    Building = battlefieldItem.Building != null ? new BuildingData()
                        {
                            OwnerId = battlefieldItem.Building.OwnerId,
                            Definition = battlefieldItem.Building.Definition,
                            Health = battlefieldItem.Building.Health,
                            Power = battlefieldItem.Building.Power
                        }
                        : null,
                    Unit = battlefieldItem.Unit != null ? new UnitData()
                    {
                        OwnerId = battlefieldItem.Unit.OwnerId,
                        Definition = battlefieldItem.Unit.Definition,
                        Health = battlefieldItem.Unit.Health,
                        Power = battlefieldItem.Unit.Power
                    } : null
                };
            }
            private PlayerData ConvertPlayer(Player player)
            {
                var processors = player.Processors.Select(ConvertProcessor).ToList();
                
                return new PlayerData()
                {
                    Id = player.Id,
                    Processors = processors,
                    DiskStorage = ConvertLineStorage(player.DiskStorage),
                    MemoryStorage = ConvertLineStorage(player.MemoryStorage)
                };
            }
            private ProcessorData ConvertProcessor(Processor processor)
            {
                return new ProcessorData()
                {
                    Definition = processor.Definition,
                    ProcessorStorage = ConvertLineStorage(processor.ProcessorStorage),
                    ClockSpeed = processor.ClockSpeed,
                };
            }
            private LineStorageData ConvertLineStorage(LineStorage lineStorage)
            {
                var diskStorageItems = new List<LineItemData>();

                foreach (var storedItem in lineStorage.Items)
                {
                    if (storedItem != null)
                    {
                        diskStorageItems.Add(new LineItemData()
                        {
                            Description = storedItem.Description,
                            Memory = new MemoryData()
                            {
                                OwnerId = storedItem.Memory.OwnerId,
                                Definition = storedItem.Memory.Definition,
                                Progress = storedItem.Memory.Progress
                            }
                        });
                    }
                }

                return new LineStorageData()
                {
                    Length = lineStorage.Length,
                    Items = diskStorageItems
                };
            }
        }
    }
}