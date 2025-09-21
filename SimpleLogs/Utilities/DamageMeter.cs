using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Linq;

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
        "baneful impaction",
        "eukrasian dosis",
        "eukrasian dosis ii",
        "eukrasian dosis iii",
        "eukrasian dyskrasia",
        "higanbana",
        "venomous bite",
        "windbite",
        "caustic bite",
        "stormbite",
        "thunder",
        "thunder ii",
        "thunder iii",
        "thunder iv",
        "high thunder",
        "high thunder ii",
    };
    
    private List<int> debuffPotencies = new List<int>()
    {
        50,
        60,
        70,
        30,
        50,
        80,
        20,
        40,
        80,
        140,
        40,
        60,
        80,
        40,
        50,
        15,
        20,
        20,
        25,
        45,
        30,
        50,
        35,
        60,
        40
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
        15,
        30,
        30,
        30,
        30,
        60,
        45,
        45,
        45,
        45
    };
    
    public class PartyMember
    {
        public string name;
        public int damage;
        public int potency;
        public double dps;
        public double pps;
        public List<DebuffEntry> debuffs;
        public double debuffDuration;
        public double debuff2Duration;

        public PartyMember()
        {
            debuffs = new List<DebuffEntry>();
            name = "";
        }
    }
    
    public class DamageEntry
    {
        public string name;
        public int damage;
        public int potency;
        public double timestamp;
        public DamageEntry()
        {
            name = "";

        }
    }
    
    public class DebuffEntry
    {
        public string name;
        public string debuff;
        public double timestamp;

        public DebuffEntry()
        {
            name = "";
            debuff = "";
        }
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
        this.debuffLog = new List<DebuffEntry>();
    }
    
    public void AddDamageEntry(string name, int damage, int potency, double time)
    {
        #if DEBUG
        this.plugin.DebugLogger.AddEntry($"Adding damage entry for {name} with {damage} damage and {potency} potency at {time}");
        #endif
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
        #if DEBUG
        this.plugin.DebugLogger.AddEntry($"Adding debuff entry for {name} with {debuff} at {time}");
        #endif
        DebuffEntry newDebuff = new DebuffEntry();
        newDebuff.name = name;
        newDebuff.debuff = debuff;
        newDebuff.timestamp = time;
        debuffLog.Add(newDebuff);
        for (int i = 0; i < partyMembers.Count; i++)
        {
            if (partyMembers[i].name == name)
            {
                partyMembers[i].debuffs.Add(newDebuff);
                break;
            }
        }
    }
    
    public void AddPartyMember(string name)
    {
        //if (plugin.Configuration.debuggingEnabled)
        //{
        //    plugin.Configuration.addedToParty += 1;
        //}
        #if DEBUG
        this.plugin.DebugLogger.AddEntry($"Adding party member {name}");
        #endif
        PartyMember newPlayer = new PartyMember();
        newPlayer.name = name;
        newPlayer.damage = 0;
        partyMembers.Add(newPlayer);
    }
    
    public void UpdatePartyMemberDamage(string name, int damage, int potency)
    {
        #if DEBUG
        this.plugin.DebugLogger.AddEntry($"Updating party member {name} with {damage} damage and {potency} potency");
        #endif
        for (int i = 0; i < partyMembers.Count; i++)
        {
            if (partyMembers[i].name == name)
            {
                //TODO: debuff handling and approximation
                partyMembers[i].potency += potency;
                partyMembers[i].damage += damage;
                partyMembers[i].pps = partyMembers[i].potency / GetFightDuration();
                partyMembers[i].dps = partyMembers[i].damage / GetFightDuration();
                break;
            }
        }
    }

    public void CalcDOTDuration()
    {
        // calculate whether the DoT effect is still active or not
        // as well as whether it has been reapplied or not
        // then calculate the approximate damage of the dot using 
        // its duration on the target, the damage of the other spells
        // used in the same timeframe, and the potency of the dot vs the other spells used
        // then add that damage to the total damage of the player
        // and update the dps of the player

        #if DEBUG
        this.plugin.DebugLogger.AddEntry($"Calculating DoT duration");
        #endif
        foreach (var member in partyMembers)
        {
            bool active = false;
            double duration = -1;
            double timestamp = -1;
            string name = "";

            bool active2 = false;
            double duration2 = -1;
            double timestamp2 = -1;
            string name2 = "";
            
            foreach (var debuff in member.debuffs)
            {
                if (!IsSecondDOT(debuff))
                {
                    name = debuff.name;
                    if (!active)
                    {
                        active = true;
                        duration = debuffDurations[debuffs.IndexOf(debuff.debuff)];
                        timestamp = debuff.timestamp;
                    }
                    else if (debuff.timestamp - timestamp > debuffDurations[debuffs.IndexOf(debuff.debuff)])
                    {
                        duration += debuffDurations[debuffs.IndexOf(debuff.debuff)];
                        timestamp = debuff.timestamp;
                    }
                    else
                    {
                        duration += debuff.timestamp - timestamp;
                        timestamp = debuff.timestamp;
                    }
                    
                }
                else
                {
                    name2 = debuff.name;
                    if (!active2)
                    {
                        active2 = true;
                        duration2 = debuffDurations[debuffs.IndexOf(debuff.debuff)];
                        timestamp2 = debuff.timestamp;
                    }
                    else if (debuff.timestamp - timestamp2 > debuffDurations[debuffs.IndexOf(debuff.debuff)])
                    {
                        duration2 += debuffDurations[debuffs.IndexOf(debuff.debuff)];
                        timestamp2 = debuff.timestamp;
                    }
                    else
                    {
                        duration2 += debuff.timestamp - timestamp2;
                        timestamp2 = debuff.timestamp;
                    }
                }
                active = false;
                active2 = false;
            }

            double endTimestamp = damageLog.Last().timestamp;
            double lastDOTEndingAt = timestamp + debuffDurations[debuffs.IndexOf(name)];
            if (endTimestamp > lastDOTEndingAt)
            {
                duration -= endTimestamp - lastDOTEndingAt;
            }

            if (active)
            {
                member.debuffDuration = duration;
            }
            
            double endTimestamp2 = damageLog.Last().timestamp;
            double lastDOTEndingAt2 = timestamp2 + debuffDurations[debuffs.IndexOf(name2)];
            if (endTimestamp > lastDOTEndingAt)
            {
                duration2 -= endTimestamp2 - lastDOTEndingAt2;
            }

            if (active2)
            {
                member.debuff2Duration = duration2;
            }
            
            for (int i = 0; i < partyMembers.Count; i++)
            {
                if (partyMembers[i].name == member.name)
                {
                    #if DEBUG
                    this.plugin.DebugLogger.AddEntry($"Updating party member {member.name} with {member.debuffDuration} debuff duration and {member.debuff2Duration} debuff2 duration");
                    #endif
                    partyMembers[i].debuffDuration = member.debuffDuration;
                    partyMembers[i].debuff2Duration = member.debuff2Duration;
                    break;
                }
            }
        }
        
        
    }

    private void CalcDotDmg()
    {
        #if DEBUG
        this.plugin.DebugLogger.AddEntry($"Calculating DoT damage");
        #endif
        foreach (var member in partyMembers)
        {
            string dot = "";
            string dot2 = "";
            foreach (var debuff in member.debuffs)
            {
                if (!IsSecondDOT(debuff))
                {
                    dot = debuff.debuff;
                }
                else
                {
                    dot2 = debuff.debuff;
                }
            }

            double dotPot = (debuffPotencies[debuffs.IndexOf(dot)] * member.debuffDuration / 3);
            int dotDmg = (int)Math.Round(dotPot * member.damage / member.potency);

            if (dot2 == "")
            {
                member.damage += dotDmg;
            }
            else
            {
                double dotPot2 = debuffPotencies[debuffs.IndexOf(dot2)] * member.debuff2Duration / 3;
                int dotDmg2 = (int)Math.Round(dotPot2 * member.damage / member.potency);
                member.damage += dotDmg + dotDmg2;
            }
            for (int i = 0; i < partyMembers.Count; i++)
            {
                if (partyMembers[i].name == member.name)
                {
                    #if DEBUG
                    this.plugin.DebugLogger.AddEntry($"Updating party member {member.name} with {member.damage} damage");
                    #endif
                    partyMembers[i].damage = member.damage;
                    partyMembers[i].dps = partyMembers[i].damage / GetFightDuration();
                    break;
                }
            }
        }
    }

    private bool IsSecondDOT(DebuffEntry debuff)
    {
        #if DEBUG
        this.plugin.DebugLogger.AddEntry($"Checking if {debuff.debuff} is a second DoT");
        #endif

        if (debuff.debuff == "windbite")
        {
            return true;
        }
        if (debuff.debuff == "stormbite")
        {
            return true;
        }
        if (debuff.debuff == "baneful impaction")
        {
            return true;
        }
        return false;
    }

    private double GetFightDuration()
    {
        #if DEBUG
        this.plugin.DebugLogger.AddEntry($"Getting fight duration");
        #endif
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
    
    public List<DebuffEntry> GetDebuffLog()
    {
        return debuffLog;
    }
    
    public void Reset()
    {
#if DEBUG
        this.plugin.DebugLogger.AddEntry("Clearing damage log.");
#endif
        damageLog.Clear();
#if DEBUG
        this.plugin.DebugLogger.AddEntry("Clearing party member list.");
#endif
        partyMembers.Clear();
#if DEBUG
        this.plugin.DebugLogger.AddEntry("Clearing debuff log.");
#endif
        debuffLog.Clear();
    }

    public void HandleEvent(string name, int damage, int potency, double timestamp)
    {
        #if DEBUG
        this.plugin.DebugLogger.AddEntry($"Handling event for {name} with {damage} damage and {potency} potency at {timestamp}");
        #endif
        if (IsPartyMember(name))
        {
            UpdatePartyMemberDamage(name, damage, potency);
            AddDamageEntry(name, damage, potency, timestamp);
            CalcDOTDuration();
            CalcDotDmg();
        }
        else
        {
            AddPartyMember(name);
            UpdatePartyMemberDamage(name, damage, potency);
            AddDamageEntry(name, damage, potency, timestamp);
            CalcDOTDuration();
            CalcDotDmg();
        }
    }

    public void HandleDebuffEvent(string name, string debuff, double time)
    {
        if (IsPartyMember(name))
        {
            AddDebuffEntry(name, debuff, time);
            CalcDOTDuration();
            CalcDotDmg();
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
