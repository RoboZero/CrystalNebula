using Source.Logic.Data;
using Source.Serialization;

namespace Source.Logic.State
{
    public static class SampleStates
    {
        public static readonly GameState TestState1 = new ()
        {
            Level = new LevelData
            {
                Definition = GameResources.BuildDefinitionPath("Levels","Level1")
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
                                    Definition = GameResources.BuildDefinitionPath("Programs","Build"),
                                    Progress = 0
                                }
                            },
                            new MemoryItemData
                            {
                                Program = new ProgramData
                                {
                                    OwnerId = 1,
                                    Definition = GameResources.BuildDefinitionPath("Programs","Research"),
                                    Progress = 0
                                }
                            }
                        }
                    }
                }
            },
            BattlefieldStorage = new BattlefieldStorageData
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
                            Definition = GameResources.BuildDefinitionPath("Buildings", "Flag"),
                        },
                        Unit = new UnitData
                        {
                            OwnerId = 1,
                            Definition = GameResources.BuildDefinitionPath("Units", "Guardian"),
                        },
                    }
                }
            }
        };
    }
}