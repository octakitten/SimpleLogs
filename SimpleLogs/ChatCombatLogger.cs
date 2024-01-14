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
            lastEvent.type = type.ToString();
            lastEvent.sender = sender.TextValue;
            lastEvent.message = message.TextValue;
            lastEvent.timestamp = timer.GetElapsedTime().TotalSeconds;
            chatLog.Add(lastEvent);
        }

        private void AnalyzeChatLog()
        {
            int entry = 0;
            bool lastCast = false;
            string lastPlayer = "";
            while (true)
            {
                if (chatLog[entry].type == "None")
                {
                    
                    string[] words = chatLog[entry].message.Split(' ');
                    int current = 0;
                    if (words[0] == "Critical!")
                    {
                        current = current + 1;
                    }
                    
                    if (words[0] == "Direct" || words[1] == "Hit!")
                    {
                        current = current + 2;
                    }
                    
                    if (words[0] == "Critical" || words[1] == "Direct" || words[1] == "Hit!")
                    {
                        current = current + 3;
                    }

                    if (words[current] == "You" && ( words[current + 1] == "use" || words[current + 1] == "cast"))
                    {
                        lastCast = true;
                        lastPlayer = chatLog[entry].sender;
                    } 
                    else if (words[current + 3] == "use" || words[current + 3] == "cast")
                    {
                        lastCast = true;
                        lastPlayer = words[current] + " " + words[current + 1] + " " + words[current + 2];
                    }
                    else if (lastCast == true)
                    {
                        
                    }
                    else
                    {
                        lastCast = false;
                    }
                }
            }
        }

        private void HandleMessage(string[] message)
        {
            
        }

        private void ClearChatLog()
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