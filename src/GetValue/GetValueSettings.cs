using PoeHUD.Hud.Settings;
using PoeHUD.Plugins;

namespace GetValue
{
    public class GetValueSettings : SettingsBase
    {
        public GetValueSettings()
        {
            LeagueList = new ListNode();
        }

        [Menu("League", 1)]
        public ListNode LeagueList { get; set; }

    }
}