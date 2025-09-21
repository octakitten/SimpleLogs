using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Plugin.Services;
using System;
using Lumina.Excel.Sheets;
using Hexa.NET.ImPlot;

namespace SimpleLogs.Chat
{

    public static class Abilities
    {
        
        public static string[] debuffs =
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
            "high thunder ii"
        };

        public struct AST
        {
            public static string[] debuffs =
            {
                "combust",
                "combust ii",
                "combust iii",
            };
            
            public static int[] debuffPotencies = 
            {
                50,
                60,
                70
            };
            
            public static string[] abilities =
            {
                "malefic",
                "gravity",
                "malefic ii",
                "stellar burst",
                "stellar explosion",
                "malefic iii",
                "lord of crowns",
                "malefic iv",
                "fall malefic",
                "gravity ii",
                "macrocosmos",
                "oracle"
            };
        
            public static int[] abilityPotencies =
            {
                150,
                120,
                160,
                205,
                310,
                190,
                400,
                230,
                270,
                140,
                270,
                860
            };
        }

        public struct WHM
        {
            public static string[] debuffs =
            {
                "aero",
                "aero ii",
                "dia",
            };
            
            public static int[] debuffPotencies = 
            {
                30,
                50,
                80
            };

            public static string[] abilities =
            {
                "stone",
                "aero",
                "stone ii",
                "aero ii",
                "holy",
                "stone iii",
                "assize",
                "stone iv",
                "dia",
                "glare",
                "glare iii",
                "holy iii",
                "glare iv",
                "afflatus misery"
            };

            public static int[] abilityPotencies =
            {
                140,
                50,
                190,
                50,
                140,
                220,
                400,
                260,
                80,
                290,
                340,
                150,
                640,
                1360
            };
        }
        
        public struct SCH 
        {
            public static string[] debuffs =
            {
                "bio",
                "bio ii",
                "biolysis",
                "baneful impaction"
            };
            
            public static int[] debuffPotencies = 
            {
                20,
                40,
                80,
                140
            };
            
            public static string[] abilities =
            {
                "ruin",
                "ruin ii",
                "energy drain",
                "art of war",
                "broil",
                "broil ii",
                "broil iii",
                "broil iv",
                "art of war ii"
            };
        
            public static int[] abilityPotencies =
            {
                150,
                220,
                100,
                165,
                220,
                240,
                255,
                310,
                180,
            };
        }

        public struct SGE
        {
            public static string[] debuffs =
            {
                "eukrasian dosis",
                "eukrasian dosis ii",
                "eukrasian dosis iii",
                "eukrasian dyskrasia"
            };
            
            public static int[] debuffPotencies = 
            {
                40,
                60,
                80,
                40
            };

            public static string[] abilities =
            {
                "dosis",
                "phlegma",
                "dyskrasia",
                "toxikon",
                "dosis ii",
                "phlegma ii",
                "dosis iii",
                "phlegma iii",
                "dyskrasia ii",
                "toxikon ii",
                "pneuma",
                "psyche"
            };

            public static int[] abilityPotencies =
            {
                300,
                400,
                160,
                300,
                320,
                490,
                370,
                600,
                170,
                370,
                370,
                600
            };
        }

        public struct SAM
        {
            public static string[] debuffs = 
            {
                "higanbana"
            };
            
            public static int[] debuffPotencies = 
            {
                50
            };

            public static string[] abilities =
            {
                "hakaze",
                "jinpu",
                "enpi",
                "shifu",
                "fuga",
                "gekko",
                "higanbana",
                "tenka goken",
                "midare setsugekka",
                "mangetsu",
                "kasha",
                "oka",
                "yukikaze",
                "hissatsu: shinten",
                "hissatsu: gyoten",
                "hissatsu: yaten",
                "hissatsu: kyuten",
                "hissatsu: guren",
                "hissatsu: senei",
                "shoha",
                "fuko",
                "ogi namikiri",
                "kaeshi: namikiri",
                "gyofu",
                "zanshin",
                "kaeshi: goken",
                "kaeshi: setsugekka",
                "tendo goken"
            };

	    public static int[] abilityPotencies =
	    {
            200,
            300,
            270,
            300,
            90,
            420,
            200,
            300,
            640,
            120,
            420,
            120,
            340,
            250,
            100,
            100,
            100,
            400,
            800,
            640,
            100,
            1000,
            1000,
            240,
            940,
            300,
            640,
            410
	    };
        }

	public struct BRD
	{
	    public static string[] debuffs = 
	    {
            "venomous bite",
            "windbite",
            "caustic bite",
            "stormbite"
	    };

	    public static int[] debuffPotencies = 
	    {
            15,
            20,
            20,
            25
	    };

	    public static string[] abilities = 
	    {
            "heavy shot",
            "straight shot",
            "venomous bite",
            "bloodletter",
            "quick nock",
            "wide volley",
            "windbite",
            "rain of death",
            "pitch perfect",
            "empyreal arrow",
            "iron jaws",
            "sidewinder",
            "caustic bite",
            "stormbite",
            "refulgent arrow",
            "shadowbite",
            "burst shot",
            "apex arrow",
            "ladonsbite",
            "blast arrow",
            "heartbreak shot",
            "resonant arrow",
            "radiant encore",
	    };

	    public static int[] abilityPotencies = 
	    {
            160,
            200,
            100,
            130,
            110,
            140,
            60,
            100,
            360,
            260,
            100,
            400,
            150,
            100,
            280,
            180,
            220,
            600,
            140,
            600,
            180,
            600,
            900
	    };
	}

	public struct BLM
	{
	    public static string[] debuffs = 
        {
		    "thunder",
		    "thunder ii",
		    "thunder iii",
		    "thunder iv",
		    "high thunder",
		    "high thunder ii"
		};

	    public static int[] debuffPotencies = 
	    {
            45,
            30,
            50,
            35,
            60,
            40
	    };

	    public static string[] abilities = 
	    {
            "blizzard",
            "fire",
            "thunder",
            "blizzard ii",
            "scathe",
            "fire ii",
            "thunder ii",
            "fire iii",
            "blizzard iii",
            "freeze",
            "thunder iii",
            "flare",
            "blizzard iv",
            "fire iv",
            "thunder iv",
            "foul",
            "despair",
            "xenoglossy",
            "high fire ii",
            "high blizzard ii",
            "paradox",
            "high thunder",
            "high thunder ii",
            "flare star"
	    };

	    public static int[] abilityPotencies = 
	    {
            180,
            180,
            100,
            80,
            100,
            80,
            60,
            290,
            290,
            120,
            120,
            240,
            300,
            300,
            80,
            600,
            350,
            890,
            100,
            100,
            540,
            150,
            100,
            500
	    };

	}
        
    }

    public class Log
    {
        private Plugin plugin;
        private ChatEvent lastEvent;
        private List<ChatEvent> chatLog = new List<ChatEvent>();
        private Utilities.Timer timer;
        private bool casting;
        private string castingPlayer = "";
        private int castingPotency = -1;
        private const string castingType = "2091";
        private const string debuffType = "2735";
        private const string damageType1 = "2729";
        private const string damageType2 = "2857";

    

    public struct ChatEvent
        {
            public string type;
            public string sender;
            public string message;
            public double timestamp;
        }

        public Log(Plugin plugin, Utilities.Timer tmr)
        {
            timer = new Utilities.Timer();
            this.plugin = plugin;
            this.timer = tmr;
            // ChatEvent lastEvent = new ChatEvent();
            // delegate = new IChatGui.OnMessageDelegate(OnChatMessage);
            // Subscribe to the ChatMessage event
            this.plugin.ChatGui.ChatMessage += new IChatGui.OnMessageDelegate(OnChatMessage);
        }

        private void OnChatMessage(XivChatType type, int timestamp, ref SeString sender, ref SeString message, ref bool isHandled)
        {
            #if DEBUG
            this.plugin.DebugLogger.AddEntry($"Chat message received: {type} {sender} {message}");
            #endif
            // Record the chat message to the chat log
            ChatEvent newEvent = new ChatEvent();
            newEvent.type = type.ToString().ToLower();
            newEvent.sender = sender.ToString().ToLower();
            newEvent.message = message.ToString().ToLower();
            newEvent.timestamp = timer.GetElapsedTime().TotalSeconds;
            AnalyzeChatMessage(newEvent);
        }


        public void AnalyzeChatMessage(ChatEvent chatEvent)
        {
            #if DEBUG
            this.plugin.DebugLogger.AddEntry($"Analyzing chat message: {chatEvent.type} {chatEvent.sender} {chatEvent.message}");
            #endif
            if (IsDebuffEvent(chatEvent))
            {
                chatEvent.sender = castingPlayer;
                HandleDebuffEvent(chatEvent);
            }
            else if (casting)
            {
                chatEvent.sender = castingPlayer;
                lastEvent = chatEvent;
                chatLog.Add(chatEvent);
                HandleDamageEvent(castingPlayer, chatEvent);
            }
            else if (IsCastEvent(chatEvent))
            {
                if (IsYouEvent(chatEvent))
                {
                    casting = true;
                    castingPlayer = "you";
                    chatEvent.sender = "you";
                    lastEvent = chatEvent;
                    chatLog.Add(chatEvent);
                }
                else
                {
                    casting = true;
                    string[] dummy = chatEvent.message.Split(' ');
                    if (IsServerName(dummy[2]))
                    {
                        castingPlayer = dummy[0] + " " + dummy[1] + " " + dummy[2];
                    }
                    else
                    {
                        castingPlayer = dummy[0] + " " + dummy[1];
                    }
                    chatEvent.sender = castingPlayer;
                    lastEvent = chatEvent;
                    chatLog.Add(chatEvent);
                }
            }
            else if (IsDamageEvent(chatEvent))
            {
                casting = false;
                ChatEvent newEvent = new ChatEvent();
                newEvent.type = chatEvent.type;
                newEvent.sender = chatEvent.sender;
                newEvent.timestamp = chatEvent.timestamp;
                newEvent.message = SanitizeDmgMsg(chatEvent.message);
                if (IsYouEvent(newEvent))
                {
                    newEvent.sender = "you";
                    lastEvent = newEvent;
                    chatLog.Add(newEvent);
                    HandleDamageEvent("you", newEvent);
                }
                else
                {
                    newEvent.sender = castingPlayer;
                    lastEvent = newEvent;
                    chatLog.Add(newEvent);
                    HandleDamageEvent(castingPlayer, newEvent);
                }
            }
            else if (IsAutoDmgEvent(chatEvent))
            {
                casting = false;
                ChatEvent newEvent = new ChatEvent();
                newEvent.type = chatEvent.type;
                newEvent.sender = chatEvent.sender;
                newEvent.timestamp = chatEvent.timestamp;
                newEvent.message = SanitizeDmgMsg(chatEvent.message);
                string[] newWords = newEvent.message.Split(" ");
                if (IsYouEvent(chatEvent))
                {
                    newEvent.sender = "you";
                    lastEvent = newEvent;
                    chatLog.Add(newEvent);
                    HandleDamageEvent("you", chatEvent);
                }
                else
                {
                    if (IsServerName(newWords[2]))
                    {
                        newEvent.sender = newWords[0] + " " + newWords[1] + " " + newWords[2];
                        lastEvent = newEvent;
                        chatLog.Add(newEvent);
                        HandleDamageEvent(newWords[0] + " " + newWords[1] + " " + newWords[2], newEvent);
                    }
                    else
                    {
                        newEvent.sender = newWords[0] + " " + newWords[1];
                        lastEvent = newEvent;
                        HandleDamageEvent(newWords[0] + " " + newWords[1], newEvent);
                    }
                }
            }
            else
            {
                casting = false;
            }
            
        }

        public void AnalyzeChatLog()
        {
            #if DEBUG
            this.plugin.DebugLogger.AddEntry("Analyzing chat log");
            #endif
            plugin.DamageMeter.Reset();
            for (int entry = 0; entry < chatLog.Count; entry++)
            {
                AnalyzeChatMessage(chatLog[entry]);
            }
        }

        private void HandleDamageEvent(string player, ChatEvent cEvent)
        {
            #if DEBUG
            this.plugin.DebugLogger.AddEntry($"Handling damage event: {player} {cEvent.type} {cEvent.sender} {cEvent.message}");
            #endif
            //plugin.Configuration.handledDamageEvent = true;
            string[] words = cEvent.message.Split(' ');
            foreach (var word in words)
            {
                try
                {
                    int dmg = int.Parse(word);
                    plugin.DamageMeter.HandleEvent(player, dmg, castingPotency, cEvent.timestamp);
                    break;
                }
                catch
                {
                    //do nothing if it fails
                }
            }
        }

        private void HandleDebuffEvent(ChatEvent cEvent)
        {
            #if DEBUG
            this.plugin.DebugLogger.AddEntry($"Handling debuff event: {cEvent.type} {cEvent.sender} {cEvent.message}");
            #endif
            if (IsYouEvent(cEvent))
            {
                cEvent.sender = "you";
                lastEvent = cEvent;
                chatLog.Add(cEvent);
                bool done = false;
                foreach (var dbf in Abilities.AST.debuffs)
                {
                    if (cEvent.message.Contains(dbf))
                    {
                        plugin.DamageMeter.HandleDebuffEvent("you", dbf, cEvent.timestamp);
                        done = true;
                        break;
                    }
                }

                if (!done)
                {
                    foreach (var dbf in Abilities.WHM.debuffs)
                    {
                        if (cEvent.message.Contains(dbf))
                        {
                            plugin.DamageMeter.HandleDebuffEvent("you", dbf, cEvent.timestamp);
                            done = true;
                            break;
                        }
                    }
                }

                if (!done)
                {
                    foreach (var dbf in Abilities.SCH.debuffs)
                    {
                        if (cEvent.message.Contains(dbf))
                        {
                            plugin.DamageMeter.HandleDebuffEvent("you", dbf, cEvent.timestamp);
                            done = true;
                            break;
                        }
                    }
                }

                if (!done)
                {
                    foreach (var dbf in Abilities.SGE.debuffs)
                    {
                        if (cEvent.message.Contains(dbf))
                        {
                            plugin.DamageMeter.HandleDebuffEvent("you", dbf, cEvent.timestamp);
                            done = true;
                            break;
                        }
                    }
                }

                if (!done)
                {
                    foreach (var dbf in Abilities.SAM.debuffs)
                    if (cEvent.message.Contains(dbf))
                    {
                        plugin.DamageMeter.HandleDebuffEvent("you", dbf, cEvent.timestamp);
                        done = true;
                        break;
                    }
                }

                if (!done)
                {
                    foreach (var dbf in Abilities.BRD.debuffs)
                    if (cEvent.message.Contains(dbf))
                    {
                        plugin.DamageMeter.HandleDebuffEvent("you", dbf, cEvent.timestamp);
                        done = true;
                        break;
                    }
                }

                if (!done)
                {
                    foreach (var dbf in Abilities.BLM.debuffs)
                    if (cEvent.message.Contains(dbf))
                    {
                        plugin.DamageMeter.HandleDebuffEvent("you", dbf, cEvent.timestamp);
                        done = true;
                        break;
                    }
                }
                
            }
            else
            {
                bool done = false;
                foreach (var dbf in Abilities.AST.debuffs)
                {
                    if (cEvent.message.Contains(dbf))
                    {
                        string[] dummy = cEvent.message.Split(' ');
                        if (IsServerName(dummy[2]))
                        {
                            cEvent.sender = dummy[0] + ' ' + dummy[1] + ' ' + dummy[2];
                            lastEvent = cEvent;
                            chatLog.Add(cEvent);
                            plugin.DamageMeter.HandleDebuffEvent(cEvent.sender, dbf, cEvent.timestamp);
                        }
                        else
                        {
                            cEvent.sender = dummy[0] + ' ' + dummy[1];
                            lastEvent = cEvent;
                            chatLog.Add(cEvent);
                            plugin.DamageMeter.HandleDebuffEvent(cEvent.sender, dbf, cEvent.timestamp);
                        }
                        done = true;
                        break;
                    }
                }

                if (!done)
                {
                    foreach (var dbf in Abilities.WHM.debuffs)
                    {
                        if (cEvent.message.Contains(dbf))
                        {
                            string[] dummy = cEvent.message.Split(' ');
                            if (IsServerName(dummy[2]))
                            {
                                cEvent.sender = dummy[0] + ' ' + dummy[1] + ' ' + dummy[2];
                                lastEvent = cEvent;
                                chatLog.Add(cEvent);
                                plugin.DamageMeter.HandleDebuffEvent(cEvent.sender, dbf, cEvent.timestamp);
                            }
                            else
                            {
                                cEvent.sender = dummy[0] + ' ' + dummy[1];
                                lastEvent = cEvent;
                                chatLog.Add(cEvent);
                                plugin.DamageMeter.HandleDebuffEvent(cEvent.sender, dbf, cEvent.timestamp);
                            }
                            done = true;
                            break;
                        }
                    }
                }

                if (!done)
                {
                    foreach (var dbf in Abilities.SCH.debuffs)
                    {
                        if (cEvent.message.Contains(dbf))
                        {
                            string[] dummy = cEvent.message.Split(' ');
                            if (IsServerName(dummy[2]))
                            {
                                cEvent.sender = dummy[0] + ' ' + dummy[1] + ' ' + dummy[2];
                                lastEvent = cEvent;
                                chatLog.Add(cEvent);
                                plugin.DamageMeter.HandleDebuffEvent(cEvent.sender, dbf, cEvent.timestamp);
                            }
                            else
                            {
                                cEvent.sender = dummy[0] + ' ' + dummy[1];
                                lastEvent = cEvent;
                                chatLog.Add(cEvent);
                                plugin.DamageMeter.HandleDebuffEvent(cEvent.sender, dbf, cEvent.timestamp);
                            }
                            done = true;
                            break;
                        }
                    }
                }

                if (!done)
                {
                    foreach (var dbf in Abilities.SGE.debuffs)
                    {
                        if (cEvent.message.Contains(dbf))
                        {
                            string[] dummy = cEvent.message.Split(' ');
                            if (IsServerName(dummy[2]))
                            {
                                cEvent.sender = dummy[0] + ' ' + dummy[1] + ' ' + dummy[2];
                                lastEvent = cEvent;
                                chatLog.Add(cEvent);
                                plugin.DamageMeter.HandleDebuffEvent(cEvent.sender, dbf, cEvent.timestamp);
                            }
                            else
                            {
                                cEvent.sender = dummy[0] + ' ' + dummy[1];
                                lastEvent = cEvent;
                                chatLog.Add(cEvent);
                                plugin.DamageMeter.HandleDebuffEvent(cEvent.sender, dbf, cEvent.timestamp);
                            }
                            done = true;
                            break;
                        }
                    }
                }

                if (!done)
                {
                    foreach (var dbf in Abilities.SAM.debuffs)
                    {
                        if (cEvent.message.Contains(dbf))
                        {
                            string[] dummy = cEvent.message.Split(' ');
                            if (IsServerName(dummy[2]))
                            {
                                cEvent.sender = dummy[0] + ' ' + dummy[1] + ' ' + dummy[2];
                                lastEvent = cEvent;
                                chatLog.Add(cEvent);
                                plugin.DamageMeter.HandleDebuffEvent(cEvent.sender, dbf, cEvent.timestamp);
                            }
                            else
                            {
                                cEvent.sender = dummy[0] + ' ' + dummy[1];
                                lastEvent = cEvent;
                                chatLog.Add(cEvent);
                                plugin.DamageMeter.HandleDebuffEvent(cEvent.sender, dbf, cEvent.timestamp);
                            }
                            done = true;
                            break;
                        }
                    }
                }

                if (!done)
                {
                    foreach (var dbf in Abilities.BRD.debuffs)
                    {
                        if (cEvent.message.Contains(dbf))
                        {
                            string[] dummy = cEvent.message.Split(' ');
                            if (IsServerName(dummy[2]))
                            {
                                cEvent.sender = dummy[0] + ' ' + dummy[1] + ' ' + dummy[2];
                                lastEvent = cEvent;
                                chatLog.Add(cEvent);
                                plugin.DamageMeter.HandleDebuffEvent(cEvent.sender, dbf, cEvent.timestamp);
                            }
                            else
                            {
                                cEvent.sender = dummy[0] + ' ' + dummy[1];
                                lastEvent = cEvent;
                                chatLog.Add(cEvent);
                                plugin.DamageMeter.HandleDebuffEvent(cEvent.sender, dbf, cEvent.timestamp);
                            }
                            done = true;
                            break;
                        }
                    }
                }

                if (!done)
                {
                    foreach (var dbf in Abilities.BLM.debuffs)
                    {
                        if (cEvent.message.Contains(dbf))
                        {
                            string[] dummy = cEvent.message.Split(' ');
                            if (IsServerName(dummy[2]))
                            {
                                cEvent.sender = dummy[0] + ' ' + dummy[1] + ' ' + dummy[2];
                                lastEvent = cEvent;
                                chatLog.Add(cEvent);
                                plugin.DamageMeter.HandleDebuffEvent(cEvent.sender, dbf, cEvent.timestamp);
                            }
                            else
                            {
                                cEvent.sender = dummy[0] + ' ' + dummy[1];
                                lastEvent = cEvent;
                                chatLog.Add(cEvent);
                                plugin.DamageMeter.HandleDebuffEvent(cEvent.sender, dbf, cEvent.timestamp);
                            }
                            done = true;
                            break;
                        }
                    }
                }
            }
        }

        private string FindAbility(ChatEvent cEvent)
        {
            #if DEBUG
            this.plugin.DebugLogger.AddEntry($"Finding ability: {cEvent.type} {cEvent.sender} {cEvent.message}");
            #endif
            if (IsYouEvent(cEvent))
            {
                foreach (var abl in Abilities.AST.abilities)
                {
                    if (cEvent.message.Contains(abl))
                    {
                        return abl;
                    }
                }
                foreach (var abl in Abilities.WHM.abilities)
                {
                    if (cEvent.message.Contains(abl))
                    {
                        return abl;
                    }
                }
                foreach (var abl in Abilities.SCH.abilities)
                {
                    if (cEvent.message.Contains(abl))
                    {
                        return abl;
                    }
                }
                foreach (var abl in Abilities.SGE.abilities)
                {
                    if (cEvent.message.Contains(abl))
                    {
                        return abl;
                    }
                }
                foreach (var abl in Abilities.SAM.abilities)
                {
                    if (cEvent.message.Contains(abl))
                    {
                        return abl;
                    }
                }
                foreach (var abl in Abilities.BRD.abilities)
                {
                    if (cEvent.message.Contains(abl))
                    {
                        return abl;
                    }
                }
                foreach (var able in Abilities.BLM.abilities)
                {
                    if (cEvent.message.Contains(able))
                    {
                        return able;
                    }
                }
            }
            else
            {
                string newMsg = RemoveNameFromMsg(cEvent.message);
                foreach (var abl in Abilities.AST.abilities)
                {
                    if (newMsg.Contains(abl))
                    {
                        return abl;
                    }
                }
                foreach (var abl in Abilities.WHM.abilities)
                {
                    if (newMsg.Contains(abl))
                    {
                        return abl;
                    }
                }
                foreach (var abl in Abilities.SCH.abilities)
                {
                    if (newMsg.Contains(abl))
                    {
                        return abl;
                    }
                }
                foreach (var abl in Abilities.SGE.abilities)
                {
                    if (newMsg.Contains(abl))
                    {
                        return abl;
                    }
                }
                foreach (var abl in Abilities.SAM.abilities)
                {
                    if (newMsg.Contains(abl))
                    {
                        return abl;
                    }
                }
                foreach (var abl in Abilities.BRD.abilities)
                {
                    if (newMsg.Contains(abl))
                    {
                        return abl;
                    }
                }
                foreach (var abl in Abilities.BLM.abilities)
                {
                    if (newMsg.Contains(abl))
                    {
                        return abl;
                    }
                }
            }

            return "failed!";
        }

        private int GetPotency(string ability)
        {
            #if DEBUG
            this.plugin.DebugLogger.AddEntry($"Getting potency for ability: {ability}");
            #endif
            int ptn = -1;
            for (int i = 0; i < Abilities.AST.abilities.Length; i++)
            {
                if (ability == Abilities.AST.abilities[i])
                {
                    ptn = Abilities.AST.abilityPotencies[i];
                    return ptn;
                }
            }
            for (int i = 0; i < Abilities.WHM.abilities.Length; i++)
            {
                if (ability == Abilities.WHM.abilities[i])
                {
                    ptn = Abilities.WHM.abilityPotencies[i];
                    return ptn;
                }
            }
            for (int i = 0; i < Abilities.SCH.abilities.Length; i++)
            {
                if (ability == Abilities.SCH.abilities[i])
                {
                    ptn = Abilities.SCH.abilityPotencies[i];
                    return ptn;
                }
            }
            for (int i = 0; i < Abilities.SGE.abilities.Length; i++)
            {
                if (ability == Abilities.SGE.abilities[i])
                {
                    ptn = Abilities.SGE.abilityPotencies[i];
                    return ptn;
                }
            }
            for (int i = 0; i < Abilities.SAM.abilities.Length; i++)
            {
                if (ability == Abilities.SAM.abilities[i])
                {
                    ptn = Abilities.SAM.abilityPotencies[i];
                    return ptn;
                }
            }
            for (int i = 0; i < Abilities.BRD.abilities.Length; i++)
            {
                if (ability == Abilities.BRD.abilities[i])
                {
                    ptn = Abilities.BRD.abilityPotencies[i];
                    return ptn;
                }
            }
            for (int i = 0; i < Abilities.BLM.abilities.Length; i++)
            {
                if (ability == Abilities.BLM.abilities[i])
                {
                    ptn = Abilities.BLM.abilityPotencies[i];
                    return ptn;
                }
            }

            return ptn;
        }

        private static string[] ServerNames =
        {
            "adamantoise",
            "cactuar",
            "faerie",
            "gilgamesh",
            "jenova",
            "midgardsormr",
            "sargatanas",
            "siren",
            "balmung",
            "brynhildr",
            "coeurl",
            "diablos",
            "goblin",
            "malboro",
            "mateus",
            "zalera",
            "cuchulainn",
            "golem",
            "halicarnassus",
            "kraken",
            "maduin",
            "marilith",
            "rafflesia",
            "seraph",
            "behemoth",
            "excalibur",
            "exodus",
            "famfrit",
            "hyperion",
            "lamia",
            "leviathan",
            "ultros",
            "cerberus",
            "louisoix",
            "moogle",
            "omega",
            "phantom",
            "ragnarok",
            "sagittarius",
            "spriggan",
            "alpha",
            "lichl",
            "odin",
            "phoenix",
            "raiden",
            "shiva",
            "twintania",
            "zodiark",
            "bismarck",
            "ravana",
            "sephirot",
            "sophia",
            "zurvan",
            "aegis",
            "atomos",
            "carbuncle",
            "garuda",
            "gungnir",
            "kujata",
            "tonberry",
            "typhon",
            "alexander",
            "bahamut",
            "durandal",
            "fenrir",
            "ifrit",
            "ridill",
            "tiamat",
            "ultima",
            "anima",
            "asura",
            "chocobo",
            "hades",
            "ixion",
            "masamune",
            "pandaemonium",
            "titan",
            "belias",
            "mandragora",
            "ramuh",
            "shinryu",
            "unicorn",
            "valefor",
            "yojimbo",
            "zeromus"
        };

        private bool IsServerName(string word)
        {
            word = word.ToLower();
            #if DEBUG
            this.plugin.DebugLogger.AddEntry($"Checking if {word} is a server name");
            #endif
            foreach (var serverName in ServerNames)
            {
                if (word.ToLower() == serverName)
                {
                    return true;
                }
            }

            return false;
        }

        private string RemoveNameFromMsg(string msg)
        {
            #if DEBUG
            this.plugin.DebugLogger.AddEntry($"Removing name from message: {msg}");
            #endif
            if (msg.Contains(' '))
            {
                string[] words = msg.Split(' ');
                string newMsg = "";
                if (IsServerName(words[2]))
                {
                    for (int i = 3; i < words.Length - 1; i++)
                    {
                        newMsg += words[i] + " ";
                    }
                    newMsg += words.Last();
                }
                else 
                {
                    for (int i = 2; i < words.Length - 1; i++)
                    {
                        newMsg += words[i] + " ";
                    }
                    newMsg += words.Last();
                }

                return newMsg;
            }
            return "";
        }
        
        private bool IsCastEvent(ChatEvent cEvent)
        {
            #if DEBUG
            this.plugin.DebugLogger.AddEntry($"Checking if {cEvent.type} is a cast event");
            #endif
            if (cEvent.type == castingType)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        private bool IsDebuffEvent(ChatEvent cEvent)
        {
            #if DEBUG
            this.plugin.DebugLogger.AddEntry($"Checking if {cEvent.type} is a debuff event");
            #endif
            foreach (string dbf in Abilities.debuffs)
            {
                if (cEvent.message.Contains(dbf))
                {
                    return true;
                }

            }
                return false;
        }
        
        private bool IsDamageEvent(ChatEvent cEvent)
        {
            #if DEBUG
            this.plugin.DebugLogger.AddEntry($"Checking if {cEvent.type} is a damage event");
            #endif
            if (cEvent.type == damageType1 || cEvent.type == damageType2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsYouEvent(ChatEvent cEvent)
        {
            #if DEBUG
            this.plugin.DebugLogger.AddEntry($"Checking if {cEvent.sender} is you");
            #endif
            if (cEvent.sender == "you")
            {
                return true;
            }
            string[] words;
            if (cEvent.message.Contains(' '))
            {
                words = cEvent.message.Split(' ');
                if (words[0] == "you")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        private bool IsAutoDmgEvent(ChatEvent cEvent)
        {
            #if DEBUG
            this.plugin.DebugLogger.AddEntry($"Checking if {cEvent.type} is an auto damage event");
            #endif
            string[] words;
            if (cEvent.message.Contains(' '))
            {
                words = cEvent.message.Split(' ');
                if (words.Length < 4) {
                    return false;
                }

                if (words[0] == "you" && words[1] == "hit")
                {
                    return true;
                }
                
                if (words[3] == "hits")
                {
                    return true;
                }

                if (words [2] == "hits")
                {
                    return true;
                }
            }

            return false;
        }

        private string SanitizeDmgMsg(string msg)
        {
            #if DEBUG
            this.plugin.DebugLogger.AddEntry($"Sanitizing damage message of crit and dhit messages: {msg}");
            #endif
            msg = msg.ToLower();
            string[] wordsCheck = msg.Split(" ");
            string[] removes = {"critical", "critical!", "direct", "hit!"};
            List<string> dummy = new List<string>();
            if (wordsCheck[0] == removes[1])
            {
                for (int i = 1; i < wordsCheck.Length; i++)
                {
                    dummy.Append(wordsCheck[i]);
                }
            } 
            else if (wordsCheck[1] == removes[3])
            {
                for (int i = 2; i < wordsCheck.Length; i++)
                {
                    dummy.Append(wordsCheck[i]);
                }
            }
            else if (wordsCheck[2] == removes[3])
            {
                for (int i = 3; i < wordsCheck.Length; i++)
                {
                    dummy.Append(wordsCheck[i]);
                }
            }

            string[] words = dummy.ToArray();
            string message = "";
            for (int i = 0; i < words.Length - 1; i++)
            {
                message += words[i] + " ";
            }

            message += words.Last();
            return message;
        }

        public void Reset()
        {
            chatLog.Clear();
        }

        public List<ChatEvent> GetChatLog()
        {
            return chatLog;
        }

        public void Dispose()
        {
            // Unsubscribe from the ChatMessage event
            this.plugin.ChatGui.ChatMessage -= new IChatGui.OnMessageDelegate(OnChatMessage);
        }
    }

}
