using System;
using Dalamud.Game.Gui;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text;
using Dalamud.Plugin;
using System.Collections.Generic;
using System.Text;

namespace SimpleLogs
{

    public class ChatCombatLogger
    {
        private Plugin plugin;
        private ChatEvent lastEvent;
        private List<ChatEvent> chatLog = new List<ChatEvent>();
        private Timer timer;
        
        public struct ChatEvent
        {
            public string type;
            public string sender;
            public string message;
            public double timestamp;
        }

        public ChatCombatLogger(Plugin plugin, Timer tmr)
        {
            timer = new Timer();
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

        public void AnalyzeChatLog()
        {
            plugin.DamageMeter.Reset();
            bool lastCast = false;
            string lastPlayer = "";
            for (int entry = 0; entry < chatLog.Count; entry++)
            {
                if (true)
                {
                    
                    string[] words = chatLog[entry].message.Split(' ');
                    int current = 0;
                    if (lastCast == true)
                    {
                        HandleDamageEvent(lastPlayer, chatLog[entry]);
                    }
                    else if (words[current] == "you" && ( words[current + 1] == "use" || words[current + 1] == "cast"))
                    {
                        lastCast = true;
                        lastPlayer = "you";
                    } 
                    else if (words[current + 3] == "use" || words[current + 3] == "cast")
                    {
                        lastCast = true;
                        lastPlayer = words[current] + " " + words[current + 1] + " " + words[current + 2];
                    }
                    else if (words[current] == "you" && words[current + 1] == "hit")
                    {
                        lastCast = false;
                        HandleDamageEvent("you", chatLog[entry]);
                    }
                    else if (words[current + 3] == "hits")
                    {
                        lastCast = false;
                        HandleDamageEvent(words[current] + " " + words[current + 1] + " " + words[current + 2], chatLog[entry]);
                    }
                    else
                    {
                        lastCast = false;
                    
                    }
                }
            }
        }

        private void HandleDamageEvent(string player, ChatEvent cEvent)
        {
            plugin.Configuration.handledDamageEvent = true;
            string[] words = cEvent.message.Split(' ');
            foreach (var word in words)
            {
                try
                {
                    int dmg = int.Parse(word);
                    plugin.DamageMeter.HandleEvent(player, dmg, cEvent.timestamp);
                    break;
                }
                catch
                {
                    //do nothing if it fails
                }
            }
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