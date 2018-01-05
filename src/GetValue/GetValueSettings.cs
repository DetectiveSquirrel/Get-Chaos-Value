using PoeHUD.Hud.Settings;
using PoeHUD.Plugins;
using SharpDX;

namespace GetValue
{
    public class GetValueSettings : PoeHUD.Hud.Settings.SettingsBase
    {
        public GetValueSettings()
        {
            LeagueList = new ListNode();
            ReloadButton = new ButtonNode();
            AutoReload = new ToggleNode();
            AutoReloadTimer = new RangeNode<int>(15, 1, 60);

            // Visible stash tab
            VisibleStashValue = new ToggleNode(true);
            X = new RangeNode<int>(100, 0, 1920);
            Y = new RangeNode<int>(800, 0, 1080);
            ColorNode = new ColorNode(Color.AliceBlue);
            FontSize = new RangeNode<int>(20, 0, 200);
            SignificantDigits = new RangeNode<int>(5, 0, 10);
            Debug = new ToggleNode(false);
        }

        [Menu("League", 1)]
        public ListNode LeagueList { get; set; }

        [Menu("Reload", 2)]
        public ButtonNode ReloadButton { get; set; }

        [Menu("Auto Reload Toggle", 3)]
        public ToggleNode AutoReload { get; set; }

        [Menu("Auto Reload (Minutes)", 31, 3)]
        public RangeNode<int> AutoReloadTimer { get; set; }

        [Menu("Visible Stash Value", "Calculate value (in chaos) for the current visible stash tab.", 4)]
        public ToggleNode VisibleStashValue { get; set; }


        [Menu("X", "Horizontal position of where the value in chaos should be drawn.", 41, 4)]
        public RangeNode<int> X { get; set; }

        [Menu("Y", "Horizontal position of where the value in chaos should be drawn.", 42, 4)]
        public RangeNode<int> Y { get; set; }

        [Menu("Font-Size", "Size of the font used to draw the chaos value of the visible stash tab.", 43, 4)]
        public RangeNode<int> FontSize { get; set; }

        [Menu("Color", 44, 4)]
        public ColorNode ColorNode { get; set; }

        [Menu("Significant Digits", 48, 4)]
        public RangeNode<int> SignificantDigits { get; set; }

        [Menu("Debug", "Displays a border with text, on items we cannot calculate a price for.", 50, 4)]
        public ToggleNode Debug { get; set; }
    }
}