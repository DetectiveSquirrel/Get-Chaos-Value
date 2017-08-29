using PoeHUD.Hud.Settings;
using PoeHUD.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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