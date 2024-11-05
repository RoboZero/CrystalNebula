using System.Collections.Generic;
using System.Linq;
using Source.Logic.State;
using Source.Serialization.Data;

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
                        CurrentProgress = storedItem.Memory.Progress
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
}