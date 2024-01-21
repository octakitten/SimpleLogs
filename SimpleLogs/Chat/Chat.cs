using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text;
using System.Collections.Generic;
using System.Linq;

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
            "eukrasian dosis",
            "eukrasian dosis ii",
            "eukrasian dosis iii",
        };

        public struct AST
        {
            public static string[] debuffs =
            {
                "combust",
                "combust ii",
                "combust iii",
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
                "gravity ii"
            };
        
            public static int[] abilityPotencies =
            {
                150,
                120,
                160,
                205,
                310,
                190,
                250,
                230,
                250,
                130
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

            public static string[] abilities =
            {

            };

            public static int[] potencies =
            {

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
            // Subscribe to the ChatMessage event
            this.plugin.ChatGui.ChatMessage += OnChatMessage;
        }

        private void OnChatMessage(XivChatType type, uint senderId, ref SeString sender, ref SeString message,
            ref bool isHandled)
        {
            // Record the chat message to the chat log
            lastEvent.type = type.ToString().ToLower();
            lastEvent.sender = sender.TextValue.ToLower();
            lastEvent.message = message.TextValue.ToLower();
            lastEvent.timestamp = timer.GetElapsedTime().TotalSeconds;
            chatLog.Add(lastEvent);
        }

        public void AnalyzeChatMessage(ChatEvent chatEvent)
        {
            if (casting)
            {
                HandleDamageEvent(castingPlayer, chatEvent);
            }

            if (IsCastEvent(chatEvent))
            {
                
            }
            
        }

        public void AnalyzeChatLog()
        {
            plugin.DamageMeter.Reset();
            for (int entry = 0; entry < chatLog.Count; entry++)
            {
                    string[] words = chatLog[entry].message.Split(' ');
                    int current = 0;
                    if (IsDebuffEvent(chatLog[entry]))
                    {
                        HandleDebuffEvent(chatLog[entry]);
                    }
                    if (casting)
                    {
                        HandleDamageEvent(castingPlayer, chatLog[entry]);
                    }
                    else if (IsCastEvent(chatLog[entry]))
                    {
                        if (IsYouEvent(chatLog[entry]))
                        {
                            casting = true;
                            castingPlayer = "you";
                        }
                        else
                        {
                            casting = true;
                            castingPlayer = words[current] + " " + words[current + 1] + " " + words[current + 2];
                        }
                    } 
                    else if (IsDamageEvent(chatLog[entry]))
                    {
                        ChatEvent newEvent = new ChatEvent();
                        newEvent.type = chatLog[entry].type;
                        newEvent.sender = chatLog[entry].sender;
                        newEvent.timestamp = chatLog[entry].timestamp;
                        newEvent.message = SanitizeDmgMsg(chatLog[entry].message);
                        string[] newWords = newEvent.message.Split(" ");
                        if (IsYouEvent(newEvent))
                        {
                            HandleDamageEvent("you", newEvent);
                        }
                        else
                        {
                            HandleDamageEvent(castingPlayer, newEvent);
                        }
                    }
                    else if (IsAutoDmgEvent(chatLog[entry]))
                    {
                        casting = false;
                        ChatEvent newEvent = new ChatEvent();
                        newEvent.type = chatLog[entry].type;
                        newEvent.sender = chatLog[entry].sender;
                        newEvent.timestamp = chatLog[entry].timestamp;
                        newEvent.message = SanitizeDmgMsg(chatLog[entry].message);
                        string[] newWords = newEvent.message.Split(" ");
                        if (IsYouEvent(chatLog[entry]))
                        {
                            HandleDamageEvent("you", chatLog[entry]);
                        }
                        else
                        {
                            HandleDamageEvent(newWords[current] + " " + newWords[current + 1] + " " + newWords[current + 2], newEvent);
                        }
                    }
                    else
                    {
                        casting = false;
                    }
            }
        }

        private void HandleDamageEvent(string player, ChatEvent cEvent)
        {
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
            if (IsYouEvent(cEvent))
            {
                for (var i = 0; i < Abilities.debuffs.Length; i++)
                {
                    if (cEvent.message.Contains(Abilities.debuffs[i]))
                    {
                        plugin.DamageMeter.HandleDebuffEvent("you", Abilities.debuffs[i], cEvent.timestamp);
                        break;
                    }
                }
            }
            else
            {
                for (var i = 0; i < Abilities.debuffs.Length; i++)
                {
                    if (cEvent.message.Contains(Abilities.debuffs[i]))
                    {
                        string[] dummy = cEvent.message.Split(' ');
                        plugin.DamageMeter.HandleDebuffEvent(dummy[0] + ' ' + dummy[1] + ' ' + dummy[2], Abilities.debuffs[i], cEvent.timestamp);
                        break;
                    }
                }
            }
        }
        
        private bool IsCastEvent(ChatEvent cEvent)
        {
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
            if (cEvent.type == debuffType)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        private bool IsDamageEvent(ChatEvent cEvent)
        {
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
            string[] words = cEvent.message.Split(' ');
            if (words[0] == "you")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsAutoDmgEvent(ChatEvent cEvent)
        {
            string[] words = cEvent.message.Split(' ');
            if (words[0] == "you" && words[1] == "hit")
            {
                return true;
            }
            
            if (words[3] == "hits")
            {
                return true;
            }

            return false;
        }

        private string SanitizeDmgMsg(string msg)
        {
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
            this.plugin.ChatGui.ChatMessage -= OnChatMessage;
        }
    }

}