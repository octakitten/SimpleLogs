using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;

namespace SimpleLogs.Utilities;


public class DamageMeter
{
    private List<string> debuffs = new List<string>()
    {
        "combust",
        "combust ii",
        "combust iii",
        "aero",
        "aero ii",
        "dia",
        "bio",
        "bio ii",
        "biolysis",
        "eukrasian dosis",
        "eukrasian dosis ii",
        "eukrasian dosis iii",
    };
    
    private List<int> debuffPotencies = new List<int>()
    {
        40,
        50,
        55,
        35,
        55,
        72,
        20,
        40,
        70,
        40,
        60,
        75,
    };
    
    private List<int> debuffDurations = new List<int>()
    {
        30,
        30,
        30,
        30,
        30,
        30,
        30,
        30,
        30,
        30,
        30,
        30,
    };
    
    public class PartyMember
    {
        public string name;
        public int damage;
        public double dps;
    }
    
    public class DamageEntry
    {
        public string name;
        public int damage;
        public int potency;
        public double timestamp;
    }
    
    public class DebuffEntry
    {
        public string name;
        public string debuff;
        public double timestamp;
    }
    
    private List<DamageEntry> damageLog;
    private List<PartyMember> partyMembers;
    private List<DebuffEntry> debuffLog;
    private Plugin plugin;
    
    public DamageMeter(Plugin plugin)
    {
        this.plugin = plugin;
        this.damageLog = new List<DamageEntry>();
        this.partyMembers = new List<PartyMember>();
    }
    
    public void AddDamageEntry(string name, int damage, int potency, double time)
    {
        DamageEntry newEntry = new DamageEntry();
        newEntry.name = name;
        newEntry.damage = damage;
        newEntry.potency = potency;
        newEntry.timestamp = time;
        damageLog.Add(newEntry);
    }
    
    public bool IsPartyMember(string name)
    {
        for (int i = 0; i < partyMembers.Count; i++)
        {
            if (partyMembers[i].name == name)
            {
                return true;
            }
        }
        return false;
    }

    public void AddDebuffEntry(string name, string debuff, double time)
    {
        DebuffEntry newDebuff = new DebuffEntry();
        newDebuff.name = name;
        newDebuff.debuff = debuff;
        newDebuff.timestamp = time;
        debuffLog.Add(newDebuff);
    }
    
    public void AddPartyMember(string name)
    {
        //if (plugin.Configuration.debuggingEnabled)
        //{
        //    plugin.Configuration.addedToParty += 1;
        //}
        PartyMember newPlayer = new PartyMember();
        newPlayer.name = name;
        newPlayer.damage = 0;
        partyMembers.Add(newPlayer);
    }
    
    public void UpdatePartyMemberDamage(string name, int damage)
    {
        for (int i = 0; i < partyMembers.Count; i++)
        {
            if (partyMembers[i].name == name)
            {
                //TODO: debuff handling and approximation
                partyMembers[i].damage += damage;
                partyMembers[i].dps = partyMembers[i].damage / GetFightDuration();
                break;
            }
        }
    }

    private double GetFightDuration()
    {
        double duration = 0;
        int count = damageLog.Count;
        if (count > 0)
        {
            duration = damageLog[count - 1].timestamp - damageLog[0].timestamp;
            //plugin.Configuration.latestFightDuration = duration;
        }
        return duration;
    }
    
    public List<DamageEntry> GetDamageLog()
    {
        return damageLog;
    }
    
    public List<PartyMember> GetPartyMembers()
    {
        return partyMembers;
    }
    
    public void Reset()
    {
        damageLog.Clear();
        partyMembers.Clear();
    }

    public void HandleEvent(string name, int damage, int potency, double timestamp)
    {
        if (IsPartyMember(name))
        {
            UpdatePartyMemberDamage(name, damage);
            AddDamageEntry(name, damage, potency, timestamp);
        }
        else
        {
            AddPartyMember(name);
            UpdatePartyMemberDamage(name, damage);
            AddDamageEntry(name, damage, potency, timestamp);
        }
    }

    public void HandleDebuffEvent(string name, string debuff, double time)
    {
        if (IsPartyMember(name))
        {
            AddDebuffEntry(name, debuff, time);
        }
        else
        {
            AddPartyMember(name);
            AddDebuffEntry(name, debuff, time);
        }
    }
    
    public void Dispose()
    {
        
    }
    
    
}