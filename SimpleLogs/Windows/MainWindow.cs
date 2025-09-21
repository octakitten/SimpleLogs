using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using Dalamud.Bindings.ImGui;
using Dalamud.Bindings.ImPlot;

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


    unsafe public override void Draw()
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
        
        
        //ImPlot.PlotBegin("Simple DPS Meter!");
        //ImPlot.PlotBars("Simple DPS!", members, 8);
        //ImPlot.EndPlot();
        ImPlot.BeginPlot("DPS!");
        foreach (var member in plugin.DamageMeter.GetPartyMembers())
        {
            var testname = new char[4];
            testname[0] = '1';
            testname[1] = '2';
            testname[2] = '3';
            testname[3] = '4';
            var memdps = Convert.ToSingle(member.dps);
            float* memdpsptr = &memdps;
            var memname = member.name;
            //byte* memnameptr = &(byte)memname;
            ImPlot.PlotBars(memname, memdpsptr, 1);
            //ImGui.Text(member.name);
            //ImGui.Text(member.damage.ToString());
            //ImGui.Text(member.dps.ToString());
            //ImGui.Text(member.debuffDuration.ToString());
        }
        ImPlot.EndPlot();
        ImGui.Separator();

        
        /*
        ImGui.Text($"Did we add a party member? {plugin.Configuration.addedToParty}");
        ImGui.Text($"Did we handle a damage event? {plugin.Configuration.handledDamageEvent}");
        ImGui.Text($"Did we add damage to a player? {plugin.Configuration.addedDamage}, {plugin.Configuration.addedDamageAmount}");
        ImGui.Text($"Latest fight duration? {plugin.Configuration.latestFightDuration}");
        ImGui.Text("Bottom text");
        */
    }
}
