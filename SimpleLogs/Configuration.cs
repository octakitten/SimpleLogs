using Dalamud.Configuration;
using Dalamud.Plugin;
using System;
using System.Collections.Generic;

namespace SimpleLogs
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;

        public bool SomePropertyToBeSavedAndWithADefault { get; set; } = true;
        // some debugging variables here
        
        //public bool debuggingEnabled { get; set; } = true;
        //public int addedToParty { get; set; } = 0;
        
        //public bool handledDamageEvent { get; set; } = false;
        
        //public bool addedDamage { get; set; } = false;
        
        //public int addedDamageAmount { get; set; } = -1;
        
        //public double latestFightDuration { get; set; } = -1;
        
        //end debugging variables

        public bool ChatLoggingEnabled { get; set; } = true;

        public bool PacketLoggingEnabled { get; set; } = true;

        // leave this true for now, we'll disable this functionality later
        public bool network_testing { get; set; } = true;

        public List<string> chatLog = new List<string>();
        public List<string> packetLog = new List<string>();

        // the below exist just to make saving less cumbersome
        [NonSerialized]
        public IDalamudPluginInterface? PluginInterface;

        public void Initialize(IDalamudPluginInterface pluginInterface)
        {
            this.PluginInterface = pluginInterface;
        }

        public void Save()
        {
            this.PluginInterface!.SavePluginConfig(this);
        }
    }
}
