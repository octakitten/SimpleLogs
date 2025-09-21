using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using Dalamud.Bindings.ImGui;

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
        if (plugin.ChatLogger.GetChatLog().Count >= 1)
        {
            ImGui.Separator();
            ImGui.Text("Latest entry:");
            ImGui.Text((plugin.ChatLogger.GetChatLog()[plugin.ChatLogger.GetChatLog().Count - 1]).type);
            ImGui.Text((plugin.ChatLogger.GetChatLog()[plugin.ChatLogger.GetChatLog().Count - 1]).sender);
            ImGui.Text((plugin.ChatLogger.GetChatLog()[plugin.ChatLogger.GetChatLog().Count - 1]).message);
            ImGui.Text((plugin.ChatLogger.GetChatLog()[plugin.ChatLogger.GetChatLog().Count - 1]).timestamp.ToString());
            ImGui.Separator();
        }
        ImGui.Text("All entries:");
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
