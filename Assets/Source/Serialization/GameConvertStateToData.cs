using System.Collections.Generic;
using System.Linq;
using Source.Logic.State;
using Source.Serialization.Data;

namespace Source.Serialization
{
    public class GameConvertStateToData
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
                Building = battlefieldItem.Building != null
                    ? new BuildingData()
                    {
                        OwnerId = battlefieldItem.Building.OwnerId,
                        Definition = battlefieldItem.Building.Definition,
                        Health = battlefieldItem.Building.Health,
                        Power = battlefieldItem.Building.Power
                    }
                    : null,
                Unit = battlefieldItem.Unit != null
                    ? new UnitData()
                    {
                        OwnerId = battlefieldItem.Unit.OwnerId,
                        Definition = battlefieldItem.Unit.Definition,
                        Health = battlefieldItem.Unit.Health,
                        Power = battlefieldItem.Unit.Power
                    }
                    : null
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
                            Progress = storedItem.Memory.CurrentProgress
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