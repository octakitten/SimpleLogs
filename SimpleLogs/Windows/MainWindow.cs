using System;
using System.Numerics;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace SimpleLogs.Windows;

public class MainWindow : Window, IDisposable
{
    private Plugin plugin;

    public MainWindow(Plugin plugin) : base(
        "Simple Meter!", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.plugin = plugin;
    }

    public void Dispose()
    {
    }

    public override void Draw()
    {
        ImGui.Text($"The random config bool is {this.plugin.Configuration.SomePropertyToBeSavedAndWithADefault}");

        if (ImGui.Button("Show Settings"))
        {
            this.plugin.DrawConfigUI();
        }
        
        if (ImGui.Button("Update"))
        {
            plugin.ChatLogger.AnalyzeChatLog();
        }

        if (ImGui.Button("Reset"))
        {
            plugin.DamageMeter.Reset();
            plugin.ChatLogger.Reset();
        }

        ImGui.Spacing();

        ImGui.Text("Here's your damage:");
        ImGui.Spacing();
        
        ImGui.Columns(3);
        ImGui.Separator();
        
        foreach (var member in plugin.DamageMeter.GetPartyMembers())
        {
            // put the name in the first column
            ImGui.Text(member.name);
            ImGui.NextColumn();
            // put the damage in the second column
            ImGui.Text(member.damage.ToString());
            ImGui.NextColumn();
            // put the DPS in the third column
            ImGui.Text(member.dps.ToString());
            ImGui.NextColumn();
        }
        
    }
}
