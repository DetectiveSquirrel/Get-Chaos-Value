using Ninja_Price.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using ExileCore.PoEMemory.Elements;
using ExileCore.PoEMemory.Elements.InventoryElements;
using ExileCore.Shared.Enums;
using Ninja_Price.API.PoeNinja.Classes;
using Color = SharpDX.Color;

namespace Ninja_Price.Main
{
    public partial class Main
    {
        public List<NormalInventoryItem> GetInventoryItems()
        {
            var inventory = GameController.Game.IngameState.IngameUi.InventoryPanel;
            return !inventory.IsVisible ? null : inventory[InventoryIndex.PlayerInventory].VisibleInventoryItems.ToList();
        }

        public List<CustomItem> FormatItems(List<NormalInventoryItem> itemList)
        {
            return itemList.ToList().Where(x => x?.Item?.IsValid == true).Select(inventoryItem => new CustomItem(inventoryItem)).ToList();
        }

        public string GetShardParent(string shardBaseName)
        {
            var name = "";
            var orbsAndTheirRespectiveShards = new Dictionary<string, string>
            {
                {"Transmutation Shard", "Orb of Transmutation"},
                {"Alteration Shard", "Orb of Alteration"},
                {"Annulment Shard", "Orb of Annulment"},
                {"Exalted Shard", "Exalted Orb"},
                {"Mirror Shard", "Mirror of Kalandra"},
                {"Regal Shard", "Regal Orb"},
                {"Alchemy Shard", "Orb of Alchemy"},
                {"Chaos Shard", "Chaos Orb"},
                {"Ancient Shard", "Ancient Orb"},
                {"Engineer's Shard", "Engineer's Orb"},
                {"Harbinger's Shard", "Harbinger's Orb"},
                {"Horizon Shard", "Orb of Horizons"},
                {"Binding Shard", "Orb of Binding"},
                {"Scroll Fragment", "Scroll of Wisdom"},
                {"Ritual Splinter", "Ritual Vessel"},
                {"Crescent Splinter", "The Maven's Writ" }
            };
            try
            {
                name = orbsAndTheirRespectiveShards[shardBaseName];
            }
            catch
            {
                //LogMessage($"Couldn't find key with value: {shardBaseName}.", 1);
            }

            return name;
        }

        public void GetHoveredItem()
        {
            try
            {
                var uiHover = GameController.Game.IngameState.UIHover;
                if (uiHover.AsObject<HoverItemIcon>().ToolTipType != ToolTipType.ItemInChat)
                {
                    var inventoryItemIcon = uiHover.AsObject<NormalInventoryItem>();
                    var tooltip = inventoryItemIcon.Tooltip;
                    var poeEntity = inventoryItemIcon.Item;
                    if (tooltip != null && poeEntity.Address != 0 && poeEntity.IsValid)
                    {
                        var item = inventoryItemIcon.Item;
                        var baseItemType = GameController.Files.BaseItemTypes.Translate(item.Path);
                        if (baseItemType != null)
                        {
                            HoveredItem = new CustomItem(inventoryItemIcon);
                            if (HoveredItem.ItemType != ItemTypes.None)
                                GetValue(HoveredItem);
                        }
                    }
                }
            }
            catch
            {
                // ignored
                //LogError("Error in GetHoveredItem()", 10);
            }
        }

        public void GetValue(CustomItem item)
        {
            try
            {
                item.PriceData.ExaltedPrice = (double)CollectedData.Currency.Lines.Find(x => x.CurrencyTypeName == "Exalted Orb").ChaosEquivalent;
                if(item.BaseName.Contains("Rogue's Marker"))
                {
                    item.PriceData.MinChaosValue = 0;
                }
                else
                {
                    switch (item.ItemType) // easier to get data for each item type and handle logic based on that
                    {
                        // TODO: Complete
                        case ItemTypes.Currency:
                            if (item.BaseName.StartsWith("Chaos ")) // Chaos Orb or Shard
                            {
                                switch (item.CurrencyInfo.IsShard)
                                {
                                    case false:
                                        item.PriceData.MinChaosValue = item.CurrencyInfo.StackSize;
                                        break;
                                    case true:
                                        item.PriceData.MinChaosValue = item.CurrencyInfo.StackSize / 20.0;
                                        break;
                                }
                                break;
                            }
                            if (item.BaseName.Contains("Ritual Splinter")) // Ritual
                            {
                                var shardParent = GetShardParent(item.BaseName);
                                var shardCurrencySearch = CollectedData.Currency.Lines.Find(x => x.CurrencyTypeName == shardParent);
                                if (shardCurrencySearch != null)
                                {
                                    item.PriceData.MinChaosValue = item.CurrencyInfo.StackSize * (double)shardCurrencySearch.ChaosEquivalent / 100;
                                    item.PriceData.ChangeInLast7Days = (double)shardCurrencySearch.ReceiveSparkLine.TotalChange;
                                }
                                break;
                            }
                            switch (item.CurrencyInfo.IsShard)
                            {
                                case false:
                                    var normalCurrencySearch = CollectedData.Currency.Lines.Find(x => x.CurrencyTypeName == item.BaseName);
                                    if (normalCurrencySearch != null)
                                    {
                                        item.PriceData.MinChaosValue = item.CurrencyInfo.StackSize * (double)normalCurrencySearch.ChaosEquivalent;
                                        item.PriceData.ChangeInLast7Days = (double)normalCurrencySearch.ReceiveSparkLine.TotalChange;
                                    }

                                    break;
                                case true:
                                    var shardParent = GetShardParent(item.BaseName);
                                    var shardCurrencySearch = CollectedData.Currency.Lines.Find(x => x.CurrencyTypeName == shardParent);
                                    if (shardCurrencySearch != null)
                                    {
                                        item.PriceData.MinChaosValue = item.CurrencyInfo.StackSize * (double)shardCurrencySearch.ChaosEquivalent / (item.CurrencyInfo.MaxStackSize > 0 ? item.CurrencyInfo.MaxStackSize : 20);
                                        item.PriceData.ChangeInLast7Days = (double)shardCurrencySearch.ReceiveSparkLine.TotalChange;
                                    }

                                    break;
                            }
                            break;
                        case ItemTypes.Catalyst:
                            var catalystSearch = CollectedData.Currency.Lines.Find(x => x.CurrencyTypeName == item.BaseName);
                            if (catalystSearch != null)
                            {
                                item.PriceData.MinChaosValue = item.CurrencyInfo.StackSize * (double)catalystSearch.ChaosEquivalent;
                                item.PriceData.ChangeInLast7Days = (double)catalystSearch.ReceiveSparkLine.TotalChange;
                            }
                        break;
                        case ItemTypes.DivinationCard:
                            var divinationSearch = CollectedData.DivinationCards.Lines.Find(x => x.Name == item.BaseName);
                            if (divinationSearch != null)
                            {
                                item.PriceData.MinChaosValue = item.CurrencyInfo.StackSize * (double)divinationSearch.ChaosValue;
                                item.PriceData.ChangeInLast7Days = (double)divinationSearch.Sparkline.TotalChange;
                            }
                            break;
                    case ItemTypes.Essence:
                        var essenceSearch = CollectedData.Essences.Lines.Find(x => x.Name == item.BaseName);
                        if (essenceSearch != null)
                        {
                            item.PriceData.MinChaosValue = item.CurrencyInfo.StackSize * (double)essenceSearch.ChaosValue;
                            item.PriceData.ChangeInLast7Days = (double)essenceSearch.Sparkline.TotalChange;
                        }
                        break;
                    case ItemTypes.Oil:
                        var oilSearch = CollectedData.Oils.Lines.Find(x => x.Name == item.BaseName);
                        if (oilSearch != null)
                        {
                            item.PriceData.MinChaosValue = item.CurrencyInfo.StackSize * (double)oilSearch.ChaosValue;
                            item.PriceData.ChangeInLast7Days = (double)oilSearch.Sparkline.TotalChange;
                        }
                        break;
                    case ItemTypes.Artifact:
                        var artifactSearch = CollectedData.Artifacts.Lines.Find(x => x.Name == item.BaseName);
                        if (artifactSearch != null)
                        {
                            item.PriceData.MinChaosValue = item.CurrencyInfo.StackSize * (double)artifactSearch.ChaosValue;
                            item.PriceData.ChangeInLast7Days = (double)artifactSearch.Sparkline.TotalChange;
                        }
                        break;
                    case ItemTypes.Fragment:
                        var fragmentSearch = CollectedData.Fragments.Lines.Find(x => x.CurrencyTypeName == item.BaseName);
                        if (fragmentSearch != null)
                        {
                            item.PriceData.MinChaosValue = item.CurrencyInfo.StackSize * (double)fragmentSearch.ChaosEquivalent;
                            item.PriceData.ChangeInLast7Days = (double)fragmentSearch.ReceiveSparkLine.TotalChange;
                        }

                            break;
                        case ItemTypes.MavenInvitation:
                            var InvitationSearch = CollectedData.Invitations.Lines.Find(x => x.Name == item.BaseName);
                            if (InvitationSearch != null)
                            {
                                item.PriceData.MinChaosValue = (double)InvitationSearch.ChaosValue;
                                item.PriceData.ChangeInLast7Days = (double)InvitationSearch.Sparkline.TotalChange;
                            }

                            break;
                        case ItemTypes.DeliriumOrbs:
                            var deliriumOrbsSearch = CollectedData.DeliriumOrb.Lines.Find(x => x.Name == item.BaseName);
                            if (deliriumOrbsSearch != null)
                            {
                                item.PriceData.MinChaosValue = item.CurrencyInfo.StackSize * (double)deliriumOrbsSearch.ChaosValue;
                                item.PriceData.ChangeInLast7Days = (double)deliriumOrbsSearch.Sparkline.TotalChange;
                            }

                            break;
                        case ItemTypes.Vials:
                            var vialCurrencySearch = CollectedData.Vials.Lines.Find(x => x.Name == item.BaseName);
                            if (vialCurrencySearch != null)
                            {
                                item.PriceData.MinChaosValue = item.CurrencyInfo.StackSize * (double)vialCurrencySearch.ChaosValue;
                                item.PriceData.ChangeInLast7Days = (double)vialCurrencySearch.Sparkline.TotalChange;
                            }
                            break;
                        case ItemTypes.Incubator:
                            var incubatorSearch = CollectedData.Incubators.Lines.Find(x => x.Name == item.BaseName);
                            if (incubatorSearch != null)
                            {
                                item.PriceData.MinChaosValue = (double)incubatorSearch.ChaosValue;
                                item.PriceData.ChangeInLast7Days = (double)incubatorSearch.Sparkline.TotalChange;
                            }

                            break;
                        case ItemTypes.Scarab:
                            var scarabSearch = CollectedData.Scarabs.Lines.Find(x => x.Name == item.BaseName);
                            if (scarabSearch != null)
                            {
                                item.PriceData.MinChaosValue = item.CurrencyInfo.StackSize * (double)scarabSearch.ChaosValue;
                                item.PriceData.ChangeInLast7Days = (double)scarabSearch.Sparkline.TotalChange;
                            }

                            break;
                        case ItemTypes.UniqueAccessory:
                            var uniqueAccessorySearch = CollectedData.UniqueAccessories.Lines.FindAll(x =>
                                (x.Name == item.UniqueName || item.UniqueNameCandidates.Contains(x.Name)) &&
                                !x.DetailsId.Contains("-relic"));
                            if (uniqueAccessorySearch.Count == 1)
                            {
                                item.PriceData.MinChaosValue = uniqueAccessorySearch[0].ChaosValue ?? 0;
                                item.PriceData.ChangeInLast7Days = uniqueAccessorySearch[0].Sparkline.TotalChange ?? 0;
                            }
                            else if (uniqueAccessorySearch.Count > 1)
                            {
                                item.PriceData.MinChaosValue = uniqueAccessorySearch.Min(x => x.ChaosValue) ?? 0;
                                item.PriceData.MaxChaosValue = uniqueAccessorySearch.Max(x => x.ChaosValue) ?? 0;
                                item.PriceData.ChangeInLast7Days = 0;
                            }
                            else
                            {
                                item.PriceData.MinChaosValue = 0;
                                item.PriceData.ChangeInLast7Days = 0;
                            }
                            break;
                        case ItemTypes.UniqueArmour:
                        {
                            var allLinksLines = CollectedData.UniqueArmours.Lines.Where(x =>
                                (x.Name == item.UniqueName || item.UniqueNameCandidates.Contains(x.Name)) &&
                                !x.DetailsId.Contains("-relic"));
                            var uniqueArmourSearchLinks = item.LargestLink switch
                            {
                                < 5 => allLinksLines.Where(x => x.Links != 5 && x.Links != 6).ToList(),
                                5 => allLinksLines.Where(x => x.Links == 5).ToList(),
                                6 => allLinksLines.Where(x => x.Links == 6).ToList(),
                                _ => new List<UniqueArmours.Line>()
                            };

                            if (uniqueArmourSearchLinks.Count == 1)
                            {
                                item.PriceData.MinChaosValue = uniqueArmourSearchLinks[0].ChaosValue ?? 0;
                                item.PriceData.ChangeInLast7Days = uniqueArmourSearchLinks[0].Sparkline.TotalChange ?? 0;
                            }
                            else if (uniqueArmourSearchLinks.Count > 1)
                            {
                                item.PriceData.MinChaosValue = uniqueArmourSearchLinks.Min(x => x.ChaosValue) ?? 0;
                                item.PriceData.MaxChaosValue = uniqueArmourSearchLinks.Max(x => x.ChaosValue) ?? 0;
                                item.PriceData.ChangeInLast7Days = 0;
                            }
                            else
                            {
                                item.PriceData.MinChaosValue = 0;
                                item.PriceData.ChangeInLast7Days = 0;
                            }

                            break;
                        }
                        case ItemTypes.UniqueFlask:
                            var uniqueFlaskSearch = CollectedData.UniqueFlasks.Lines.FindAll(x =>
                                (x.Name == item.UniqueName || item.UniqueNameCandidates.Contains(x.Name)) &&
                                !x.DetailsId.Contains("-relic"));
                            if (uniqueFlaskSearch.Count == 1)
                            {
                                item.PriceData.MinChaosValue = uniqueFlaskSearch[0].ChaosValue ?? 0;
                                item.PriceData.ChangeInLast7Days = uniqueFlaskSearch[0].Sparkline.TotalChange ?? 0;
                            }
                            else if (uniqueFlaskSearch.Count > 1)
                            {
                                item.PriceData.MinChaosValue = uniqueFlaskSearch.Min(x => x.ChaosValue) ?? 0;
                                item.PriceData.MaxChaosValue = uniqueFlaskSearch.Max(x => x.ChaosValue) ?? 0;
                                item.PriceData.ChangeInLast7Days = 0;
                            }
                            else
                            {
                                item.PriceData.MinChaosValue = 0;
                                item.PriceData.ChangeInLast7Days = 0;
                            }

                            break;
                        case ItemTypes.UniqueJewel:
                            var uniqueJewelSearch = CollectedData.UniqueJewels.Lines.FindAll(x => 
                                (x.Name == item.UniqueName || item.UniqueNameCandidates.Contains(x.Name)) &&
                                !x.DetailsId.Contains("-relic"));
                            if (uniqueJewelSearch.Count == 1)
                            {
                                item.PriceData.MinChaosValue = uniqueJewelSearch[0].ChaosValue ?? 0;
                                item.PriceData.ChangeInLast7Days = uniqueJewelSearch[0].Sparkline.TotalChange ?? 0;
                            }
                            else if (uniqueJewelSearch.Count > 1)
                            {
                                item.PriceData.MinChaosValue = uniqueJewelSearch.Min(x => x.ChaosValue) ?? 0;
                                item.PriceData.MaxChaosValue = uniqueJewelSearch.Max(x => x.ChaosValue) ?? 0;
                                item.PriceData.ChangeInLast7Days = 0;
                            }
                            else
                            {
                                item.PriceData.MinChaosValue = 0;
                                item.PriceData.ChangeInLast7Days = 0;
                            }

                            break;
                        case ItemTypes.UniqueMap:
                            var uniqueMapSearch = CollectedData.UniqueMaps.Lines.FindAll(x =>
                                (x.BaseType == item.UniqueName || item.UniqueNameCandidates.Contains(x.BaseType)) &&
                                item.MapInfo.MapTier == x.MapTier);
                            if (uniqueMapSearch.Count == 1)
                            {
                                item.PriceData.MinChaosValue = uniqueMapSearch[0].ChaosValue ?? 0;
                                item.PriceData.ChangeInLast7Days = uniqueMapSearch[0].Sparkline.TotalChange ?? 0;
                            }
                            else if (uniqueMapSearch.Count > 1)
                            {
                                item.PriceData.MinChaosValue = uniqueMapSearch.Min(x => x.ChaosValue) ?? 0;
                                item.PriceData.MaxChaosValue = uniqueMapSearch.Max(x => x.ChaosValue) ?? 0;
                                item.PriceData.ChangeInLast7Days = 0;
                            }
                            else
                            {
                                item.PriceData.MinChaosValue = 0;
                                item.PriceData.ChangeInLast7Days = 0;
                            }

                            break;
                        case ItemTypes.Resonator:
                            var resonatorSearch = CollectedData.Resonators.Lines.Find(x => x.Name == item.BaseName);
                            if (resonatorSearch != null)
                            {
                                item.PriceData.MinChaosValue = item.CurrencyInfo.StackSize * (double)resonatorSearch.ChaosValue;
                                item.PriceData.ChangeInLast7Days = (double)resonatorSearch.Sparkline.TotalChange;
                            }

                            break;
                        case ItemTypes.Fossil:
                            var fossilSearch = CollectedData.Fossils.Lines.Find(x => x.Name == item.BaseName);
                            if (fossilSearch != null)
                            {
                                item.PriceData.MinChaosValue = item.CurrencyInfo.StackSize * (double)fossilSearch.ChaosValue;
                                item.PriceData.ChangeInLast7Days = (double)fossilSearch.Sparkline.TotalChange;
                            }

                            break;
                        case ItemTypes.UniqueWeapon:
                        {
                            var allLinksLines = CollectedData.UniqueWeapons.Lines.Where(x =>
                                (x.Name == item.UniqueName || item.UniqueNameCandidates.Contains(x.Name)) &&
                                !x.DetailsId.Contains("-relic"));
                            var uniqueArmourSearchLinks = item.LargestLink switch
                            {
                                < 5 => allLinksLines.Where(x => x.Links != 5 && x.Links != 6).ToList(),
                                5 => allLinksLines.Where(x => x.Links == 5).ToList(),
                                6 => allLinksLines.Where(x => x.Links == 6).ToList(),
                                _ => new List<UniqueWeapons.Line>()
                            };
                            if (uniqueArmourSearchLinks.Count == 1)
                            {
                                item.PriceData.MinChaosValue = uniqueArmourSearchLinks[0].ChaosValue ?? 0;
                                item.PriceData.ChangeInLast7Days = uniqueArmourSearchLinks[0].Sparkline.TotalChange ?? 0;
                            }
                            else if (uniqueArmourSearchLinks.Count > 1)
                            {
                                item.PriceData.MinChaosValue = uniqueArmourSearchLinks.Min(x => x.ChaosValue) ?? 0;
                                item.PriceData.MaxChaosValue = uniqueArmourSearchLinks.Max(x => x.ChaosValue) ?? 0;
                                item.PriceData.ChangeInLast7Days = 0;
                            }
                            else
                            {
                                item.PriceData.MinChaosValue = 0;
                                item.PriceData.ChangeInLast7Days = 0;
                            }
                            break;
                        }
                        case ItemTypes.Map:
                            switch (item.MapInfo.MapType)
                            {
                                case MapTypes.Blighted:
                                    WhiteMaps.Line normalBlightedMapSearch;

                                    if (Settings.MapVariant.Value)
                                        normalBlightedMapSearch = CollectedData.WhiteMaps.Lines.Find(x => x.BaseType == $"Blighted {item.BaseName}" && item.MapInfo.MapTier == x.MapTier && x.Variant == Settings.LeagueList.Value);
                                    else
                                        normalBlightedMapSearch = CollectedData.WhiteMaps.Lines.Find(x => x.BaseType == $"Blighted {item.BaseName}" && item.MapInfo.MapTier == x.MapTier);

                                        if (normalBlightedMapSearch != null)
                                        {
                                            item.PriceData.MinChaosValue = (double)normalBlightedMapSearch.ChaosValue;
                                            item.PriceData.ChangeInLast7Days = (double)normalBlightedMapSearch.Sparkline.TotalChange;
                                        }

                                    break;
                                case MapTypes.None:
                                    WhiteMaps.Line normalMapSearch;

                                    if (Settings.MapVariant.Value)
                                        normalMapSearch = CollectedData.WhiteMaps.Lines.Find(x => x.BaseType == item.BaseName && item.MapInfo.MapTier == x.MapTier && x.Variant == Settings.LeagueList.Value);
                                    else
                                        normalMapSearch = CollectedData.WhiteMaps.Lines.Find(x => x.BaseType == item.BaseName && item.MapInfo.MapTier == x.MapTier);

                                    if (normalMapSearch != null)
                                    {
                                        item.PriceData.MinChaosValue = (double)normalMapSearch.ChaosValue;
                                        item.PriceData.ChangeInLast7Days = (double)normalMapSearch.Sparkline.TotalChange;
                                    }

                                    break;
                            }
                            break;
                    }
                }
            }
            catch (Exception)
            {
                if (Settings.Debug) { LogMessage($"{GetCurrentMethod()}.GetValue()", 5, Color.Red); }
            }
            finally
            {
                if(item.PriceData.MaxChaosValue == 0)
                {
                    item.PriceData.MaxChaosValue = item.PriceData.MinChaosValue;
                }
            }
        }

        public void GetValueHaggle(CustomItem item)
        {
            try
            {
                item.PriceData.ExaltedPrice = (double)CollectedData.Currency.Lines.Find(x => x.CurrencyTypeName == "Exalted Orb").ChaosEquivalent;
                switch (item.ItemTypeGamble) // easier to get data for each item type and handle logic based on that
                {
                    case ItemTypes.UniqueArmour:
                        var uniqueArmourSearch = CollectedData.UniqueArmours.Lines.FindAll(x => x.BaseType == item.BaseName && !x.Name.StartsWith("Replica ") && (x.Links < 5 || x.Links == null));
                        if (uniqueArmourSearch.Count > 0)
                        {

                            foreach (var result in uniqueArmourSearch)
                            {
                                item.PriceData.ItemBasePrices.Add((double)result.ChaosValue);
                            }
                        }
                        break;
                    case ItemTypes.UniqueWeapon:
                        var uniqueWeaponSearch = CollectedData.UniqueWeapons.Lines.FindAll(x => x.BaseType == item.BaseName && !x.Name.StartsWith("Replica ") && (x.Links < 5 || x.Links == null));
                        if (uniqueWeaponSearch.Count > 0)
                        {

                            foreach (var result in uniqueWeaponSearch)
                            {
                                item.PriceData.ItemBasePrices.Add((double)result.ChaosValue);
                            }
                        }
                        break;
                    case ItemTypes.UniqueAccessory:
                        var uniqueAccessorySearch = CollectedData.UniqueAccessories.Lines.FindAll(x => x.BaseType == item.BaseName && !x.Name.StartsWith("Replica "));
                        if (uniqueAccessorySearch.Count > 0)
                        {

                            foreach (var result in uniqueAccessorySearch)
                            {
                                item.PriceData.ItemBasePrices.Add((double)result.ChaosValue);
                            }
                        }
                        break;
                    case ItemTypes.UniqueJewel:
                        var uniqueJewelSearch = CollectedData.UniqueJewels.Lines.FindAll(x => x.DetailsId.Contains(item.BaseName.ToLower().Replace(" ", "-")) && !x.Name.StartsWith("Replica "));
                        if (uniqueJewelSearch.Count > 0)
                        {

                            foreach (var result in uniqueJewelSearch)
                            {
                                item.PriceData.ItemBasePrices.Add((double)result.ChaosValue);
                            }
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                if (Settings.Debug)
                {
                    LogMessage($"{GetCurrentMethod()}.GetValueHaggle() Error that i dont understand, Item: {item.BaseName}", 5, Color.Red);
                    LogMessage($"{GetCurrentMethod()}.GetValueHaggle() {e.Message}", 5, Color.Red);
                }
            }
        }

        public bool ShouldUpdateValues()
        {
            if (StashUpdateTimer.ElapsedMilliseconds > Settings.ValueLoopTimerMS)
            {
                StashUpdateTimer.Restart();
                if (Settings.Debug) { LogMessage($"{GetCurrentMethod()} ValueUpdateTimer.Restart()", 5, Color.DarkGray); }
            }
            else
            {
                return false;
            }
            // TODO: Get inventory items and not just stash tab items, this will be done at a later date
            try
            {
                if (!Settings.VisibleStashValue.Value || !GameController.Game.IngameState.IngameUi.StashElement.IsVisible)
                {
                    if (Settings.Debug) { LogMessage($"{GetCurrentMethod()}.ShouldUpdateValues() Stash is not visible", 5, Color.DarkGray); }
                    return false;
                }

                // Dont continue if the stash page isnt even open
                if (GameController.Game.IngameState.IngameUi.StashElement.VisibleStash?.VisibleInventoryItems == null)
                {
                    if (Settings.Debug) LogMessage($"{GetCurrentMethod()}.ShouldUpdateValues() Items == null", 5, Color.DarkGray);
                    return false;
                }
            }
            catch (Exception)
            {
                if (Settings.Debug) LogMessage($"{GetCurrentMethod()}.ShouldUpdateValues()", 5, Color.DarkGray);
                return false;
            }

            if (Settings.Debug) LogMessage($"{GetCurrentMethod()}.ShouldUpdateValues() == True", 5, Color.LimeGreen);
            return true;
        }

        public bool ShouldUpdateValuesInventory()
        {
            if (InventoryUpdateTimer.ElapsedMilliseconds > Settings.ValueLoopTimerMS)
            {
                InventoryUpdateTimer.Restart();
                if (Settings.Debug) { LogMessage($"{GetCurrentMethod()} ValueUpdateTimer.Restart()", 5, Color.DarkGray); }
            }
            else
            {
                return false;
            }
            // TODO: Get inventory items and not just stash tab items, this will be done at a later date
            try
            {
                if (!Settings.VisibleInventoryValue.Value || !GameController.Game.IngameState.IngameUi.InventoryPanel.IsVisible)
                {
                    if (Settings.Debug) { LogMessage($"{GetCurrentMethod()}.ShouldUpdateValuesInventory() Inventory is not visible", 5, Color.DarkGray); }
                    return false;
                }

                // Dont continue if the stash page isnt even open
                if (GameController.Game.IngameState.IngameUi.InventoryPanel[InventoryIndex.PlayerInventory].VisibleInventoryItems == null)
                {
                    if (Settings.Debug) LogMessage($"{GetCurrentMethod()}.ShouldUpdateValuesInventory() Items == null", 5, Color.DarkGray);
                    return false;
                }
            }
            catch (Exception)
            {
                if (Settings.Debug) LogMessage($"{GetCurrentMethod()}.ShouldUpdateValuesInventory()", 5, Color.DarkGray);
                return false;
            }

            if (Settings.Debug) LogMessage($"{GetCurrentMethod()}.ShouldUpdateValuesInventory() == True", 5, Color.LimeGreen);
            return true;
        }
        
        private CustomItem GetHelmetEnchantValue(string EnchantName)
        {
            if (string.IsNullOrWhiteSpace(EnchantName))
                return null;

            var enchantSearch = CollectedData.HelmetEnchants.lines.Find(x => x.name.ToLower().Contains(EnchantName.ToLower()));
            return enchantSearch == null
                ? null
                : new CustomItem
                {
                    PriceData = new RelevantPriceData
                    {
                        MinChaosValue = enchantSearch.chaosValue, ExaltedPrice = enchantSearch.exaltedValue,
                        ItemType = ItemTypes.None, ChangeInLast7Days = enchantSearch.sparkline.totalChange
                    },
                    BaseName = enchantSearch.name
                };
        }
    }
}
