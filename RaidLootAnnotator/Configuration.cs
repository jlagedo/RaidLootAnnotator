using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace RaidLootAnnotator;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;

    public LootData LootData { get; set; } = new LootData();

    // the below exist just to make saving less cumbersome
    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
    }
}
