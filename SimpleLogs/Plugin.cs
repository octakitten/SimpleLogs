// #define DEBUG // Comment this line to disable debug logging
using Dalamud.Game.Command;
using Dalamud.Plugin;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using SimpleLogs.Windows;
using SimpleLogs.Network;
using SimpleLogs.Chat;
using SimpleLogs.Utilities;
using Dalamud.Interface;

namespace SimpleLogs
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "Simple Logs";
        private const string CommandName = "/slogs";

        private IDalamudPluginInterface PluginInterface { get; init; }
        private ICommandManager CommandManager { get; init; }
        public Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new("SimpleLogs");
        public IGameNetwork GameNetwork { get; init; }
        public IChatGui ChatGui { get; init; }
        #if DEBUG
        public Testing Testing { get; init; }
        #endif
        public ConfigWindow ConfigWindow { get; init; }
        public MainWindow MainWindow { get; init; }
        public Timer timer;
        public DamageMeter DamageMeter { get; init; }

        public Log ChatLogger { get; init; }

        private Network.Network NetworkLogger { get; init; }


        public Plugin(
            IDalamudPluginInterface pluginInterface,
            ICommandManager commandManager,
            IGameNetwork gameNetwork,
            IChatGui chatGui)
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
                HelpMessage = "Open the SLogs Config Window."
            });

            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
            this.PluginInterface.UiBuilder.OpenMainUi += DrawMainUI;
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

        public void DrawUI()
        {
            this.WindowSystem.Draw();
        }

        public void DrawMainUI()
        {
            MainWindow.IsOpen = true;
        }

        public void DrawConfigUI()
        {
            ConfigWindow.IsOpen = true;
        }
    }
}
