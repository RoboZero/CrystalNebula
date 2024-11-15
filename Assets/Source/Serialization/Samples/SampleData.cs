using System.Collections.Generic;
using Source.Serialization.Data;

namespace Source.Serialization.Samples
{
    public static class SampleData
    {
        public static readonly GameData TestState1 = new ()
        {
            Level = new LevelData
            {
                Definition = GameResources.BuildDefinitionPath(GameResourceConstants.LEVELS_PATH,"Level1")
            },
            Players = new List<PlayerData>
            {
                new PlayerData
                {
                    Id = 0,
                    PersonalStorage = new MemoryStorageData()
                    {
                        Length = 1,
                        Items = new List<MemoryItemData>
                        {
                            new MemoryItemData
                            {
                                Location = 0,
                                Memory = new MemoryData
                                {
                                    OwnerId = 0,
                                    Definition = GameResources.BuildDefinitionPath(GameResourceConstants.PROGRAMS_PATH,"Build"),
                                    Progress = 0
                                }
                            },
                        }
                    },
                    Processors = new List<ProcessorData>
                    {
                        new ProcessorData
                        {
                            Definition = "Processors/Basic",
                            ClockSpeed = 1.0f,
                            ProcessorStorage = new MemoryStorageData
                            {
                                Length = 1,
                                Items = new List<MemoryItemData>
                                {
                                    new MemoryItemData
                                    {
                                        Location = 0,
                                        Memory = new MemoryData
                                        {
                                            OwnerId = 0,
                                            Definition = GameResources.BuildDefinitionPath(GameResourceConstants.PROGRAMS_PATH,"Build"),
                                            Progress = 0
                                        }
                                    },
                                }
                            }  
                        }
                    },
                    MemoryStorage = new MemoryStorageData
                    {
                        Length = 3,
                        Items = new List<MemoryItemData>
                        {
                            new MemoryItemData
                            {
                                Location = 0,
                                Memory = new MemoryData
                                {
                                    OwnerId = 0,
                                    Definition = GameResources.BuildDefinitionPath(GameResourceConstants.PROGRAMS_PATH,"Build"),
                                    Progress = 0
                                }
                            },
                            new MemoryItemData
                            {
                                Location = 1,
                                Memory = new MemoryData
                                {
                                    OwnerId = 0,
                                    Definition = GameResources.BuildDefinitionPath(GameResourceConstants.PROGRAMS_PATH,"Research"),
                                    Progress = 0
                                }
                            }
                        }
                    },
                    DiskStorage = new MemoryStorageData
                    {
                        Length = 5,
                        Items = new List<MemoryItemData>
                        {
                            new MemoryItemData
                            {
                                Location = 0,
                                Memory = new MemoryData
                                {
                                    OwnerId = 0,
                                    Definition = GameResources.BuildDefinitionPath(GameResourceConstants.PROGRAMS_PATH,"Build"),
                                    Progress = 0
                                }
                            },
                            new MemoryItemData
                            {
                                Location = 1,
                                Memory = new MemoryData
                                {
                                    OwnerId = 0,
                                    Definition = GameResources.BuildDefinitionPath(GameResourceConstants.PROGRAMS_PATH,"Research"),
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
                Items = new List<BattlefieldItemData>
                {
                    new BattlefieldItemData
                    {
                        Location = 19,
                        Building = new BuildingData
                        {
                            OwnerId = 0,
                            Definition = GameResources.BuildDefinitionPath(GameResourceConstants.BUILDINGS_PATH, "Flag"),
                        },
                        Unit = new UnitData
                        {
                            OwnerId = 0,
                            Definition = GameResources.BuildDefinitionPath(GameResourceConstants.UNITS_PATH, "Guardian"),
                        }
                    }
                }
            }
        };
    }
}