using Source.Logic.Data;

namespace Source.Logic.State
{
    public static class SampleStates
    {
        public static readonly GameState TestState1 = new GameState()
        {
            Level = new LevelData
            {
                Definition = "Levels:Level1"
            },
            Players = new []
            {
                new PlayerData
                {
                    Id = 1,
                    MemoryStorage = new MemoryStorageData
                    {
                        Items = new[]
                        {
                            new MemoryItemData
                            {
                                Program = new ProgramData
                                {
                                    OwnerId = 1,
                                    Definition = "Programs:Build",
                                    Progress = 0
                                }
                            },
                            new MemoryItemData
                            {
                                Program = new ProgramData
                                {
                                    OwnerId = 1,
                                    Definition = "Programs:Research",
                                    Progress = 0
                                }
                            }
                        }
                    }
                }
            },
            BattlefieldStorageStorage = new BattlefieldStorageData
            {
                Length = 20,
                Items = new []
                {
                    new BattlefieldItemData
                    {
                        Location = 19,
                        Building = new BuildingData
                        {
                            OwnerId = 1,
                            Definition = "Buildings:Flag"
                        },
                        Unit = new UnitData
                        {
                            OwnerId = 1,
                            Definition = "Units:Guardian"
                        },
                    }
                }
            }
        };
    }
}