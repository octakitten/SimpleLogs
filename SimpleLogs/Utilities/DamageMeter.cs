using System;
using System.Collections.Generic;

namespace SimpleLogs.Utilities;


public class DamageMeter
{
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
        public double timestamp;
    }
    
    private List<DamageEntry> damageLog;
    private List<PartyMember> partyMembers;
    private Plugin plugin;
    
    public DamageMeter(Plugin plugin)
    {
        this.plugin = plugin;
        this.damageLog = new List<DamageEntry>();
        this.partyMembers = new List<PartyMember>();
    }
    
    public void AddDamageEntry(string name, int damage, double time)
    {
        DamageEntry newEntry = new DamageEntry();
        newEntry.name = name;
        newEntry.damage = damage;
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
                //if (plugin.Configuration.debuggingEnabled)
                //{
                //    plugin.Configuration.addedDamage = true;
                //    plugin.Configuration.addedDamageAmount = damage;
                //}
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

    public void HandleEvent(string name, int damage, double timestamp)
    {
        if (IsPartyMember(name))
        {
            UpdatePartyMemberDamage(name, damage);
            AddDamageEntry(name, damage, timestamp);
        }
        else
        {
            AddPartyMember(name);
            UpdatePartyMemberDamage(name, damage);
            AddDamageEntry(name, damage, timestamp);
        }
    }
    
    public void Dispose()
    {
        
    }
    
    
}