using Dalamud.Game.Chat;
using Dalamud.Game.Gui;
using Dalamud.Plugin;
using System.Collections.Generic;

namespace SimpleLogs

public class ChatCombatLogger
{
    private DalamudPluginInterface pluginInterface;
    private List<string> chatLog = new List<string>();

    public ChatCombatLogger(DalamudPluginInterface pluginInterface)
    {
        this.pluginInterface = pluginInterface;

        // Subscribe to the ChatMessage event
        this.pluginInterface.Framework.Gui.Chat.OnChatMessage += OnChatMessage;
    }

    private void OnChatMessage(XivChatType type, uint senderId, ref SeString sender, ref SeString message, ref bool isHandled)
    {
        // Record the chat message to the chat log
        chatLog.Add($"Type: {type}, Sender: {sender.TextValue}, Message: {message.TextValue}");
    }

    public List<string> GetChatLog()
    {
        return chatLog;
    }

    public void Dispose()
    {
        // Unsubscribe from the ChatMessage event
        this.pluginInterface.Framework.Gui.Chat.OnChatMessage -= OnChatMessage;
    }
}