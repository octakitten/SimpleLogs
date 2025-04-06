using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace SimpleLogs.Windows;

public class MainWindow : Window, IDisposable
{
    private Plugin plugin;

    public MainWindow(Plugin plugin) : base(
        "Simple Meter!")
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
        ImGui.Text($"The useless button is: {this.plugin.Configuration.SomePropertyToBeSavedAndWithADefault}");

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
        ImGui.Separator();
        
        foreach (var member in plugin.DamageMeter.GetPartyMembers())
        {
            ImGui.Text(member.name);
            ImGui.Text(member.damage.ToString());
            ImGui.Text(member.dps.ToString());
            ImGui.Separator();
        }

        //ImGui.Text((plugin.ChatLogger.GetChatLog()[plugin.ChatLogger.GetChatLog().Count]).type);
        //ImGui.Text((plugin.ChatLogger.GetChatLog()[plugin.ChatLogger.GetChatLog().Count]).sender);
        //ImGui.Text((plugin.ChatLogger.GetChatLog()[plugin.ChatLogger.GetChatLog().Count]).message);
        //ImGui.Text((plugin.ChatLogger.GetChatLog()[plugin.ChatLogger.GetChatLog().Count]).timestamp.ToString());
        //ImGui.Separator();
        /*
        foreach (var entry in plugin.ChatLogger.GetChatLog())
        {
            ImGui.Text(entry.type);
            ImGui.Text(entry.sender);
            ImGui.Text(entry.message);
            ImGui.Text(entry.timestamp.ToString());
        }
        */
        /*
        ImGui.Text($"Did we add a party member? {plugin.Configuration.addedToParty}");
        ImGui.Text($"Did we handle a damage event? {plugin.Configuration.handledDamageEvent}");
        ImGui.Text($"Did we add damage to a player? {plugin.Configuration.addedDamage}, {plugin.Configuration.addedDamageAmount}");
        ImGui.Text($"Latest fight duration? {plugin.Configuration.latestFightDuration}");
        ImGui.Text("Bottom text");
        */
    }
}
