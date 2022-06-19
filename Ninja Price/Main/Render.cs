using Ninja_Price.Enums;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using ExileCore;
using ExileCore.PoEMemory;
using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.Elements;
using ExileCore.PoEMemory.Elements.InventoryElements;
using ExileCore.Shared.Cache;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using Color = SharpDX.Color;
using RectangleF = SharpDX.RectangleF;
using ImGuiNET;
using static Ninja_Price.Enums.HaggleTypes.HaggleType;

namespace Ninja_Price.Main
{
    public partial class Main
    {
        public Stopwatch StashUpdateTimer = Stopwatch.StartNew();
        public Stopwatch InventoryUpdateTimer = Stopwatch.StartNew();
        public double StashTabValue { get; set; }
        public double InventoryTabValue { get; set; }
        public double? ExaltedValue { get; set; }
        public List<NormalInventoryItem> ItemList { get; set; } = new List<NormalInventoryItem>();
        public List<CustomItem> FormattedItemList { get; set; } = new List<CustomItem>();

        public List<NormalInventoryItem> InventoryItemList { get; set; } = new List<NormalInventoryItem>();
        public List<CustomItem> FormattedInventoryItemList { get; set; } = new List<CustomItem>();

        public List<CustomItem> ItemsToDrawList { get; set; } = new List<CustomItem>();
        public List<CustomItem> InventoryItemsToDrawList { get; set; } = new List<CustomItem>();
        public StashElement StashPanel { get; set; }
        public InventoryElement InventoryPanel { get; set; }
        public Element HagglePanel { get; set; }

        public CustomItem HoveredItem { get; set; }

        private readonly CachedValue<List<(CustomItem, GroundItemProcessingType)>> _groundItems;

        public Main()
        {
            _groundItems = new TimeCache<List<(CustomItem, GroundItemProcessingType)>>(GetItemsOnGround, 500);
        }

        private List<(CustomItem item, GroundItemProcessingType type)> GetItemsOnGround()
        {
            var labelsOnGround = GameController.IngameState.IngameUi.ItemsOnGroundLabelsVisible;
            var result = new List<(CustomItem item, GroundItemProcessingType type)>();
            foreach (var labelOnGround in labelsOnGround)
            {
                var item = labelOnGround.ItemOnGround;
                if (item.TryGetComponent<WorldItem>(out var worldItem) &&
                    worldItem.ItemEntity is { IsValid: true } groundItemEntity &&
                    groundItemEntity.TryGetComponent<Mods>(out var mods) &&
                    mods.ItemRarity == ItemRarity.Unique)
                {
                    result.Add((new CustomItem(groundItemEntity, labelOnGround.Label), GroundItemProcessingType.WorldItem));
                }
                else if (item.TryGetComponent<HeistRewardDisplay>(out var heistReward) &&
                         heistReward.RewardItem is { IsValid: true } heistItemEntity)
                {
                    result.Add((new CustomItem(heistItemEntity, labelOnGround.Label), GroundItemProcessingType.HeistReward));
                }
            }

            result.ForEach(x => GetValue(x.item));
            return result;
        }

        // TODO: Get hovered items && items from inventory - Getting hovered item  will become useful later on

        public override void Render()
        {
            #region Reset All Data

            StashTabValue = 0;
            InventoryTabValue = 0;
            HoveredItem = null;

            StashPanel = GameController.Game.IngameState.IngameUi.StashElement;
            InventoryPanel = GameController.Game.IngameState.IngameUi.InventoryPanel;
            HagglePanel = GameController.Game.IngameState.IngameUi.HaggleWindow;

            #endregion

            if (CollectedData == null)
            {
                //nothing loaded yet, don't waste time
                return;
            }

            try // Im lazy and just want to surpress all errors produced
            {
                // only update if the time between last update is more than AutoReloadTimer interval
                if (Settings.AutoReload && Settings.LastUpDateTime.AddMinutes(Settings.ReloadTimer.Value) < DateTime.Now)
                {
                    StartDataReload(Settings.LeagueList.Value, true);
                    Settings.LastUpDateTime = DateTime.Now;
                }

                if (Settings.Debug) LogMessage($"{GetCurrentMethod()}.Loop() is Alive", 5, Color.LawnGreen);

                if (Settings.Debug)
                    LogMessage($"{GetCurrentMethod()}: Selected League: {Settings.LeagueList.Value}", 5, Color.White);

                var tabType = StashPanel.VisibleStash?.InvType;

                // Everything is updated, lets check if we should draw
                if (StashPanel.IsVisible && tabType != null)
                {
                    if (ShouldUpdateValues())
                    {
                        // Format stash items
                        switch (tabType)
                        {
                            case InventoryType.BlightStash:
                                ItemList = StashPanel.VisibleStash.VisibleInventoryItems.ToList();
                                ItemList.RemoveAt(0);
                                ItemList.RemoveAt(ItemList.Count - 1);
                                break;
                            case InventoryType.MetamorphStash:
                                ItemList = StashPanel.VisibleStash.VisibleInventoryItems.ToList();
                                ItemList.RemoveAt(0);
                                break;
                            default:
                                ItemList = StashPanel.VisibleStash.VisibleInventoryItems.ToList();
                                break;
                        }
                        if (ItemList.Count == 0)
                        {
                            ItemList = (List<NormalInventoryItem>)GameController.Game.IngameState.IngameUi.RitualWindow.Items;
                        }
                        FormattedItemList = FormatItems(ItemList);

                        if (Settings.Debug)
                            LogMessage($"{GetCurrentMethod()}.Render() Looping if (ShouldUpdateValues())", 5,
                                Color.LawnGreen);

                        FormattedItemList.ForEach(GetValue);
                    }

                    // Gather all information needed before rendering as we only want to iterate through the list once

                    ItemsToDrawList = new List<CustomItem>();
                    foreach (var item in FormattedItemList)
                    {
                        if (item == null || item.Element.Address == 0) continue; // Item is fucked, skip
                        if (!item.Element.IsVisible && item.ItemType != ItemTypes.None)
                            continue; // Disregard non visible items as that usually means they aren't part of what we want to look at

                        StashTabValue += item.PriceData.MinChaosValue;
                        ItemsToDrawList.Add(item);
                    }
                }
                if (InventoryPanel.IsVisible)
                {
                    if (ShouldUpdateValuesInventory())
                    {
                        // Format Inventory Items
                        InventoryItemList = GetInventoryItems();
                        FormattedInventoryItemList = FormatItems(InventoryItemList);

                        if (Settings.Debug)
                            LogMessage($"{GetCurrentMethod()}.Render() Looping if (ShouldUpdateValuesInventory())", 5,
                                Color.LawnGreen);

                        FormattedInventoryItemList.ForEach(GetValue);
                    }

                    // Gather all information needed before rendering as we only want to iterate through the list once
                    InventoryItemsToDrawList = new List<CustomItem>();
                    foreach (var item in FormattedInventoryItemList)
                    {
                        if (item == null || item.Element.Address == 0) continue; // Item is fucked, skip
                        if (!item.Element.IsVisible && item.ItemType != ItemTypes.None)
                            continue; // Disregard non visible items as that usually means they aren't part of what we want to look at

                        InventoryTabValue += item.PriceData.MinChaosValue;
                        InventoryItemsToDrawList.Add(item);
                    }
                }

                GetHoveredItem(); // Get information for the hovered item
                DrawGraphics();
            }
            catch (Exception e)
            {
                // ignored
                if (Settings.Debug)
                {
                    LogMessage("Error in: Main Render Loop, restart PoEHUD.", 5, Color.Red);
                    LogMessage(e.ToString(), 5, Color.Orange);
                }
            }
        }

        public void DrawGraphics()
        {
            ProcessExpeditionWindow();
            ProcessItemsOnGround();

            // Hovered Item
            if (HoveredItem != null && HoveredItem.ItemType != ItemTypes.None && Settings.HoveredItem.Value)
            {
                var textSections = new List<string> { "" };
                void AddSection() => textSections.Add("");
                void AddText(string text1) => textSections[^1] += text1;

                var changeText = $"Change in last 7 Days: {HoveredItem.PriceData.ChangeInLast7Days}%%";
                var changeTextLength = changeText.Length - 1;
                var sectionBreak = $"\n{new string('-', changeTextLength)}\n";
                if (HoveredItem.PriceData.ChangeInLast7Days != 0)
                {
                    AddText(changeText);
                }

                var priceInExalts = HoveredItem.PriceData.MinChaosValue / HoveredItem.PriceData.ExaltedPrice;
                var priceInExaltsText = priceInExalts.FormatNumber(2);
                var minPriceText = HoveredItem.PriceData.MinChaosValue.FormatNumber(2);
                AddSection();
                switch (HoveredItem.ItemType)
                {
                    case ItemTypes.Currency:
                    case ItemTypes.Essence:
                    case ItemTypes.Fragment:
                    case ItemTypes.Scarab:
                    case ItemTypes.Resonator:
                    case ItemTypes.Fossil:
                    case ItemTypes.Oil:
                    case ItemTypes.Catalyst:
                    case ItemTypes.DeliriumOrbs:
                    case ItemTypes.Vials:
                    case ItemTypes.DivinationCard:
                        if (priceInExalts >= 0.1)
                        {
                            var priceInExaltsPerOne = priceInExalts / HoveredItem.CurrencyInfo.StackSize;
                            AddText(priceInExaltsPerOne >= 0.1
                                        ? $"\nExalt: {priceInExaltsText}ex ({priceInExaltsPerOne.FormatNumber(2)}ex per one)"
                                        : $"\nExalt: {priceInExaltsText}ex");
                        }
                        AddText($"\nChaos: {minPriceText}c ({(HoveredItem.PriceData.MinChaosValue / HoveredItem.CurrencyInfo.StackSize).FormatNumber(2)}c per one)");
                        break;
                    case ItemTypes.UniqueAccessory:
                    case ItemTypes.UniqueArmour:
                    case ItemTypes.UniqueFlask:
                    case ItemTypes.UniqueJewel:
                    case ItemTypes.UniqueMap:
                    case ItemTypes.UniqueWeapon:
                        if (HoveredItem.UniqueNameCandidates.Any())
                        {
                            AddText(HoveredItem.UniqueNameCandidates.Count == 1
                                        ? $"\nIdentified as: {HoveredItem.UniqueNameCandidates.First()}"
                                        : $"\nIdentified as one of:\n{string.Join('\n', HoveredItem.UniqueNameCandidates)}");
                        }

                        AddSection();
                        if (priceInExalts >= 0.1)
                        {
                            var maxExaltPriceText = (HoveredItem.PriceData.MaxChaosValue / HoveredItem.PriceData.ExaltedPrice).FormatNumber(2);
                            AddText(priceInExaltsText != maxExaltPriceText 
                                        ? $"\nExalt: {priceInExaltsText}ex - {maxExaltPriceText}ex" 
                                        : $"\nExalt: {priceInExaltsText}ex");
                        }

                        var maxPriceText = HoveredItem.PriceData.MaxChaosValue.FormatNumber(2);
                        AddText(minPriceText != maxPriceText 
                                    ? $"\nChaos: {minPriceText}c - {maxPriceText}c" 
                                    : $"\nChaos: {minPriceText}c");

                        break;
                    case ItemTypes.Map:
                    case ItemTypes.Incubator:
                    case ItemTypes.MavenInvitation:
                    case ItemTypes.SkillGem:
                        if (priceInExalts >= 0.1)
                        {
                            AddText($"\nExalt: {priceInExaltsText}ex");
                        }

                        AddText($"\nChaos: {minPriceText}c");
                        break;
                }

                if (Settings.Debug)
                {
                    AddSection();
                    AddText($"\nUniqueName: {HoveredItem.UniqueName}"
                          + $"\nBaseName: {HoveredItem.BaseName}"
                          + $"\nItemType: {HoveredItem.ItemType}"
                          + $"\nMapType: {HoveredItem.MapInfo.MapType}");
                } 
                
                if (Settings.ArtifactChaosPrices)
                {
                    if (TryGetArtifactPrice(HoveredItem, out var amount, out var artifactName))
                    {
                        AddSection();
                        AddText($"\nArtifact price: ({(HoveredItem.PriceData.MinChaosValue / amount * 100).FormatNumber(2)}c per 100 {artifactName})");
                    }
                }

                // var textMeasure = Graphics.MeasureText(text, 15);
                //Graphics.DrawBox(new RectangleF(0, 0, textMeasure.Width, textMeasure.Height), Color.Black);
                //Graphics.DrawText(text, new Vector2(50, 50), Color.White);

                var tooltipText = string.Join(sectionBreak, textSections.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()));
                if (!string.IsNullOrWhiteSpace(tooltipText))
                {
                    ImGui.BeginTooltip();
                    ImGui.SetTooltip(tooltipText);
                    ImGui.EndTooltip();
                }
            }

            // Inventory Value
            VisibleInventoryValue();

            if (Settings.HelmetEnchantPrices)
                ShowHelmetEnchantPrices();

            if (StashPanel.IsVisible)
            {
                VisibleStashValue();

                var tabType = StashPanel.VisibleStash?.InvType;
                foreach (var customItem in ItemsToDrawList)
                {
                    if (customItem.ItemType == ItemTypes.None) continue;

                    if (Settings.CurrencyTabSpecificToggle &&
                        (!Settings.DoNotDrawCurrencyTabSpecificWhileItemHovered || HoveredItem == null))
                    {
                        switch (tabType)
                        {
                            case InventoryType.CurrencyStash:
                            case InventoryType.FragmentStash:
                            case InventoryType.DelveStash:
                            case InventoryType.DeliriumStash:
                            case InventoryType.MetamorphStash:
                            case InventoryType.BlightStash:
                                PriceBoxOverItem(customItem);
                                break;
                        }
                    }

                    if (Settings.HighlightUniqueJunk)
                    {
                        if (customItem.ItemType == ItemTypes.None || customItem.ItemType == ItemTypes.Currency) continue;
                        HighlightJunkUniques(customItem);
                    }
                }

                if (Settings.HighlightUniqueJunk)
                    foreach (var customItem in InventoryItemsToDrawList)
                    {
                        if (customItem.ItemType == ItemTypes.None || customItem.ItemType == ItemTypes.Currency) continue;

                        HighlightJunkUniques(customItem);
                    }
            }

            if (!Settings.DoNotWarnAboutUniqueListLoad && GameController.Files.UniqueItemDescriptions.EntriesList.Count == 0)
            {
                ImGui.GetBackgroundDrawList()
                   .AddText(new System.Numerics.Vector2(10, GameController.IngameState.IngameUi.Root.Center.Y),
                        Color.Red.ToImgui(),
                        "Unique list is not loaded. Open the unique stash tab and load a new zone\nor disable this warning in the settings");
            }
        }

        private void VisibleStashValue()
        {
            try
            {
                //var StashType = GameController.Game.IngameState.IngameUi.StashElement.VisibleStash.InvType;
                if (!Settings.VisibleStashValue.Value || !StashPanel.IsVisible) return;
                {
                    var pos = new Vector2(Settings.StashValueX.Value, Settings.StashValueY.Value);
                    //Graphics.DrawText(
                    //    DrawImage($"{DirectoryFullName}//images//Chaos_Orb_inventory_icon.png",
                    //        new RectangleF(Settings.StashValueX.Value - Settings.StashValueFontSize.Value,
                    //            Settings.StashValueY.Value,
                    //            Settings.StashValueFontSize.Value, Settings.StashValueFontSize.Value))
                    //        ? $"{significantDigits}"
                    //        : $"{significantDigits} Chaos", Settings.StashValueFontSize.Value, pos,
                    //    Settings.UniTextColor);

                    var chaosValue = StashTabValue;
                    DrawWorthWidget(chaosValue, pos);
                }
            }
            catch (Exception e)
            {
                // ignored
                if (Settings.Debug)
                {
                    LogMessage("Error in: VisibleStashValue, restart PoEHUD.", 5, Color.Red);
                    LogMessage(e.ToString(), 5, Color.Orange);
                }
            }
        }

        private void DrawWorthWidget(double chaosValue, Vector2 pos)
        {
            Graphics.DrawText($"Chaos: {chaosValue.FormatNumber(Settings.StashValueSignificantDigits.Value)}" +
                              (ExaltedValue != null
                                   ? $"\nExalt: {(chaosValue / ExaltedValue.Value).FormatNumber(Settings.StashValueSignificantDigits.Value)}"
                                   : ""),
                pos, Settings.UniTextColor, FontAlign.Left);
        }

        private void VisibleInventoryValue()
        {
            try
            {
                var inventory = GameController.Game.IngameState.IngameUi.InventoryPanel;
                if (!Settings.VisibleInventoryValue.Value || !inventory.IsVisible) return;
                {
                    var pos = new Vector2(Settings.InventoryValueX.Value, Settings.InventoryValueY.Value);
                    DrawWorthWidget(InventoryTabValue, pos);
                }
            }
            catch (Exception e)
            {
                // ignored
                if (Settings.Debug)
                {

                    LogMessage("Error in: VisibleInventoryValue, restart PoEHUD.", 5, Color.Red);
                    LogMessage(e.ToString(), 5, Color.Orange);
                }
            }
        }

        private void PriceBoxOverItem(CustomItem item)
        {
            var box = item.Element.GetClientRect();
            var drawBox = new RectangleF(box.X, box.Y - 2, box.Width, -Settings.CurrencyTabBoxHeight);
            var position = new Vector2(drawBox.Center.X, drawBox.Center.Y - Settings.CurrencyTabFontSize.Value / 2);
           
            Graphics.DrawText(item.PriceData.MinChaosValue.FormatNumber(Settings.CurrencyTabSigDigits.Value), position, Settings.CurrencyTabFontColor, FontAlign.Center);
            Graphics.DrawBox(drawBox, Settings.CurrencyTabBackgroundColor);
            //Graphics.DrawFrame(drawBox, 1, Settings.CurrencyTabBorderColor);
        }

        private void PriceBoxOverItemHaggle(CustomItem item)
        {
            var box = item.Element.GetClientRect();
            var drawBox = new RectangleF(box.X, box.Y + 2, box.Width, +Settings.CurrencyTabBoxHeight);
            var position = new Vector2(drawBox.Center.X, drawBox.Center.Y - Settings.CurrencyTabFontSize.Value / 2);

            if (item.PriceData.ItemBasePrices.Count == 0)
                return;

            if (Settings.Debug)
                Graphics.DrawText(string.Join(",", item.PriceData.ItemBasePrices), position, Settings.CurrencyTabFontColor, FontAlign.Center);

            Graphics.DrawText(item.PriceData.ItemBasePrices.Max().FormatNumber(Settings.CurrencyTabSigDigits.Value), position, Settings.CurrencyTabFontColor, FontAlign.Center);
            Graphics.DrawBox(drawBox, Settings.CurrencyTabBackgroundColor);
            //Graphics.DrawFrame(drawBox, 1, Settings.CurrencyTabBorderColor);
        }

        private void ProcessExpeditionWindow()
        {
            if (!HagglePanel.IsVisible) return;
            {
                // Return Haggle Window Type
                var haggleText = HagglePanel.GetChildFromIndices(6, 2, 0)?.Text;

                var haggleType = haggleText switch
                {
                    "Exchange" => Exchange,
                    "Gamble" => Gamble,
                    "Deal" => Deal,
                    "Haggle" => Haggle,
                    _ => None
                };

                var inventory = HagglePanel.GetChildFromIndices(8, 1, 0, 0);
                var itemList = inventory?.GetChildrenAs<NormalInventoryItem>().Skip(1).ToList() ?? new List<NormalInventoryItem>();
                if (haggleType == Gamble)
                {
                    if (Settings.Debug)
                    {
                        foreach (var (item, index) in itemList.Select((item, index) => (item, index)))
                        {
                            LogMessage(
                                $"Haggle Item[{index}]: {GameController.Files.BaseItemTypes.Translate(item.Item.Path).BaseName}");
                        }
                    }

                    var formattedItemList = FormatItems(itemList);

                    foreach (var customItem in formattedItemList)
                    {
                        GetValueHaggle(customItem);
                        try
                        {
                            PriceBoxOverItemHaggle(customItem);
                        }
                        catch (Exception e)
                        {
                            // ignored
                            if (Settings.Debug)
                            {
                                LogMessage("Error in: ExpeditionGamble, restart PoEHUD.", 5, Color.Red);
                                LogMessage(e.ToString(), 5, Color.Orange);
                            }
                        }
                    }
                }

                if (haggleType == Haggle)
                {
                    var formattedItemList = FormatItems(itemList);
                    formattedItemList.ForEach(GetValue);
                    var tooltipRect = HoveredItem?.Element.AsObject<HoverItemIcon>()?.Tooltip?.GetClientRect() ?? new RectangleF(0, 0, 0, 0);
                    foreach (var customItem in formattedItemList)
                    {
                        var box = customItem.Element.GetClientRect();
                        if (!tooltipRect.Intersects(box))
                        {
                            if (customItem.PriceData.MinChaosValue > 0)
                            {
                                Graphics.DrawText(customItem.PriceData.MinChaosValue.FormatNumber(2), box.TopRight, Settings.CurrencyTabFontColor, FontAlign.Right);
                            }

                            if (Settings.ArtifactChaosPrices && TryGetArtifactPrice(customItem, out var amount, out var artifactName))
                            {
                                var text = $"[{artifactName.Substring(0, 3)}]\n" +
                                           (customItem.PriceData.MinChaosValue > 0
                                                ? (customItem.PriceData.MinChaosValue / amount * 100).FormatNumber(2)
                                                : "");
                                var textSize = Graphics.MeasureText(text);
                                var leftTop = box.BottomLeft - new Vector2(0, textSize.Y);
                                Graphics.DrawBox(leftTop, leftTop + textSize.TranslateToNum(), Color.Black);
                                Graphics.DrawText(text, leftTop, Settings.CurrencyTabFontColor);
                            }
                        }
                    }
                }
            }
        }

        private void ProcessItemsOnGround()
        {
            if (!Settings.PriceUniquesOnGround && !Settings.DisplayRealUniqueNameOnGround && !Settings.PriceHeistRewards) return;
            //this window allows us to change the size of the text we draw to the background list
            //yeah, it's weird
            ImGui.Begin("lmao",
                ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoBackground | ImGuiWindowFlags.NoInputs | ImGuiWindowFlags.NoFocusOnAppearing | ImGuiWindowFlags.NoNav);
            var drawList = ImGui.GetBackgroundDrawList();
            var tooltipRect = HoveredItem?.Element.AsObject<HoverItemIcon>()?.Tooltip?.GetClientRect() ?? new RectangleF(0, 0, 0, 0);
            var leftPanelRect = GameController.IngameState.IngameUi.OpenLeftPanel.Address != 0
                                    ? GameController.IngameState.IngameUi.OpenLeftPanel.GetClientRectCache
                                    : RectangleF.Empty;
            var rightPanelRect = GameController.IngameState.IngameUi.OpenRightPanel.Address != 0
                                     ? GameController.IngameState.IngameUi.OpenRightPanel.GetClientRectCache
                                     : RectangleF.Empty;
            foreach (var (item, processingType) in _groundItems.Value)
            {
                var box = item.Element.GetClientRect();
                switch (processingType)
                {
                    case GroundItemProcessingType.WorldItem:
                    {
                        if (!tooltipRect.Intersects(box) && !leftPanelRect.Intersects(box) && !rightPanelRect.Intersects(box))
                        {
                            if (Settings.PriceUniquesOnGround)
                            {
                                if (item.PriceData.MinChaosValue > 0)
                                {
                                    var s = item.PriceData.MinChaosValue.FormatNumber(2);
                                    if (item.PriceData.MaxChaosValue > item.PriceData.MinChaosValue)
                                    {
                                        s += $"-{item.PriceData.MaxChaosValue.FormatNumber(2)}";
                                    }

                                    Graphics.DrawText(s, box.TopRight, Settings.CurrencyTabFontColor, FontAlign.Right);
                                }
                            }

                            if (Settings.DisplayRealUniqueNameOnGround && !item.IsIdentified && item.UniqueNameCandidates.Any())
                            {
                                float GetRatio(string text)
                                {
                                    var textSize = Graphics.MeasureText(text);
                                    return Math.Min(box.Width * Settings.UniqueLabelSize / textSize.X, (box.Height - 2) / textSize.Y);
                                }

                                var isValuable = item.PriceData.MaxChaosValue >= Settings.ValuableUniqueOnGroundValueThreshold;
                                if (Settings.OnlyDisplayRealUniqueNameForValuableUniques && !isValuable)
                                {
                                    continue;
                                }

                                var textColor = isValuable ? Settings.ValuableUniqueItemNameTextColor : Settings.UniqueItemNameTextColor;
                                var backgroundColor = isValuable ? Settings.ValuableUniqueItemNameBackgroundColor : Settings.UniqueItemNameBackgroundColor;
                                var (text, ratio) = Enumerable.Range(1, item.UniqueNameCandidates.Count).Select(perOneLine =>
                                        string.Join('\n', MoreLinq.Extensions.BatchExtension.Batch(item.UniqueNameCandidates, perOneLine)
                                           .Select(onLine => string.Join(" / ", onLine))))
                                   .Select(text => (text, ratio: GetRatio(text)))
                                   .MaxBy(x => x.ratio);

                                ImGui.SetWindowFontScale(ratio);
                                var newTextSize = ImGui.CalcTextSize(text);
                                var textPosition = box.Center.ToVector2Num() - newTextSize / 2;
                                var rectPosition = new System.Numerics.Vector2(textPosition.X, box.Top + 1);
                                drawList.AddRectFilled(rectPosition, rectPosition + new System.Numerics.Vector2(newTextSize.X, box.Height - 2), backgroundColor.Value.ToImgui());
                                drawList.AddText(textPosition, textColor.Value.ToImgui(), text);
                                ImGui.SetWindowFontScale(1);
                            }
                        }
                        break;
                    }
                    case GroundItemProcessingType.HeistReward:
                    {
                        if (Settings.PriceHeistRewards && !leftPanelRect.Contains(box.TopRight) && !rightPanelRect.Contains(box.TopRight))
                        {
                            if (item.PriceData.MinChaosValue > 0)
                            {
                                var s = item.PriceData.MinChaosValue.FormatNumber(2);
                                if (item.PriceData.MaxChaosValue > item.PriceData.MinChaosValue)
                                {
                                    s += $"-{item.PriceData.MaxChaosValue.FormatNumber(2)}";
                                }

                                var backgroundSize = Graphics.MeasureText(s).Mult(-1).TranslateToNum();
                                Graphics.DrawBox(box.TopRight, box.TopRight + backgroundSize, Settings.CurrencyTabBackgroundColor);
                                Graphics.DrawText(s, box.TopRight, Settings.CurrencyTabFontColor, FontAlign.Right);
                            }
                        }

                        break;
                    }
                }
                
            }

            ImGui.End();
        }

        /// <summary>
        ///     Displays price for unique items, and highlights the uniques under X value by drawing a border arround them.
        /// </summary>
        /// <param name="item"></param>
        private void HighlightJunkUniques(CustomItem item)
        {
            var hoverUi = GameController.Game.IngameState.UIHoverTooltip.Tooltip;
            if (hoverUi != null && (item.Rarity != ItemRarity.Unique || hoverUi.GetClientRect().Intersects(item.Element.GetClientRect()) && hoverUi.IsVisibleLocal)) return;

            if (item.PriceData.MinChaosValue >= Settings.InventoryValueCutOff.Value) return;
            var rec = item.Element.GetClientRect();
            var fontSize = Settings.HighlightFontSize.Value;
            // var backgroundBox = Graphics.MeasureText($"{chaosValueSignificanDigits}", fontSize);
            var position = new Vector2(rec.TopRight.X - fontSize, rec.TopRight.Y);

            //Graphics.DrawBox(new RectangleF(position.X - backgroundBox.Width, position.Y, backgroundBox.Width, backgroundBox.Height), Color.Black);
            Graphics.DrawText(item.PriceData.MinChaosValue.FormatNumber(Settings.HighlightSignificantDigits.Value), 
                position, Settings.UniTextColor, FontAlign.Center);
            //Graphics.DrawFrame(item.Item.GetClientRect(), 2, Settings.HighlightColor.Value);
        }


        private void ShowHelmetEnchantPrices()
        {
            Element GetElementByString(Element element, string str)
            {
                if (string.IsNullOrWhiteSpace(str))
                    return null;

                if (element.Text != null && element.Text.Contains(str))
                    return element;

                return element.Children.Select(c => GetElementByString(c, str)).FirstOrDefault(e => e != null);
            }
            
            var ingameUi = GameController.Game.IngameState.IngameUi;
            if (!ingameUi.LabyrinthDivineFontPanel.IsVisible) return;
            var triggerEnchantment = GetElementByString(ingameUi.LabyrinthDivineFontPanel, "lvl ");
            var enchantmentContainer = triggerEnchantment?.Parent?.Parent;
            if(enchantmentContainer == null) return;
            var enchants = enchantmentContainer.Children.Select(c => new {Name = c.Children[1].Text, ContainerElement = c});
            foreach (var enchant in enchants)
            {
                var data = GetHelmetEnchantValue(enchant.Name);
                if (data == null) continue;
                var box = enchant.ContainerElement.GetClientRect();
                var drawBox = new RectangleF(box.X + box.Width, box.Y - 2, 65, box.Height);
                var position = new Vector2(drawBox.Center.X, drawBox.Center.Y - 7);

                var textColor = data.PriceData.ExaltedPrice >= 1 ? Color.Black : Color.White;
                var bgColor = data.PriceData.ExaltedPrice >= 1 ? Color.Goldenrod : Color.Black;
                Graphics.DrawText($"{data.PriceData.MinChaosValue.FormatNumber(2)}c", position, textColor, FontAlign.Center);
                Graphics.DrawBox(drawBox, bgColor);
                Graphics.DrawFrame(drawBox, Color.Black, 1);
            }
        }

        private bool TryGetArtifactPrice(CustomItem item, out double amount, out string artifactName)
        {
            amount = 0;
            artifactName = null;
            if (item?.Element == null)
                return false;

            Element GetElementByString(Element element, string str)
            {
                if (element == null || string.IsNullOrWhiteSpace(str))
                    return null;

                if (element.Text?.Contains(str) == true)
                    return element;

                return element.Children.Select(c => GetElementByString(c, str)).FirstOrDefault(e => e != null);
            }

            var costElement = GetElementByString(item.Element?.AsObject<HoverItemIcon>()?.Tooltip, "Cost");
            if (costElement?.Parent == null || 
                costElement.Parent.ChildCount < 2 ||
                costElement.Parent.GetChildAtIndex(1).ChildCount < 3)
                return false;
            var amountText = costElement.Parent.GetChildFromIndices(1, 0)?.Text;
            if (amountText == null)
                return false;
            artifactName = costElement.Parent.GetChildFromIndices(1, 2)?.Text;
            if (artifactName == null)
                return false;
            if (costElement.Text.Equals("Cost:")) // Tujen haggling
            {
                if (!int.TryParse(amountText.TrimEnd('x'), NumberStyles.Integer, CultureInfo.InvariantCulture, out var amountInt))
                {
                    return false;
                }

                amount = amountInt;
                return true;
            }

            if (costElement.Text.Equals("Cost Per Unit:")) // Artifact stacks (Dannig)
            {
                if (!double.TryParse(amountText, NumberStyles.Float, CultureInfo.InvariantCulture, out var costPerUnit))
                {
                    return false;
                }

                amount = item.CurrencyInfo.StackSize * costPerUnit;
                return true;
            }

            return false;
        }
    }
}
