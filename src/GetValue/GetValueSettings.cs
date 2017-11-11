using PoeHUD.Hud.Settings;
using PoeHUD.Plugins;

namespace GetValue
{
    public class GetValueSettings : SettingsBase
    {
        public GetValueSettings()
        {
            LeagueList = new ListNode();
            ReloadButton = new ButtonNode();
            AutoReload = new ToggleNode();
            AutoReloadTimer = new RangeNode<int>(15, 1, 60);
        }

        [Menu("League", 1)]
        public ListNode LeagueList { get; set; }

        [Menu("Reload", 2)]
        public ButtonNode ReloadButton { get; set; }

        [Menu("Auto Reload Toggle", 3)]
        public ToggleNode AutoReload { get; set; }

        [Menu("Auto Reload (Minutes)", 31, 3)]
        public RangeNode<int> AutoReloadTimer { get; set; }

    }
}