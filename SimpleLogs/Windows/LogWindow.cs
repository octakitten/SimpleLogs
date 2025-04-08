using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace SimpleLogs.Windows;
public class LogWindow : Window, IDisposable
{
    private Plugin plugin;

    public LogWindow(Plugin plugin) : base(
        "Simple Logs")
    {
        this.plugin = plugin;
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(300, 600),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };
    }

    public void Dispose() { }

    public override void Draw()
    {
        // Draw the log window content here
        ImGui.Text("Combat Log");
        ImGui.Text((plugin.ChatLogger.GetChatLog()[plugin.ChatLogger.GetChatLog().Count]).type);
        ImGui.Text((plugin.ChatLogger.GetChatLog()[plugin.ChatLogger.GetChatLog().Count]).sender);
        ImGui.Text((plugin.ChatLogger.GetChatLog()[plugin.ChatLogger.GetChatLog().Count]).message);
        ImGui.Text((plugin.ChatLogger.GetChatLog()[plugin.ChatLogger.GetChatLog().Count]).timestamp.ToString());
        ImGui.Separator();
        
        foreach (var entry in plugin.ChatLogger.GetChatLog())
        {
            ImGui.Text(entry.type);
            ImGui.Text(entry.sender);
            ImGui.Text(entry.message);
            ImGui.Text(entry.timestamp.ToString());
            ImGui.Separator();
        }
    }
}