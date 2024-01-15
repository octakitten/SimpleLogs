#define DEBUG // Comment this line to disable debug logging
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using SimpleLogs.Windows;
using Dalamud.Game.Gui;
using SimpleLogs;

namespace SimpleLogs
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "Simple Logs";
        private const string CommandName = "/slogs";

        private DalamudPluginInterface PluginInterface { get; init; }
        private ICommandManager CommandManager { get; init; }
        public Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new("SimpleLogs");
        public IGameNetwork GameNetwork { get; init; }
        public IChatGui ChatGui { get; init; }
        #if DEBUG
        public Network.Testing Testing { get; init; }
        #endif
        private ConfigWindow ConfigWindow { get; init; }
        private MainWindow MainWindow { get; init; }
        public Utilities.Timer timer;
        public Utilities.DamageMeter DamageMeter { get; init; }

        public Chat.Log ChatLogger { get; init; }

        private Network.Network NetworkLogger { get; init; }


        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] ICommandManager commandManager,
            [RequiredVersion("1.0")] IGameNetwork gameNetwork,
            [RequiredVersion("1.0")] IChatGui chatGui)
        {
            this.PluginInterface = pluginInterface;
            this.CommandManager = commandManager;
            this.GameNetwork = gameNetwork;
            this.ChatGui = chatGui;

            this.Configuration = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(this.PluginInterface);

            ConfigWindow = new ConfigWindow(this);
            MainWindow = new MainWindow(this);
            
            WindowSystem.AddWindow(ConfigWindow);
            WindowSystem.AddWindow(MainWindow);

            this.timer = new Utilities.Timer();
            ChatLogger = new Chat.Log(this, timer);
            NetworkLogger = new Network.Network(this, timer);
            DamageMeter = new Utilities.DamageMeter(this);
            #if DEBUG
            Testing = new Network.Testing(this, timer);
            #endif

            this.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "A useful message to display in /xlhelp"
            });

            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        }

        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();
            
            ConfigWindow.Dispose();
            MainWindow.Dispose();
            
            
            this.CommandManager.RemoveHandler(CommandName);
        }

        private void OnCommand(string command, string args)
        {
            // in response to the slash command, just display our main ui
            ChatLogger.AnalyzeChatLog();
            MainWindow.IsOpen = true;
        }

        private void DrawUI()
        {
            this.WindowSystem.Draw();
        }

        public void DrawConfigUI()
        {
            ConfigWindow.IsOpen = true;
        }
    }
}
