using Source.Logic.Data;

namespace Source.Logic.State
{
    public static class SampleStates
    {
        public static readonly GameState TestState1 = new GameState()
        {
            Level = new LevelData()
            {
                Definition = "Levels:Level1"
            },
            Players = new []
            {
                new PlayerData()
                {
                    Id = 1,
                    MemoryStorage = new MemoryStorageData()
                    {
                        Items = new[]
                        {
                            new MemoryStorageData.Item()
                            {
                                Program = new ProgramData()
                                {
                                    OwnerId = 1,
                                    Definition = "Programs:Build",
                                    Progress = 0
                                }
                            },
                            new MemoryStorageData.Item()
                            {
                                Program = new ProgramData()
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
            BattlefieldStorage = new BattlefieldData()
            {
                Length = 20,
                Items = new []
                {
                    new BattlefieldData.Item()
                    {
                        Location = 19,
                        Unit = new UnitData()
                        {
                            OwnerId = 1,
                            Definition = "Units:Guardian"
                        },
                        Building = new BuildingData()
                        {
                            OwnerId = 1,
                            Definition = "Buildings:Flag"
                        }
                    }
                }
            }
        };
    }
}