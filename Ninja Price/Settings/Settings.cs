using System;
using ExileCore.Shared.Attributes;
using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using Newtonsoft.Json;
using SharpDX;

namespace Ninja_Price.Settings;

public class Settings : ISettings
{
    public DateTime LastUpDateTime { get; set; } = DateTime.Now;

    [Menu("Value Loop Timer")]
    public RangeNode<int> ValueLoopTimerMS { get; set; } = new RangeNode<int>(250, 1, 2000);

    [Menu("League", 1)]
    public ListNode LeagueList { get; set; } = new ListNode();

    public ToggleNode SyncCurrentLeague { get; set; } = new ToggleNode(true);

    [Menu("Map Variant Check ?", "Toggle Map Variant Checking", 1)]
    public ToggleNode MapVariant { get; set; } = new ToggleNode(true);

    [JsonIgnore]
    public ButtonNode ReloadPrices { get; set; } = new ButtonNode();

    [Menu("Auto Reload Toggle", 3)]
    public ToggleNode AutoReload { get; set; } = new ToggleNode();

    [Menu("Reload timer (Minutes)", 31, 3)]
    public RangeNode<int> ReloadTimer { get; set; } = new RangeNode<int>(15, 1, 60);

    [Menu("Plugin Wide Text Color", 355, 3)]
    public ColorNode UniTextColor { get; set; } = Color.White;

    [Menu("Debug", "Display debug strings", 6)]
    public ToggleNode Debug { get; set; } = new ToggleNode(false);

    [Menu("Hovered Item", "This shows your prices oon items you hover over.", 567766)]
    public ToggleNode HoveredItem { get; set; } = new ToggleNode(true);

    [Menu(null, "Set to 0 to disable")]
    public RangeNode<float> MaximalValueForFractionalDisplay { get; set; } = new RangeNode<float>(0.2f, 0, 1);

    #region Visible Stash Value
    
    [JsonProperty("visibleStashValue2")]
    public StashValueSettings VisibleStashValue { get; set; } = new StashValueSettings();

    #endregion


    #region Visible Inventory Value

    [Menu("Highlight Unique Junk", "Highlight unique items under X value (useful for quick-selling to vendor).", 5)]
    public ToggleNode HighlightUniqueJunk { get; set; } = new ToggleNode(true);

    [Menu("Helmet Enchant Prices", "Display helmet enchant prices while in the laboratory.).", 6)]
    public ToggleNode HelmetEnchantPrices { get; set; } = new ToggleNode(true);

    [Menu("Artifact Chaos Prices", "Display chaos equivalent price for items with artifact costs.).", 7)]
    public ToggleNode ArtifactChaosPrices { get; set; } = new ToggleNode(true);

    [Menu("Size", "Size of the font used to draw the chaos value of the visible inventory.", 53, 5)]
    public RangeNode<int> HighlightFontSize { get; set; } = new RangeNode<int>(20, 0, 200);

    [Menu("Significant Digits", "The number of wanted decimals", 55, 5)]
    public RangeNode<int> HighlightSignificantDigits { get; set; } = new RangeNode<int>(2, 0, 2);

    [Menu("Cut off Value", "Draws a border around unique items if it's under X value in chaos", 56, 5)]
    public RangeNode<int> InventoryValueCutOff { get; set; } = new RangeNode<int>(1, 0, 10);

    [Menu("Visible Inventory Value", "Calculate value (in chaos) for the current visible Inventory tab.", 57, 5)]
    public ToggleNode VisibleInventoryValue { get; set; } = new ToggleNode(true);

    [Menu("Visible Inventory X", "Horizontal position of where the value in chaos should be drawn.", 58, 5)]
    public RangeNode<int> InventoryValueX { get; set; } = new RangeNode<int>(100, 0, 5000);

    [Menu("Visible Inventory Y", "Horizontal position of where the value in chaos should be drawn.", 59, 5)]
    public RangeNode<int> InventoryValueY { get; set; } = new RangeNode<int>(800, 0, 5000);

    #endregion

    public ToggleNode PriceHeistRewards { get; set; } = new ToggleNode(true);

    public UniqueIdentificationSettings UniqueIdentificationSettings { get; set; } = new UniqueIdentificationSettings();

    [Menu(null, index = 290)]
    public EmptyNode TradeWindowValue { get; set; }

    [Menu(null, parentIndex = 290)]
    public ToggleNode ShowTradeWindowValue { get; set; } = new ToggleNode(true);

    [Menu(null, parentIndex = 290)]
    public RangeNode<int> TradeWindowValueOffsetX { get; set; } = new RangeNode<int>(0, -2000, 2000);

    [Menu(null, parentIndex = 290)]
    public RangeNode<int> TradeWindowValueOffsetY { get; set; } = new RangeNode<int>(0, -2000, 2000);

    public ToggleNode Enable { get; set; } = new ToggleNode(true);
}

[Submenu]
public class UniqueIdentificationSettings
{
    [JsonIgnore]
    public ButtonNode RebuildUniqueItemArtMappingBackup { get; set; } = new ButtonNode();

    [Menu(null, "Use if you want to ignore what's in game memory and rely only on your custom/builtin file")]
    public ToggleNode IgnoreGameUniqueArtMapping { get; set; } = new ToggleNode(false);

    public ToggleNode PriceUniquesOnGround { get; set; } = new ToggleNode(true);

    public ToggleNode DisplayRealUniqueNameOnGround { get; set; } = new ToggleNode(true);

    public ToggleNode OnlyDisplayRealUniqueNameForValuableUniques { get; set; } = new ToggleNode(false);

    public RangeNode<float> UniqueLabelSize { get; set; } = new RangeNode<float>(0.8f, 0.1f, 1);

    public ColorNode UniqueItemNameTextColor { get; set; } = new ColorNode(Color.Black);

    public ColorNode UniqueItemNameBackgroundColor { get; set; } = new ColorNode(new Color(175, 96, 37));

    public RangeNode<int> ValuableUniqueOnGroundValueThreshold { get; set; } = new RangeNode<int>(50, 0, 100000);

    public ColorNode ValuableUniqueItemNameTextColor { get; set; } = new ColorNode(new Color(175, 96, 37));

    public ColorNode ValuableUniqueItemNameBackgroundColor { get; set; } = new ColorNode(Color.White);
}

[Submenu]
public class StashValueSettings
{
    [Menu(null, "Calculate value (in chaos) for the current visible stash tab.")]
    public ToggleNode Show { get; set; } = new ToggleNode(true);

    [Menu("X", "Horizontal position of where the value should be drawn.")]
    public RangeNode<int> PositionX { get; set; } = new RangeNode<int>(100, 0, 5000);

    [Menu("Y", "Horizontal position of where the value should be drawn.")]
    public RangeNode<int> PositionY { get; set; } = new RangeNode<int>(100, 0, 5000);

    public RangeNode<int> SignificantDigits { get; set; } = new RangeNode<int>(2, 0, 2);
    public RangeNode<int> TopValuedItemCount { get; set; } = new RangeNode<int>(3, 0, 10);
    public ToggleNode EnableBackground { get; set; } = new ToggleNode(true);
    public CurrencyTabSettings CurrencyTabSettings { get; set; } = new CurrencyTabSettings();
}

[Submenu]
public class CurrencyTabSettings
{
    public ToggleNode ShowItemOverlay { get; set; } = new ToggleNode(true);
    public ToggleNode DoNotDrawWhileAnItemIsHovered { get; set; } = new ToggleNode(true);
    public RangeNode<int> FontSize { get; set; } = new RangeNode<int>(14, 5, 50);
    public RangeNode<int> SignificantDigits { get; set; } = new RangeNode<int>(2, 0, 2);
    public RangeNode<int> BoxHeight { get; set; } = new RangeNode<int>(15, 0, 100);
    public ColorNode FontColor { get; set; } = new Color(216, 216, 216, 255);
    public ColorNode BackgroundColor { get; set; } = new Color(0, 0, 0, 255);
}