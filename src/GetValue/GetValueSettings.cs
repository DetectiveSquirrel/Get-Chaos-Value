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

            Debug = new ToggleNode(false);

            // Visible stash tab
            VisibleStashValue = new ToggleNode(true);
            StashValueX = new RangeNode<int>(100, 0, 1920);
            StashValueY = new RangeNode<int>(800, 0, 1080);
            StashValueColorNode = new ColorNode(Color.AliceBlue);
            StashValueFontSize = new RangeNode<int>(20, 0, 200);
            StashValueSignificantDigits = new RangeNode<int>(5, 0, 10);

            // Inventory Value
            HighlightJunk = new ToggleNode(true);
            InventoryValueX = new RangeNode<int>(100, 0, 1920);
            InventoryValueY = new RangeNode<int>(800, 0, 1080);
            InventoryValueColorNode = new ColorNode(Color.AliceBlue);
            InventoryValueFontSize = new RangeNode<int>(20, 0, 200);
            InventoryValueSignificantDigits = new RangeNode<int>(5, 0, 10);
            InventoryValueCutOff = new RangeNode<int>(1, 0, 10);
        }

        [Menu("League", 1)]
        public ListNode LeagueList { get; set; }

        [Menu("Reload", 2)]
        public ButtonNode ReloadButton { get; set; }

        [Menu("Auto Reload Toggle", 3)]
        public ToggleNode AutoReload { get; set; }

        [Menu("Auto Reload (Minutes)", 31, 3)]
        public RangeNode<int> AutoReloadTimer { get; set; }

        #region Visible Stash Value

        [Menu("Visible Stash Value", "Calculate value (in chaos) for the current visible stash tab.", 4)]
        public ToggleNode VisibleStashValue { get; set; }

        [Menu("X", "Horizontal position of where the value in chaos should be drawn.", 41, 4)]
        public RangeNode<int> StashValueX { get; set; }

        [Menu("Y", "Horizontal position of where the value in chaos should be drawn.", 42, 4)]
        public RangeNode<int> StashValueY { get; set; }

        [Menu("Size", "Size of the font used to draw the chaos value of the visible stash tab.", 43, 4)]
        public RangeNode<int> StashValueFontSize { get; set; }

        [Menu("Color", 44, 4)]
        public ColorNode StashValueColorNode { get; set; }

        [Menu("Significant Digits", 45, 4)]
        public RangeNode<int> StashValueSignificantDigits { get; set; }
        #endregion


        #region Visible Inventory Value

        [Menu("Visible Inventory Value", 5)]
        public ToggleNode HighlightJunk { get; set; }

        [Menu("X", "Horizontal position of where the value in chaos should be drawn.", 51, 5)]
        public RangeNode<int> InventoryValueX { get; set; }

        [Menu("Y", "Horizontal position of where the value in chaos should be drawn.", 52, 5)]
        public RangeNode<int> InventoryValueY { get; set; }

        [Menu("Size", "Size of the font used to draw the chaos value of the visible inventory.", 53, 5)]
        public RangeNode<int> InventoryValueFontSize { get; set; }

        [Menu("Color", 54, 5)]
        public ColorNode InventoryValueColorNode { get; set; }

        [Menu("Significant Digits", 55, 5)]
        public RangeNode<int> InventoryValueSignificantDigits { get; set; }

        [Menu("Cut off Value for Uniques", "Draws a border and auto sells unique items if it's under X value in chaos", 56, 5)]
        public RangeNode<int> InventoryValueCutOff { get; set; }

        #endregion

        [Menu("Debug", "Displays a border with text, on items we cannot calculate a price for.", 6)]
        public ToggleNode Debug { get; set; }
    }
}