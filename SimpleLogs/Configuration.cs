using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace SimpleLogs
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;

        public bool SomePropertyToBeSavedAndWithADefault { get; set; } = true;

        public bool ChatLoggingEnabled { get; set; } = true;

        public bool PacketLoggingEnabled { get; set; } = true;

        // leave this true for now, we'll disable this functionality later
        public bool network_testing { get; set; } = true;

        public List<string> chatLog = new List<string>();
        public List<string> packetLog = new List<string>();

        // the below exist just to make saving less cumbersome
        [NonSerialized]
        private DalamudPluginInterface? PluginInterface;

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.PluginInterface = pluginInterface;
        }

        public void Save()
        {
            this.PluginInterface!.SavePluginConfig(this);
        }
    }
}
