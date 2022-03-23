using Ninja_Price.Enums;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using ExileCore.PoEMemory;
using ExileCore.PoEMemory.Elements;
using ExileCore.PoEMemory.Elements.InventoryElements;
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
                if (StashPanel.IsVisible)
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
                        if (item == null || item.Item.Address == 0) continue; // Item is fucked, skip
                        if (!item.Item.IsVisible && item.ItemType != ItemTypes.None)
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
                        if (item == null || item.Item.Address == 0) continue; // Item is fucked, skip
                        if (!item.Item.IsVisible && item.ItemType != ItemTypes.None)
                            continue; // Disregard non visible items as that usually means they aren't part of what we want to look at

                        InventoryTabValue += item.PriceData.MinChaosValue;
                        InventoryItemsToDrawList.Add(item);
                    }
                }

                GetHoveredItem(); // Get information for the hovered item
                ProcessExpeditionWindow();
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
            // Hovered Item
            if (HoveredItem != null && HoveredItem.ItemType != ItemTypes.None && Settings.HoveredItem.Value)
            {
                var text = $"Change in last 7 Days: {HoveredItem.PriceData.ChangeInLast7Days}%%";
                var changeTextLength = text.Length-1;
                text += $"\n\r{String.Concat(Enumerable.Repeat('-', changeTextLength))}";

                switch (HoveredItem.ItemType)
                {
                    case ItemTypes.Currency:
                    case ItemTypes.Essence:
                    case ItemTypes.Fragment:
                    case ItemTypes.Scarab:
                    case ItemTypes.Resonator:
                    case ItemTypes.Fossil:
                    case ItemTypes.Oil:
                    case ItemTypes.Artifact:
                    case ItemTypes.Catalyst:
                    case ItemTypes.DeliriumOrbs:
                    case ItemTypes.Vials:
                    case ItemTypes.DivinationCard:
                        if (HoveredItem.PriceData.MinChaosValue / HoveredItem.PriceData.ExaltedPrice >= 0.1)
                        {
                            text += $"\n\rExalt: {HoveredItem.PriceData.MinChaosValue / HoveredItem.PriceData.ExaltedPrice:0.##}ex";
                            text += $"\n\r{String.Concat(Enumerable.Repeat('-', changeTextLength))}";
                        }
                        text += $"\n\rChaos: {HoveredItem.PriceData.MinChaosValue / HoveredItem.CurrencyInfo.StackSize}c";
                        text += $"\n\rTotal: {HoveredItem.PriceData.MinChaosValue}c";
                        break;
                    case ItemTypes.UniqueAccessory:
                    case ItemTypes.UniqueArmour:
                    case ItemTypes.UniqueFlask:
                    case ItemTypes.UniqueJewel:
                    case ItemTypes.UniqueMap:
                    case ItemTypes.UniqueWeapon:
                        if (HoveredItem.UniqueNameCandidates.Any())
                        {
                            if (HoveredItem.UniqueNameCandidates.Count == 1)
                            {
                                text += $"\nIdentified as: {HoveredItem.UniqueNameCandidates.First()}";
                            }
                            else
                            {
                                text += $"\nIdentified as one of:\n";
                                text += string.Join('\n', HoveredItem.UniqueNameCandidates);
                            }
                        }
                        if (HoveredItem.PriceData.MinChaosValue / HoveredItem.PriceData.ExaltedPrice >= 0.1)
                        {
                            text += $"\n\rExalt: {HoveredItem.PriceData.MinChaosValue / HoveredItem.PriceData.ExaltedPrice:0.##}ex - {HoveredItem.PriceData.MaxChaosValue / HoveredItem.PriceData.ExaltedPrice:0.##}ex";
                            text += $"\n\r{String.Concat(Enumerable.Repeat('-', changeTextLength))}";
                        }
                        text += $"\n\rChaos: {HoveredItem.PriceData.MinChaosValue}c - {HoveredItem.PriceData.MaxChaosValue}c";
                        break;
                    case ItemTypes.Map:
                    case ItemTypes.Incubator:
                    case ItemTypes.MavenInvitation:
                        if (HoveredItem.PriceData.MinChaosValue / HoveredItem.PriceData.ExaltedPrice >= 0.1)
                        {
                            text += $"\n\rExalt: {HoveredItem.PriceData.MinChaosValue / HoveredItem.PriceData.ExaltedPrice:0.##}ex";
                            text += $"\n\r{String.Concat(Enumerable.Repeat('-', changeTextLength))}";
                        }
                        text += $"\n\rChaos: {HoveredItem.PriceData.MinChaosValue}c";
                        break;
                }
                
                if (Settings.Debug)
                {
                    text += $"\n\rUniqueName: {HoveredItem.UniqueName}";
                    text += $"\n\rBaseName: {HoveredItem.BaseName}";
                    text += $"\n\rItemType: {HoveredItem.ItemType}";
                    text += $"\n\rMapType: {HoveredItem.MapInfo.MapType}";
                } 
                
                if (Settings.ArtifactChaosPrices)
                {
                    if (TryGetArtifactPrice(HoveredItem, out var amount, out var artifactName))
                    {
                        text += $"\nArtifact price: ({Math.Round(HoveredItem.PriceData.MinChaosValue / amount * 100, 2)}c per 100 {artifactName})";
                    }
                }

                // var textMeasure = Graphics.MeasureText(text, 15);
                //Graphics.DrawBox(new RectangleF(0, 0, textMeasure.Width, textMeasure.Height), Color.Black);
                //Graphics.DrawText(text, new Vector2(50, 50), Color.White);

                ImGui.BeginTooltip();
                ImGui.SetTooltip(text);
                ImGui.EndTooltip();
            }

            // Inventory Value
            VisibleInventoryValue();

            if (Settings.HelmetEnchantPrices)
                ShowHelmetEnchantPrices();

            if (!StashPanel.IsVisible)
                return;

            // Stash Tab Value
            VisibleStashValue();

            var tabType = StashPanel.VisibleStash.InvType;
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
            Graphics.DrawText($"Chaos: {Math.Round(chaosValue, Settings.StashValueSignificantDigits.Value):#,##0.################}" +
                              (ExaltedValue != null
                                   ? $"\nExalt: {Math.Round(chaosValue / ExaltedValue.Value, Settings.StashValueSignificantDigits.Value):#,##0.################}"
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
            var box = item.Item.GetClientRect();
            var drawBox = new RectangleF(box.X, box.Y - 2, box.Width, -Settings.CurrencyTabBoxHeight);
            var position = new Vector2(drawBox.Center.X, drawBox.Center.Y - Settings.CurrencyTabFontSize.Value / 2);
           
            Graphics.DrawText(Math.Round(item.PriceData.MinChaosValue, Settings.CurrencyTabSigDigits.Value).ToString(), position, Settings.CurrencyTabFontColor, FontAlign.Center);
            Graphics.DrawBox(drawBox, Settings.CurrencyTabBackgroundColor);
            //Graphics.DrawFrame(drawBox, 1, Settings.CurrencyTabBorderColor);
        }

        private void PriceBoxOverItemHaggle(CustomItem item)
        {
            var box = item.Item.GetClientRect();
            var drawBox = new RectangleF(box.X, box.Y + 2, box.Width, +Settings.CurrencyTabBoxHeight);
            var position = new Vector2(drawBox.Center.X, drawBox.Center.Y - Settings.CurrencyTabFontSize.Value / 2);

            if (item.PriceData.ItemBasePrices.Count == 0)
                return;

            if (Settings.Debug)
                Graphics.DrawText(string.Join(",", item.PriceData.ItemBasePrices), position, Settings.CurrencyTabFontColor, FontAlign.Center);

            Graphics.DrawText(Math.Round(item.PriceData.ItemBasePrices.Max(), Settings.CurrencyTabSigDigits.Value).ToString(CultureInfo.InvariantCulture), position, Settings.CurrencyTabFontColor, FontAlign.Center);
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
                    var tooltipRect = HoveredItem?.Item.AsObject<HoverItemIcon>()?.Tooltip?.GetClientRect() ?? new RectangleF(0, 0, 0, 0);
                    foreach (var customItem in formattedItemList)
                    {
                        var box = customItem.Item.GetClientRect();
                        if (!tooltipRect.Intersects(box))
                        {
                            if (customItem.PriceData.MinChaosValue > 0)
                            {
                                Graphics.DrawText(Math.Round(customItem.PriceData.MinChaosValue, 2).ToString(), box.TopRight, Settings.CurrencyTabFontColor, FontAlign.Right);
                            }

                            if (Settings.ArtifactChaosPrices && TryGetArtifactPrice(customItem, out var amount, out var artifactName))
                            {
                                var text = $"[{artifactName.Substring(0, 3)}]\n" +
                                           (customItem.PriceData.MinChaosValue > 0
                                                ? Math.Round(customItem.PriceData.MinChaosValue / amount * 100, 2).ToString()
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

        /// <summary>
        ///     Displays price for unique items, and highlights the uniques under X value by drawing a border arround them.
        /// </summary>
        /// <param name="item"></param>
        private void HighlightJunkUniques(CustomItem item)
        {
            var hoverUi = GameController.Game.IngameState.UIHoverTooltip.Tooltip;
            if (hoverUi != null && (item.Rarity != ItemRarity.Unique || hoverUi.GetClientRect().Intersects(item.Item.GetClientRect()) && hoverUi.IsVisibleLocal)) return;

            var chaosValueSignificantDigits = Math.Round(item.PriceData.MinChaosValue, Settings.HighlightSignificantDigits.Value);
            if (chaosValueSignificantDigits >= Settings.InventoryValueCutOff.Value) return;
            var rec = item.Item.GetClientRect();
            var fontSize = Settings.HighlightFontSize.Value;
            // var backgroundBox = Graphics.MeasureText($"{chaosValueSignificanDigits}", fontSize);
            var position = new Vector2(rec.TopRight.X - fontSize, rec.TopRight.Y);

            //Graphics.DrawBox(new RectangleF(position.X - backgroundBox.Width, position.Y, backgroundBox.Width, backgroundBox.Height), Color.Black);
            Graphics.DrawText($"{chaosValueSignificantDigits}", position, Settings.UniTextColor, FontAlign.Center);
            //Graphics.DrawFrame(item.Item.GetClientRect(), 2, Settings.HighlightColor.Value);
        }


        private void ShowHelmetEnchantPrices()
        {
            ExileCore.PoEMemory.Element GetElementByString(ExileCore.PoEMemory.Element element, string str)
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
                Graphics.DrawText(Math.Round(data.PriceData.MinChaosValue, 2) + "c", position, textColor, FontAlign.Center);
                Graphics.DrawBox(drawBox, bgColor);
                Graphics.DrawFrame(drawBox, Color.Black, 1);
            }
        }

        private bool TryGetArtifactPrice(CustomItem item, out double amount, out string artifactName)
        {
            amount = 0;
            artifactName = null;
            if (item?.Item == null)
                return false;

            Element GetElementByString(Element element, string str)
            {
                if (element == null || string.IsNullOrWhiteSpace(str))
                    return null;

                if (element.Text != null && element.Text.Contains(str))
                    return element;

                return element.Children.Select(c => GetElementByString(c, str)).FirstOrDefault(e => e != null);
            }

            var costElement = GetElementByString(item.Item?.AsObject<HoverItemIcon>()?.Tooltip, "Cost");
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
