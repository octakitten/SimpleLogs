using System;
using Dalamud.Game.Gui;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text;
using Dalamud.Plugin;
using System.Collections.Generic;
using System.Text;

namespace SimpleLogs.Chat
{

    public class Log
    {
        private Plugin plugin;
        private ChatEvent lastEvent;
        private List<ChatEvent> chatLog = new List<ChatEvent>();
        private Utilities.Timer timer;
        private bool casting = false;
        private string castingPlayer = "";
        private int castingPotency = -1;
        private const string castingType = "2091";
        private const string debuffType = "2735";
        private const string damageType1 = "2729";
        private const string damageType2 = "2857";

        private string[] debuffs =
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

        private string[] abilities =
        {
            "malefic",
            "malefic ii"
        };
        
        private int[] abilityPotencies =
        {
            150,
            160
        };
    

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
                if (true)
                {
                    
                    string[] words = chatLog[entry].message.Split(' ');
                    int current = 0;
                    if (IsDebuffEvent(chatLog[entry]))
                    {
                        
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
                        if (IsYouEvent(chatLog[entry]))
                        {
                            HandleDamageEvent("you", chatLog[entry]);
                        }
                        else
                        {
                            HandleDamageEvent(words[current] + " " + words[current + 1] + " " + words[current + 2], chatLog[entry]);
                        }
                    }
                    else if (IsAutoDmgEvent(chatLog[entry]))
                    {
                        casting = false;
                        if (IsYouEvent(chatLog[entry]))
                        {
                            HandleDamageEvent("you", chatLog[entry]);
                        }
                        else
                        {
                            HandleDamageEvent(words[current] + " " + words[current + 1] + " " + words[current + 2], chatLog[entry]);
                        }
                    }
                    else
                    {
                        casting = false;
                    }
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