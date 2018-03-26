using System;
using System.Collections.Generic;
using ImGuiNET;
using PoeHUD.Models;
using PoeHUD.Models.Enums;
using PoeHUD.Models.Interfaces;
using PoeHUD.Poe.Components;
using PoeHUD.Poe.Elements;
using SharpDX;
using SharpDX.Direct3D9;
using Map = PoeHUD.Poe.Components.Map;
using ImVector2 = System.Numerics.Vector2;
using ImVector4 = System.Numerics.Vector4;
using Vector4 = SharpDX.Vector4;

namespace GetValue
{
    public partial class GetValuePlugin
    {
        private string GetClassName(BaseItemType baseItemType) => GameController.Files.ItemClasses.contents.TryGetValue(baseItemType.ClassName, out var tmp) ? tmp.ClassName : baseItemType.ClassName;

        private void ShowChaosValue(RectangleF window, Vector2 textPos, IEntity itemEntity, string className, string path, bool identified, string uniqueItemName, string baseItemName, bool isMap, ItemRarity itemRarity, int lineCount, bool stackable)
        {
            #region Map Fragments and Offerings

            if (className == "Map Fragments")
            {
                if (itemEntity == null) return;
                if (Fragments.Lines.Find(x => x.CurrencyTypeName == baseItemName) == null) return;
                var item = Fragments.Lines.Find(x => x.CurrencyTypeName == baseItemName);
                var text = $"Chaos: {item.ChaosEquivalent} || Change last 7 days: {item.ReceiveSparkLine.TotalChange}%";
                DrawText(ref textPos, ref lineCount, text);
                if (stackable)
                {
                    var text2 = $"Total: {itemEntity.GetComponent<Stack>().Size * item.ChaosEquivalent}";
                    DrawText(ref textPos, ref lineCount, text2);
                }

                BackgroundBox(window, lineCount);
            }

            #endregion

            #region Normal Maps

            else if (itemRarity != ItemRarity.Unique && isMap)
            {
                if (itemEntity == null) return;
                // Ssaped map check
                var isShaped = false;
                foreach (var itemList in itemEntity.GetComponent<Mods>().ItemMods)
                    if (itemList.RawName.Contains("MapShaped"))
                        isShaped = true;
                if (isShaped)
                {
                    if (WhiteMaps.Lines.Find(x => x.Name == "Shaped" + baseItemName && x.Variant == "Atlas2") == null)
                    {
                        var item = WhiteMaps.Lines.Find(x => x.BaseType == "Shaped " + baseItemName && x.Variant == "Atlas2");
                        var text = $"Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";
                        DrawText(ref textPos, ref lineCount, text);
                        BackgroundBox(window, lineCount);
                    }
                }
                else
                {
                    if (WhiteMaps.Lines.Find(x => x.Name == baseItemName && x.Variant == "Atlas2") != null)
                    {
                        var item = WhiteMaps.Lines.Find(x => x.Name == baseItemName && x.Variant == "Atlas2");
                        var text = $"Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";
                        DrawText(ref textPos, ref lineCount, text);
                        BackgroundBox(window, lineCount);
                    }
                }
            }

            #endregion

            #region Unique Maps

            else if (itemRarity == ItemRarity.Unique && isMap)
            {
                if (itemEntity == null) return;
                if (UniqueMaps.Lines.Find(x => x.BaseType == baseItemName) == null) return;
                var item = UniqueMaps.Lines.Find(x => x.BaseType == baseItemName);
                var text = $"Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";
                DrawText(ref textPos, ref lineCount, text);
                BackgroundBox(window, lineCount);
            }

            #endregion

            #region Currency, but NOT Chaos Orbs, Shards, Essences, Wisdom Scrolls or Prohecies.

            else if (path.Contains("Currency") && baseItemName != "Chaos Orb" && !baseItemName.Contains("Shard") && !baseItemName.Contains("Essence") && !baseItemName.Contains("Remnant of") && !baseItemName.Contains("Wisdom") && baseItemName != "Prophecy")
            {
                if (itemEntity == null) return;
                if (Currency.Lines.Find(x => x.CurrencyTypeName == baseItemName) == null) return;
                var item = Currency.Lines.Find(x => x.CurrencyTypeName == baseItemName);
                var text = $"Chaos: {item.ChaosEquivalent} || Change last 7 days: {item.ReceiveSparkLine.TotalChange}%";
                DrawText(ref textPos, ref lineCount, text);
                if (stackable)
                {
                    var text2 = $"Total: {itemEntity.GetComponent<Stack>().Size * item.ChaosEquivalent}";
                    DrawText(ref textPos, ref lineCount, text2);
                }

                BackgroundBox(window, lineCount);
            }

            #endregion

            #region Shards

            else if (path.Contains("Currency") && baseItemName.Contains("Shard") && !baseItemName.Contains("Essence") && !baseItemName.Contains("Remnant of") && !baseItemName.Contains("Wisdom") && baseItemName != "Prophecy")
            {
                if (itemEntity == null) return;
                switch (baseItemName)
                {
                    case "Transmutation Shard":
                        ShowShardValues("Orb of Transmutation", textPos, lineCount, stackable, window, itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Alteration Shard":
                        ShowShardValues("Orb of Alteration", textPos, lineCount, stackable, window, itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Annulment Shard":
                        ShowShardValues("Orb of Annulment", textPos, lineCount, stackable, window, itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Exalted Shard":
                        ShowShardValues("Exalted Orb", textPos, lineCount, stackable, window, itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Mirror Shard":
                        ShowShardValues("Mirror of Kalandra", textPos, lineCount, stackable, window, itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Regal Shard":
                        ShowShardValues("Regal Orb", textPos, lineCount, stackable, window, itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Alchemy Shard":
                        ShowShardValues("Orb of Alchemy", textPos, lineCount, stackable, window, itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Chaos Shard":
                        ShowShardValues("Chaos Orb", textPos, lineCount, stackable, window, itemEntity.GetComponent<Stack>().Size);
                        break;
                    // Harb Orbs
                    case "Ancient Shard":
                        ShowShardValues("Ancient Orb", textPos, lineCount, stackable, window, itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Engineer's Shard":
                        ShowShardValues("Engineer's Orb", textPos, lineCount, stackable, window, itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Harbinger's Shard":
                        ShowShardValues("Harbinger's Orb", textPos, lineCount, stackable, window, itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Horizon Shard":
                        ShowShardValues("Orb of Horizons", textPos, lineCount, stackable, window, itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Binding Shard":
                        ShowShardValues("Orb of Binding", textPos, lineCount, stackable, window, itemEntity.GetComponent<Stack>().Size);
                        break;
                }
            }

            #endregion

            #region Essences

            else if (baseItemName.Contains("Essence") || baseItemName.Contains("Remnant of"))
            {
                if (itemEntity == null) return;
                if (Essences.Lines.Find(x => x.Name == baseItemName) == null) return;
                var item = Essences.Lines.Find(x => x.Name == baseItemName);
                var text = $"Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";
                DrawText(ref textPos, ref lineCount, text);
                BackgroundBox(window, lineCount);
            }

            #endregion

            #region Divination Cards

            else if (className.Contains("Divination"))
            {
                if (itemEntity == null) return;
                if (DivinationCards.Lines.Find(x => x.Name == baseItemName) == null) return;
                var item = DivinationCards.Lines.Find(x => x.Name == baseItemName);
                var text = $"Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";
                DrawText(ref textPos, ref lineCount, text);
                BackgroundBox(window, lineCount);
            }

            #endregion

            else
                switch (itemRarity)
                {
                    #region Amulets, Rings and Belts

                    case ItemRarity.Unique when (className == "Amulets" || className == "Rings" || className == "Belts") && identified:
                        if (itemEntity == null) return;
                        const string taliosSignCorrect = "Tasalio's Sign";
                        const string taliosSignIncorrect = "Tasalio’s Sign";
                        if (UniqueAccessories.Lines.Find(x => x.Name == uniqueItemName) == null) return;
                        if (UniqueAccessories.Lines.Find(x => x.Name == taliosSignCorrect) == null) return;
                        if (UniqueAccessories.Lines.Find(x => x.Name == uniqueItemName) != null)
                        {
                            var item = UniqueAccessories.Lines.Find(x => x.Name == uniqueItemName);
                            var text = $"Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";
                            DrawText(ref textPos, ref lineCount, text);
                        }
                        else if (uniqueItemName == taliosSignIncorrect)
                        {
                            var item = UniqueAccessories.Lines.Find(x => x.Name == taliosSignCorrect);
                            var text = $"Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";
                            DrawText(ref textPos, ref lineCount, text);
                        }

                        BackgroundBox(window, lineCount);
                        break;

                    #endregion

                    #region Quivers

                    case ItemRarity.Unique when (itemEntity.HasComponent<Armour>() || className == "Quivers") && identified:
                        const string victariosFlightCorrect = "Victario's Flight";
                        const string victariosFlightIncorrect = "Ondar's Flight";
                        if (uniqueItemName == victariosFlightIncorrect && UniqueArmours.Lines.Find(x => x.Name == victariosFlightCorrect) != null)
                        {
                            var item = UniqueArmours.Lines.Find(x => x.Name == victariosFlightCorrect);
                            var text = $"Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";
                            DrawText(ref textPos, ref lineCount, text);
                        }
                        else
                        {
                            if (UniqueArmours.Lines.Find(x => x.Name == uniqueItemName && x.Links == 0) != null)
                            {
                                var item = UniqueArmours.Lines.Find(x => x.Name == uniqueItemName && x.Links == 0);
                                var text = $"Links: 0 || Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";
                                DrawText(ref textPos, ref lineCount, text);
                            }

                            if (UniqueArmours.Lines.Find(x => x.Name == uniqueItemName && x.Links == 5) != null)
                            {
                                var item = UniqueArmours.Lines.Find(x => x.Name == uniqueItemName && x.Links == 5);
                                var text = $"Links: 5 || Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";
                                DrawText(ref textPos, ref lineCount, text);
                            }

                            if (UniqueArmours.Lines.Find(x => x.Name == uniqueItemName && x.Links == 6) != null)
                            {
                                var item = UniqueArmours.Lines.Find(x => x.Name == uniqueItemName && x.Links == 6);
                                var text = $"Links: 6 || Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";
                                DrawText(ref textPos, ref lineCount, text);
                            }
                        }

                        BackgroundBox(window, lineCount);
                        break;

                    #endregion

                    #region Flasks

                    case ItemRarity.Unique when itemEntity.HasComponent<Flask>() && identified:
                        if (uniqueItemName == "Vessel of Vinktar")
                        {
                            if (UniqueFlasks.Lines.Find(x => x.Name == uniqueItemName && x.Variant == "Added Attacks") != null)
                            {
                                var item = UniqueFlasks.Lines.Find(x => x.Name == uniqueItemName && x.Variant == "Added Attacks");
                                var text = $"{item.Variant} || Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";
                                DrawText(ref textPos, ref lineCount, text);
                            }

                            if (UniqueFlasks.Lines.Find(x => x.Name == uniqueItemName && x.Variant == "Penetration") != null)
                            {
                                var item = UniqueFlasks.Lines.Find(x => x.Name == uniqueItemName && x.Variant == "Penetration");
                                var text = $"{item.Variant} || Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";
                                DrawText(ref textPos, ref lineCount, text);
                            }

                            if (UniqueFlasks.Lines.Find(x => x.Name == uniqueItemName && x.Variant == "Added Spells") != null)
                            {
                                var item = UniqueFlasks.Lines.Find(x => x.Name == uniqueItemName && x.Variant == "Added Spells");
                                var text = $"{item.Variant} || Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";
                                DrawText(ref textPos, ref lineCount, text);
                            }

                            if (UniqueFlasks.Lines.Find(x => x.Name == uniqueItemName && x.Variant == "Conversion") != null)
                            {
                                var item = UniqueFlasks.Lines.Find(x => x.Name == uniqueItemName && x.Variant == "Conversion");
                                var text = $"{item.Variant} || Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";
                                DrawText(ref textPos, ref lineCount, text);
                            }
                        }
                        else
                        {
                            if (UniqueFlasks.Lines.Find(x => x.Name == uniqueItemName) == null) return;
                            var item = UniqueFlasks.Lines.Find(x => x.Name == uniqueItemName);
                            var text = $"Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";
                            DrawText(ref textPos, ref lineCount, text);
                        }

                        BackgroundBox(window, lineCount);
                        break;

                    #endregion

                    #region Jewels

                    case ItemRarity.Unique when className == "Jewel" && identified:
                        const string correctOne = "Fortified Legion";
                        const string incorrectOne = "Bulwark Legion";
                        if (UniqueJewels.Lines.Find(x => x.Name == uniqueItemName) != null)
                        {
                            var item = UniqueJewels.Lines.Find(x => x.Name == uniqueItemName);
                            var text = $"Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";
                            DrawText(ref textPos, ref lineCount, text);
                        }
                        else if (uniqueItemName == incorrectOne && UniqueJewels.Lines.Find(x => x.Name == correctOne) != null)
                        {
                            var item = UniqueJewels.Lines.Find(x => x.Name == correctOne);
                            var text = $"Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";
                            DrawText(ref textPos, ref lineCount, text);
                        }

                        BackgroundBox(window, lineCount);
                        break;

                    #endregion

                    #region Weapons

                    case ItemRarity.Unique when itemEntity.HasComponent<Weapon>() && identified:
                        if (UniqueWeapons.Lines.Find(x => x.Name == uniqueItemName && x.Links == 0) != null)
                        {
                            var item = UniqueWeapons.Lines.Find(x => x.Name == uniqueItemName && x.Links == 0);
                            var text = $"Links: 0 || Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";
                            DrawText(ref textPos, ref lineCount, text);
                        }

                        if (UniqueWeapons.Lines.Find(x => x.Name == uniqueItemName && x.Links == 5) != null)
                        {
                            var item = UniqueWeapons.Lines.Find(x => x.Name == uniqueItemName && x.Links == 5);
                            var text = $"Links: 5 || Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";
                            DrawText(ref textPos, ref lineCount, text);
                        }

                        if (UniqueWeapons.Lines.Find(x => x.Name == uniqueItemName && x.Links == 6) != null)
                        {
                            var item = UniqueWeapons.Lines.Find(x => x.Name == uniqueItemName && x.Links == 6);
                            var text = $"Links: 6 || Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";
                            DrawText(ref textPos, ref lineCount, text);
                        }

                        BackgroundBox(window, lineCount);
                        break;

                    #endregion

                    default:
                        if (baseItemName.Contains("Breachstone"))
                        {
                            if (Fragments.Lines.Find(x => x.CurrencyTypeName == baseItemName) == null) return;
                            var item = Fragments.Lines.Find(x => x.CurrencyTypeName == baseItemName);
                            var text = $"Chaos: {item.ChaosEquivalent} || Change last 7 days: {item.ReceiveSparkLine.TotalChange}%";
                            DrawText(ref textPos, ref lineCount, text);
                            BackgroundBox(window, lineCount);
                        }

                        break;
                }
        }

        private void DrawText(ref Vector2 textPos, ref int lineCount, string text)
        {
            Graphics.DrawText(text, 15, textPos, Settings.UniTextColor);
            lineCount++;
            textPos.Y += 15;
        }

        private void BackgroundBox(RectangleF window, int lineCount)
        {
            window.Height *= lineCount;
            Graphics.DrawBox(window, new Color(0, 0, 0, 240));
        }

        private void ShowShardValues(string orbParent, Vector2 textPos, int lineCount, bool stackable, RectangleF window, int stackSize)
        {
            if (orbParent != "Chaos Orb")
            {
                if (Currency.Lines.Find(x => x.CurrencyTypeName == orbParent) == null) return;
                var item = Currency.Lines.Find(x => x.CurrencyTypeName == orbParent);
                var text = $"1 Shard: {item.ChaosEquivalent / 20} Chaos";
                DrawText(ref textPos, ref lineCount, text);
                if (stackable)
                {
                    var text2 = $"Full Stack: {item.ChaosEquivalent} Chaos";
                    DrawText(ref textPos, ref lineCount, text2);
                    var text3 = $"Total: {item.ChaosEquivalent / 20 * stackSize} Chaos";
                    DrawText(ref textPos, ref lineCount, text3);
                }
            }
            else
            {
                const string text = "1 Shard: 0.05 Chaos";
                DrawText(ref textPos, ref lineCount, text);
                if (stackable)
                {
                    const string text2 = "Full Stack: 1 Chaos";
                    DrawText(ref textPos, ref lineCount, text2);
                    var text3 = $"Total: {1.00 / 20.00 * stackSize} Chaos";
                    DrawText(ref textPos, ref lineCount, text3);
                }
            }

            BackgroundBox(window, lineCount);
        }

        private void Highlighter()
        {
            if (!Settings.HighlightUniqueJunk.Value) return;
            var inventoryItems = GetInventoryItems();
            if (inventoryItems != null) HighlightJunkUniques(inventoryItems);
            var stashTabItems = GameController.Game.IngameState.ServerData.StashPanel?.VisibleStash?.VisibleInventoryItems;
            if (stashTabItems == null) return;
            HighlightJunkUniques(stashTabItems);
        }

        /// <summary>
        ///     This function is made by Github.com/Nymann
        /// </summary>
        /// <param name="normalInventoryItem"></param>
        /// <returns></returns>
        private double GetChaosValue(NormalInventoryItem normalInventoryItem)
        {
            var itemEntity = normalInventoryItem.Item;
            var itemRarity = itemEntity.GetComponent<Mods>().ItemRarity;
            var isMap = itemEntity.HasComponent<Map>();
            var baseType = GameController.Files.BaseItemTypes.Translate(itemEntity.Path);
            var className = GetClassName(baseType);
            var baseItemName = baseType.BaseName;
            var classItemName = baseType.ClassName;
            var path = itemEntity.Path;
            var stack = itemEntity.GetComponent<Stack>();
            var stackable = stack?.Info != null;
            if (baseItemName.Equals("Scroll of Wisdom") || baseItemName.Equals("Scroll Fragment")) return 0;
            if (className == "Map Fragments")
            {
                if (Fragments.Lines.Find(x => x.CurrencyTypeName == baseItemName) == null) return NotFound;
                var item = Fragments.Lines.Find(x => x.CurrencyTypeName == baseItemName);
                if (stackable)
                    return itemEntity.GetComponent<Stack>().Size * item.ChaosEquivalent;
                return item.ChaosEquivalent;
            }

            #region Normal Maps

            if (itemRarity != ItemRarity.Unique && isMap)
            {
                // Ssaped map check
                var isShaped = false;
                foreach (var itemList in itemEntity.GetComponent<Mods>().ItemMods)
                    if (itemList.RawName.Contains("MapShaped"))
                        isShaped = true;
                if (isShaped)
                {
                    if (WhiteMaps.Lines.Find(x => x.Name == "Shaped" + baseItemName && x.Variant == "Atlas2") == null)
                    {
                        var item = WhiteMaps.Lines.Find(x => x.BaseType == "Shaped " + baseItemName && x.Variant == "Atlas2");
                        return item.ChaosValue;
                    }
                }
                else
                {
                    if (WhiteMaps.Lines.Find(x => x.Name == baseItemName && x.Variant == "Atlas2") != null)
                    {
                        var item = WhiteMaps.Lines.Find(x => x.Name == baseItemName && x.Variant == "Atlas2");
                        return item.ChaosValue;
                    }
                }
            }

            #endregion

            #region Unique Maps

            else if (itemRarity == ItemRarity.Unique && isMap)
            {
                if (UniqueMaps.Lines.Find(x => x.BaseType == baseItemName) == null) return NotFound;
                var item = UniqueMaps.Lines.Find(x => x.BaseType == baseItemName);
                return item.ChaosValue;
            }

            #endregion

            #region Currency, but NOT Shards, Essences, Wisdom Scrolls or Prohecies.

            else if (path.Contains("Currency") && !baseItemName.Contains("Shard") && !baseItemName.Contains("Essence") && !baseItemName.Contains("Remnant of") && !baseItemName.Contains("Wisdom") && baseItemName != "Prophecy")
            {
                if (baseItemName.Equals("Chaos Orb")) return stack?.Size ?? 1;
                if (Currency.Lines.Find(x => x.CurrencyTypeName == baseItemName) == null) return NotFound;
                var item = Currency.Lines.Find(x => x.CurrencyTypeName == baseItemName);
                if (stackable)
                {
                    var priceForTheStack = stack.Size * item.ChaosEquivalent;
                    return priceForTheStack;
                }

                return item.ChaosEquivalent;
            }

            #endregion

            #region Shards

            else if (path.Contains("Currency") && baseItemName.Contains("Shard") && !baseItemName.Contains("Essence") && !baseItemName.Contains("Remnant of") && !baseItemName.Contains("Wisdom") && baseItemName != "Prophecy")
            {
                var stackSize = stack?.Size ?? 1;
                return GetShardValues(baseItemName, stackSize);
            }

            #endregion

            #region Essences

            else if (baseItemName.Contains("Essence") || baseItemName.Contains("Remnant of"))
            {
                if (Essences.Lines.Find(x => x.Name == baseItemName) == null) return NotFound;
                var item = Essences.Lines.Find(x => x.Name == baseItemName);
                if (!stackable) return item.ChaosValue;
                var priceForTheStack = stack.Size * item.ChaosValue;
                return priceForTheStack;
            }

            #endregion

            #region Divination Cards

            else if (classItemName.Contains("Divination"))
            {
                if (DivinationCards.Lines.Find(x => x.Name == baseItemName) == null) return NotFound;
                var item = DivinationCards.Lines.Find(x => x.Name == baseItemName);
                if (stackable)
                {
                    var priceForTheStack = stack.Size * item.ChaosValue;
                    return priceForTheStack;
                }

                return item.ChaosValue;
            }

            #endregion

            else
            {
                var mods = itemEntity.GetComponent<Mods>();
                var uniqueItemName = mods.UniqueName;
                var identified = mods.Identified;
                switch (itemRarity)
                {
                    #region Amulets, Rings and Belts

                    case ItemRarity.Unique when (className == "Amulets" || className == "Rings" || className == "Belts") && identified:
                        const string taliosSignCorrect = "Tasalio's Sign";
                        const string taliosSignIncorrect = "Tasalio’s Sign";
                        if (UniqueAccessories.Lines.Find(x => x.Name == uniqueItemName) == null) return NotFound;
                        if (UniqueAccessories.Lines.Find(x => x.Name == taliosSignCorrect) == null) return NotFound;
                        if (UniqueAccessories.Lines.Find(x => x.Name == uniqueItemName) != null)
                        {
                            var item = UniqueAccessories.Lines.Find(x => x.Name == uniqueItemName);
                            return item.ChaosValue;
                        }
                        else if (uniqueItemName == taliosSignIncorrect)
                        {
                            var item = UniqueAccessories.Lines.Find(x => x.Name == taliosSignCorrect);
                            return item.ChaosValue;
                        }

                        break;

                    #endregion

                    #region Quivers and Armour

                    case ItemRarity.Unique when (itemEntity.HasComponent<Armour>() || classItemName == "Quiver") && identified:
                        const string victariosFlightCorrect = "Victario's Flight";
                        const string victariosFlightIncorrect = "Ondar's Flight";
                        if (uniqueItemName == victariosFlightIncorrect && UniqueArmours.Lines.Find(x => x.Name == victariosFlightCorrect) != null)
                        {
                            var item = UniqueArmours.Lines.Find(x => x.Name == victariosFlightCorrect);
                            return item.ChaosValue;
                        }
                        else
                        {
                            if (UniqueArmours.Lines.Find(x => x.Name == uniqueItemName && x.Links == 0) != null)
                            {
                                var item = UniqueArmours.Lines.Find(x => x.Name == uniqueItemName && x.Links == 0);
                                return item.ChaosValue;
                            }

                            if (UniqueArmours.Lines.Find(x => x.Name == uniqueItemName && x.Links == 5) != null)
                            {
                                var item = UniqueArmours.Lines.Find(x => x.Name == uniqueItemName && x.Links == 5);
                                return item.ChaosValue;
                            }

                            if (UniqueArmours.Lines.Find(x => x.Name == uniqueItemName && x.Links == 6) != null)
                            {
                                var item = UniqueArmours.Lines.Find(x => x.Name == uniqueItemName && x.Links == 6);
                                return item.ChaosValue;
                            }
                        }

                        break;

                    #endregion

                    #region Flasks

                    case ItemRarity.Unique when itemEntity.HasComponent<Flask>() && identified:
                        if (uniqueItemName == "Vessel of Vinktar")
                        {
                            if (UniqueFlasks.Lines.Find(x => x.Name == uniqueItemName && x.Variant == "Added Attacks") != null)
                            {
                                var item = UniqueFlasks.Lines.Find(x => x.Name == uniqueItemName && x.Variant == "Added Attacks");
                                return item.ChaosValue;
                            }

                            if (UniqueFlasks.Lines.Find(x => x.Name == uniqueItemName && x.Variant == "Penetration") != null)
                            {
                                var item = UniqueFlasks.Lines.Find(x => x.Name == uniqueItemName && x.Variant == "Penetration");
                                return item.ChaosValue;
                            }

                            if (UniqueFlasks.Lines.Find(x => x.Name == uniqueItemName && x.Variant == "Added Spells") != null)
                            {
                                var item = UniqueFlasks.Lines.Find(x => x.Name == uniqueItemName && x.Variant == "Added Spells");
                                return item.ChaosValue;
                            }

                            if (UniqueFlasks.Lines.Find(x => x.Name == uniqueItemName && x.Variant == "Conversion") != null)
                            {
                                var item = UniqueFlasks.Lines.Find(x => x.Name == uniqueItemName && x.Variant == "Conversion");
                                return item.ChaosValue;
                            }
                        }
                        else
                        {
                            if (UniqueFlasks.Lines.Find(x => x.Name == uniqueItemName) == null) return NotFound;
                            var item = UniqueFlasks.Lines.Find(x => x.Name == uniqueItemName);
                            return item.ChaosValue;
                        }

                        break;

                    #endregion

                    #region Jewels

                    case ItemRarity.Unique when classItemName.Equals("Jewel") && identified:
                        const string correctOne = "Fortified Legion";
                        const string incorrectOne = "Bulwark Legion";
                        if (UniqueJewels.Lines.Find(x => x.Name.Equals(uniqueItemName)) != null)
                        {
                            var item = UniqueJewels.Lines.Find(x => x.Name.Equals(uniqueItemName));
                            return item.ChaosValue;
                        }
                        else if (uniqueItemName.Equals(incorrectOne) && UniqueJewels.Lines.Find(x => x.Name == correctOne) != null)
                        {
                            var item = UniqueJewels.Lines.Find(x => x.Name == correctOne);
                            return item.ChaosValue;
                        }

                        break;

                    #endregion

                    #region Weapons

                    case ItemRarity.Unique when itemEntity.HasComponent<Weapon>() && identified:
                        if (UniqueWeapons.Lines.Find(x => x.Name == uniqueItemName && x.Links == 0) != null)
                        {
                            var item = UniqueWeapons.Lines.Find(x => x.Name == uniqueItemName && x.Links == 0);
                            return item.ChaosValue;
                        }

                        if (UniqueWeapons.Lines.Find(x => x.Name == uniqueItemName && x.Links == 5) != null)
                        {
                            var item = UniqueWeapons.Lines.Find(x => x.Name == uniqueItemName && x.Links == 5);
                            return item.ChaosValue;
                        }

                        if (UniqueWeapons.Lines.Find(x => x.Name == uniqueItemName && x.Links == 6) != null)
                        {
                            var item = UniqueWeapons.Lines.Find(x => x.Name == uniqueItemName && x.Links == 6);
                            return item.ChaosValue;
                        }

                        break;

                    #endregion

                    default:
                        if (baseItemName.Contains("Breachstone"))
                        {
                            if (Fragments.Lines.Find(x => x.CurrencyTypeName == baseItemName) == null) return NotFound;
                            var item = Fragments.Lines.Find(x => x.CurrencyTypeName == baseItemName);
                            return item.ChaosEquivalent;
                        }

                        break;
                }
            }

            return NotFound;
        }


        private void HighlightStashUniques()
        {
            var element = GameController.Game.IngameState.ServerData.StashPanel;
            Graphics.DrawFrame(element.GetClientRect(), 4, Color.Red);
            var items = element.VisibleStash.VisibleInventoryItems;
            foreach (var normalInventoryItem in items) Graphics.DrawFrame(normalInventoryItem.GetClientRect(), 2, Color.Aqua);
        }

        /// <summary>
        ///     Displays price for all unique items, and highlights all the uniques under X value by drawing a border arround them.
        /// </summary>
        /// <param name="items"></param>
        private void HighlightJunkUniques(IEnumerable<NormalInventoryItem> items)
        {
            foreach (var normalInventoryItem in items)
            {
                if (normalInventoryItem == null || !normalInventoryItem.Item.HasComponent<Mods>()) continue;
                var isUnique = normalInventoryItem.Item.GetComponent<Mods>().ItemRarity == ItemRarity.Unique;
                if (!isUnique) continue;
                var chaosValue = GetChaosValue(normalInventoryItem);
                if ((int) chaosValue == NotFound)
                {
                    if (Settings.Debug.Value)
                    {
                        Graphics.DrawLine(normalInventoryItem.GetClientRect().TopLeft, normalInventoryItem.GetClientRect().BottomRight, 1, Color.Red);
                        Graphics.DrawLine(normalInventoryItem.GetClientRect().TopRight, normalInventoryItem.GetClientRect().BottomLeft, 1, Color.Red);
                        Graphics.DrawFrame(normalInventoryItem.GetClientRect(), 2, Color.Red);
                    }

                    continue;
                }

                var chaosValueSignificanDigits = Math.Round((decimal) chaosValue, Settings.HighlightSignificantDigits.Value);
                var rec = normalInventoryItem.GetClientRect();
                var fontSize = Settings.HighlightFontSize.Value;
                Graphics.DrawText($"{chaosValueSignificanDigits}", fontSize, new Vector2(rec.TopRight.X - fontSize, rec.TopRight.Y), Settings.UniTextColor, FontDrawFlags.Right);
                DrawImage($"{PluginDirectory}//images//Chaos_Orb_inventory_icon.png", new RectangleF(rec.TopRight.X - fontSize, rec.TopRight.Y, Settings.HighlightFontSize.Value, Settings.HighlightFontSize.Value));
                if (chaosValueSignificanDigits >= Settings.InventoryValueCutOff.Value) continue;
                Graphics.DrawFrame(normalInventoryItem.GetClientRect(), 2, Settings.HighlightColor.Value);
            }
        }

        private void VisibleStashValue()
        {
            try
            {
                var stashPanel = GameController.Game.IngameState.ServerData.StashPanel;
                if (!Settings.VisibleStashValue.Value || !stashPanel.IsVisible) return;
                var inventoryItems = stashPanel.VisibleStash.VisibleInventoryItems;
                double sum = 0;
                foreach (var normalInventoryItem in inventoryItems)
                {
                    if (normalInventoryItem == null) continue;
                    var temp = GetChaosValue(normalInventoryItem);
                    if ((int)temp == NotFound)
                    {
                        if (Settings.Debug.Value)
                        {
                            Graphics.DrawLine(normalInventoryItem.GetClientRect().TopLeft, normalInventoryItem.GetClientRect().BottomRight, 1, Color.Red);
                            Graphics.DrawLine(normalInventoryItem.GetClientRect().TopRight, normalInventoryItem.GetClientRect().BottomLeft, 1, Color.Red);
                            Graphics.DrawFrame(normalInventoryItem.GetClientRect(), 2, Color.Red);
                        }

                        continue;
                    }

                    sum += temp;
                }

                var pos = new Vector2(Settings.StashValueX.Value, Settings.StashValueY.Value);
                var significantDigits = Math.Round((decimal)sum, Settings.StashValueSignificantDigits.Value);
                Graphics.DrawText(
                        DrawImage($"{PluginDirectory}//images//Chaos_Orb_inventory_icon.png",
                                new RectangleF(Settings.StashValueX.Value - Settings.StashValueFontSize.Value, Settings.StashValueY.Value,
                                        Settings.StashValueFontSize.Value, Settings.StashValueFontSize.Value))
                                ? $"{significantDigits}"
                                : $"{significantDigits} Chaos", Settings.StashValueFontSize.Value, pos, Settings.UniTextColor);
            }
            catch
            {
                // Divination card tab ugh.
            }
        }

        private ImVector4 ToImVector4(Vector4 vector) => new ImVector4(vector.X, vector.Y, vector.Z, vector.W);

        private void PropheccyDisplay()
        {

            if (!Settings.ProphecyPrices)
                return;

            try
            {
                var stashPanel = GameController.Game.IngameState.ServerData.StashPanel;
                var stashPanelVisable = GameController.Game.IngameState.ServerData.StashPanel.IsVisible;
                if (!Settings.VisibleStashValue.Value || !stashPanel.IsVisible) return;
                var prophystringlist = new List<string>();
                var propicies = GameController.Player.GetComponent<Player>().Prophecies;
                foreach (var prophecyDat in propicies)
                {                 
                      prophystringlist.Add($"{GetProphecyValues(prophecyDat.Name)}c - " + prophecyDat.Name);
                }


                var refBool = true;
                float menuOpacity = ImGui.GetStyle().GetColor(ColorTarget.WindowBg).W;
                if (Settings.ProphecyOverrideColors)
                {
                    var tempColor = new Vector4(Settings.ProphecyBackground.Value.R / 255.0f, Settings.ProphecyBackground.Value.G / 255.0f,
                            Settings.ProphecyBackground.Value.B / 255.0f, Settings.ProphecyBackground.Value.A / 255.0f);
                    ImGui.PushStyleColor(ColorTarget.WindowBg, ToImVector4(tempColor));
                    menuOpacity = ImGui.GetStyle().GetColor(ColorTarget.WindowBg).W;
                }

                ImGui.BeginWindow("Poe.NinjaProphs", ref refBool, new ImVector2(200, 150), menuOpacity, Settings.ProphecyLocked ? WindowFlags.NoCollapse | WindowFlags.NoScrollbar | WindowFlags.NoMove | WindowFlags.NoResize | WindowFlags.NoInputs | WindowFlags.NoBringToFrontOnFocus | WindowFlags.NoTitleBar | WindowFlags.NoFocusOnAppearing : WindowFlags.Default | WindowFlags.NoTitleBar | WindowFlags.ResizeFromAnySide);

                if (Settings.ProphecyOverrideColors)
                {
                    ImGui.PopStyleColor();
                }
                                                                                                                                                                                                  

                foreach (var VARIABLE in prophystringlist)
                {
                    ImGui.Text(VARIABLE);
                }

                ImGui.EndWindow();
            }
            catch
            {
                // Divination card tab ugh.
            }
        }
        private double GetProphecyValues(string ProphName)
        {    
            var item = Prophecies.Lines.Find(x => x.Name == ProphName);
            if (item == null) return NotFound;
            var value = item.ChaosValue;
            return value;
        }


        /// <summary>
        ///     This function is made by Github.com/Nymann
        /// </summary>
        /// <param name="baseItemName"></param>
        /// <param name="stackSize"></param>
        /// <returns></returns>
        private double GetShardValues(string baseItemName, int stackSize)
        {
            var orbsAndTheirRespectiveShards = new Dictionary<string, string>
            {
                    {
                            "Transmutation Shard", "Orb of Transmutation"
                    },
                    {
                            "Alteration Shard", "Orb of Alteration"
                    },
                    {
                            "Annulment Shard", "Orb of Annulment"
                    },
                    {
                            "Exalted Shard", "Exalted Orb"
                    },
                    {
                            "Mirror Shard", "Mirror of Kalandra"
                    },
                    {
                            "Regal Shard", "Regal Orb"
                    },
                    {
                            "Alchemy Shard", "Orb of Alchemy"
                    },
                    {
                            "Chaos Shard", "Chaos Orb"
                    },
                    {
                            "Ancient Shard", "Ancient Orb"
                    },
                    {
                            "Engineer's Shard", "Engineer's Orb"
                    },
                    {
                            "Harbinger's Shard", "Harbinger's Orb"
                    },
                    {
                            "Horizon Shard", "Orb of Horizons"
                    },
                    {
                            "Binding Shard", "Orb of Binding"
                    }
            };
            var name = "";
            try
            {
                name = orbsAndTheirRespectiveShards[baseItemName];
            }
            catch
            {
                LogMessage($"Couldn't find key with value: {baseItemName}.", 1);
            }

            var item = Currency.Lines.Find(x => x.CurrencyTypeName == name);
            if (item == null) return NotFound;
            var value = item.ChaosEquivalent / 20 * stackSize;
            return value;
        }

        /// <summary>
        ///     Draws a plugin image to screen.
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="fileName">The full path including file, fx. C:\\image\\Carl.png</param>
        /// <returns></returns>
        private bool DrawImage(string fileName, RectangleF rec)
        {
            try
            {
                Graphics.DrawPluginImage(fileName, rec);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}