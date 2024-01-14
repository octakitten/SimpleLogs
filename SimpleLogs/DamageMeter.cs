using System;
using System.Collections.Generic;

namespace SimpleLogs;


public class DamageMeter
{
    public struct PartyMember
    {
        public string name;
        public int damage;
        public double dps;
    }
    
    public struct DamageEntry
    {
        public string name;
        public int damage;
        public double timestamp;
    }

    // these two are dummy variables, dont use for anything permanent
    private PartyMember player;
    private DamageEntry lastEntry;
    
    private List<DamageEntry> damageLog = new List<DamageEntry>();
    private List<PartyMember> partyMembers = new List<PartyMember>();
    private Timer timer;
    private Plugin plugin;
    
    public DamageMeter(Plugin plugin, Timer tmr)
    {
        timer = tmr;
        this.plugin = plugin;
    }
    
    public void AddDamageEntry(string name, int damage, double time)
    {
        lastEntry.name = name;
        lastEntry.damage = damage;
        lastEntry.timestamp = time;
        damageLog.Add(lastEntry);
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
        player.name = name;
        player.damage = 0;
        partyMembers.Add(player);
    }
    
    public void UpdatePartyMemberDamage(string name, int damage)
    {
        for (int i = 0; i < partyMembers.Count; i++)
        {
            if (partyMembers[i].name == name)
            {
                player.damage = partyMembers[i].damage;
                player.damage += damage;
                player.dps = player.damage / getFightDuration();
                partyMembers[i] = player;
                return;
            }
        }
        
    }

    private double getFightDuration()
    {
        return damageLog[0].timestamp - damageLog[Index.End].timestamp;
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
        player.name = name;
        player.damage = damage;
        lastEntry.name = name;
        lastEntry.damage = damage;
        lastEntry.timestamp = timestamp;
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