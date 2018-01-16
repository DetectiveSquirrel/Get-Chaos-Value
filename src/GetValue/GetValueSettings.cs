using PoeHUD.Hud.Settings;
using PoeHUD.Plugins;
using SharpDX;

namespace GetValue
{
    public class GetValueSettings : SettingsBase
    {
        public GetValueSettings()
        {
            LeagueList      = new ListNode();
            ReloadButton    = new ButtonNode();
            AutoReload      = new ToggleNode();
            AutoReloadTimer = new RangeNode<int>(15, 1, 60);
            UniTextColor    = Color.White;
            Debug           = new ToggleNode(false);

            // Visible stash tab
            VisibleStashValue           = new ToggleNode(true);
            StashValueX                 = new RangeNode<int>(100, 0, 1920);
            StashValueY                 = new RangeNode<int>(800, 0, 1080);
            StashValueColorNode         = new ColorNode(Color.AliceBlue);
            StashValueFontSize          = new RangeNode<int>(20, 0, 200);
            StashValueSignificantDigits = new RangeNode<int>(5,  0, 10);

            // Inventory Value
            HighlightUniqueJunk        = new ToggleNode(true);
            HighlightColor             = new ColorNode(Color.AliceBlue);
            HighlightFontSize          = new RangeNode<int>(20, 0, 200);
            HighlightSignificantDigits = new RangeNode<int>(5,  0, 10);
            InventoryValueCutOff       = new RangeNode<int>(1,  0, 10);
        }

        [Menu("League", 1)] public ListNode LeagueList { get; set; }

        [Menu("Reload", 2)] public ButtonNode ReloadButton { get; set; }

        [Menu("Auto Reload Toggle", 3)] public ToggleNode AutoReload { get; set; }

        [Menu("Auto Reload (Minutes)", 31, 3)] public RangeNode<int> AutoReloadTimer { get; set; }

        [Menu("Plugi WIde Text Color", 355, 3)]
        public ColorNode UniTextColor { get; set; }


        [Menu("Debug", "Displays a border with text, on items we cannot calculate a price for.", 6)]
        public ToggleNode Debug { get; set; }

        #region Visible Stash Value

        [Menu("Visible Stash Value", "Calculate value (in chaos) for the current visible stash tab.", 4)]
        public ToggleNode VisibleStashValue { get; set; }

        [Menu("X", "Horizontal position of where the value in chaos should be drawn.", 41, 4)]
        public RangeNode<int> StashValueX { get; set; }

        [Menu("Y", "Horizontal position of where the value in chaos should be drawn.", 42, 4)]
        public RangeNode<int> StashValueY { get; set; }

        [Menu("Size", "Size of the font used to draw the chaos value of the visible stash tab.", 43, 4)]
        public RangeNode<int> StashValueFontSize { get; set; }

        [Menu("Color", 44, 4)] public ColorNode StashValueColorNode { get; set; }

        [Menu("Significant Digits", 45, 4)] public RangeNode<int> StashValueSignificantDigits { get; set; }

        #endregion


        #region Visible Inventory Value

        [Menu("Highlight Unique Junk", "Highlight unique items under X value (useful for quick-selling to vendor).", 5)]
        public ToggleNode HighlightUniqueJunk { get; set; }

        [Menu("Size", "Size of the font used to draw the chaos value of the visible inventory.", 53, 5)]
        public RangeNode<int> HighlightFontSize { get; set; }

        [Menu("Color", 54, 5)] public ColorNode HighlightColor { get; set; }

        [Menu("Significant Digits", "The number of wanted decimals", 55, 5)]
        public RangeNode<int> HighlightSignificantDigits { get; set; }

        [Menu("Cut off Value", "Draws a border arround unique items if it's under X value in chaos", 56, 5)]
        public RangeNode<int> InventoryValueCutOff { get; set; }

        #endregion
    }
}