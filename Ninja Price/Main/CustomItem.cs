using System;
using System.Collections.Generic;
using System.Linq;
using ExileCore.PoEMemory;
using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.Elements.InventoryElements;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Enums;
using Ninja_Price.Enums;

namespace Ninja_Price.Main;

public class CustomItem
{
    public static Main Core;
    public string BaseName;
    public string ClassName;
    public int Height;
    public bool IsElder;
    public bool IsIdentified;
    public bool IsCorrupted;
    public bool IsRgb;
    public bool IsShaper;
    public bool IsWeapon;
    public bool IsHovered;
    public Element Element;
    public int ItemLevel;
    public int LargestLink { get; set; } = 0;
    public string Path;
    public int Quality;
    public SkillGemQualityTypeE QualityType;
    public int GemLevel;
    public ItemRarity Rarity;
    public int Sockets;
    public string UniqueName;
    public List<string> UniqueNameCandidates;
    public int Width;
    public ItemTypes ItemType;
    public ItemTypes ItemTypeGamble;
    public MapData MapInfo { get; set; } =  new MapData();
    public CurrencyData CurrencyInfo { get; set; } =  new CurrencyData();
    public Main.RelevantPriceData PriceData { get; set; } = new Main.RelevantPriceData();

    public static void InitCustomItem(Main _core)
    {
        Core = _core;
    }

    public class MapData
    {
        public bool IsMap;
        public MapTypes MapType;
        public int MapTier;
    }
    public class CurrencyData
    {
        public bool IsShard;
        public int StackSize;
        public int MaxStackSize = 0;
    }

    public CustomItem()
    {
    }

    public CustomItem(NormalInventoryItem item) : this(item.Item, item)
    {
    }

    public CustomItem(Entity itemEntity, Element element)
    {
        try
        {
            if (element != null && element.Address != 0)
                Element = element;

            Path = itemEntity.Path;
            var baseItemType = Core.GameController.Files.BaseItemTypes.Translate(itemEntity.Path);
            ClassName = baseItemType.ClassName;
            BaseName = baseItemType.BaseName;
            Width = baseItemType.Width;
            Height = baseItemType.Height;
            var weaponClass = new List<string>
            {
                "One Hand Mace",
                "Two Hand Mace",
                "One Hand Axe",
                "Two Hand Axe",
                "One Hand Sword",
                "Two Hand Sword",
                "Thrusting One Hand Sword",
                "Bow",
                "Claw",
                "Dagger",
                "Sceptre",
                "Staff",
                "Wand"
            };
            if (itemEntity.HasComponent<Quality>())
            {
                var quality = itemEntity.GetComponent<Quality>();
                Quality = quality.ItemQuality;
            }

            if (itemEntity.TryGetComponent<SkillGem>(out var skillGem))
            {
                QualityType = skillGem.QualityType;
                GemLevel = skillGem.Level;
            }

            if (itemEntity.HasComponent<Base>())
            {
                var @base = itemEntity.GetComponent<Base>();
                IsElder = @base.isElder;
                IsShaper = @base.isShaper;
                IsCorrupted = @base.isCorrupted;
            }

            if (itemEntity.HasComponent<Mods>())
            {
                var mods = itemEntity.GetComponent<Mods>();
                Rarity = mods.ItemRarity;
                IsIdentified = mods.Identified;
                ItemLevel = mods.ItemLevel;
                UniqueName = mods.UniqueName;
                if (!IsIdentified && Rarity == ItemRarity.Unique)
                {
                    var artPath = itemEntity.GetComponent<RenderItem>()?.ResourcePath;
                    if (artPath != null)
                    {
                        UniqueNameCandidates = (Core.UniqueArtMapping.GetValueOrDefault(artPath) ?? Enumerable.Empty<string>())
                            .Where(x => !x.StartsWith("Replica "))
                            .ToList();
                    }
                }

            }

            UniqueNameCandidates ??= new List<string>();

            if (itemEntity.HasComponent<Sockets>())
            {
                try
                {
                    var sockets = itemEntity.GetComponent<Sockets>();
                    IsRgb = sockets.IsRGB;
                    Sockets = sockets.NumberOfSockets;
                    LargestLink = sockets.LargestLinkSize;
                }
                catch
                {
                }
            }

            if (weaponClass.Any(ClassName.Equals))
                IsWeapon = true;

            MapInfo.MapTier = itemEntity.HasComponent<Map>() ? itemEntity.GetComponent<Map>().Tier : 0;
            MapInfo.IsMap = MapInfo.MapTier > 0;

            if (Rarity != ItemRarity.Unique && MapInfo.IsMap)
            {
                MapInfo.MapType = MapTypes.None;

                foreach (var itemList in itemEntity.GetComponent<Mods>().ItemMods)
                {
                    if (itemList.RawName.Contains("Shaped "))
                    {
                        MapInfo.MapType = MapTypes.Shaped;
                        break;
                    }
                    else if (itemList.RawName.Contains("Elder "))
                    {
                        MapInfo.MapType = MapTypes.Elder;
                        break;
                    }
                    else if (itemList.RawName.Contains("Blighted "))
                    {
                        MapInfo.MapType = MapTypes.Blighted;
                        break;
                    }
                }
            }

            if (itemEntity.HasComponent<Stack>())
            {
                CurrencyInfo.StackSize = itemEntity.GetComponent<Stack>().Size;
                CurrencyInfo.MaxStackSize = itemEntity.GetComponent<Stack>().Info.MaxStackSize;
                if (BaseName.EndsWith(" Shard") || BaseName.EndsWith(" Fragment") ||
                    BaseName.EndsWith("Ritual Splinter"))
                    CurrencyInfo.IsShard = true;
            }


            IsHovered = Core.GameController.Game.IngameState.UIHover.AsObject<NormalInventoryItem>().Address == Element.Address;

            // sort items into types to use correct json data later from poe.ninja
            // This might need tweaking since if this catches anything other than currency.
            if (ClassName == "StackableCurrency" && !BaseName.StartsWith("Crescent Splinter") && !BaseName.StartsWith("Simulacrum") &&
                !BaseName.EndsWith("Delirium Orb") && !BaseName.Contains("Essence") && !BaseName.EndsWith(" Oil") && !BaseName.EndsWith("Artifact") &&
                !BaseName.Contains("Astragali") && !BaseName.Contains("Burial Medallion") && !BaseName.Contains("Scrap Metal") && !BaseName.Contains("Exotic Coinage") &&
                !BaseName.Contains("Remnant of") && !BaseName.Contains("Timeless ") && BaseName != "Prophecy" &&
                ClassName != "MapFragment" && !BaseName.EndsWith(" Fossil") && !BaseName.StartsWith("Splinter of ") && ClassName != "Incubator" &&
                !BaseName.EndsWith(
                    " Catalyst") /*&& !BaseName.Contains("Shard") && BaseName != "Chaos Orb" && !BaseName.Contains("Wisdom")*/
               )
            {
                ItemType = ItemTypes.Currency;
            }
            else if (BaseName.EndsWith(" Catalyst"))
            {
                ItemType = ItemTypes.Catalyst;
            }
            else if (BaseName.Contains("Astragali") || BaseName.Contains("Burial Medallion") || BaseName.Contains("Scrap Metal") || BaseName.Contains("Exotic Coinage"))     
            {
                ItemType = ItemTypes.Artifact;
            }
            else if (BaseName.EndsWith(" Oil"))
            {
                ItemType = ItemTypes.Oil;
            }
            else if (Path.Contains("Metadata/Items/DivinationCards"))
            {
                ItemType = ItemTypes.DivinationCard;
            }
            else if (BaseName.Contains("Essence") || BaseName.Contains("Remnant of"))
            {
                ItemType = ItemTypes.Essence;
            }
            else if (((ClassName == "MapFragment" || BaseName.Contains("Timeless ") || BaseName.StartsWith("Simulacrum")) && !BaseName.EndsWith(" Scarab")) ||
                     ClassName == "StackableCurrency" && BaseName.StartsWith("Splinter of ") ||
                     BaseName.StartsWith("Crescent Splinter"))
            {
                ItemType = ItemTypes.Fragment;
            }
            else if (ClassName == "MapFragment" && BaseName.EndsWith(" Scarab"))
            {
                ItemType = ItemTypes.Scarab;
            }
            else if (MapInfo.IsMap && Rarity != ItemRarity.Unique)
            {
                ItemType = ItemTypes.Map;
            }
            else if (BaseName.EndsWith(" Fossil"))
            {
                ItemType = ItemTypes.Fossil;
            }
            else if (ClassName == "DelveStackableSocketableCurrency")
            {
                ItemType = ItemTypes.Resonator;
            }
            else if (ClassName == "Incubator")
            {
                ItemType = ItemTypes.Incubator;
            }
            else if (BaseName.EndsWith("Delirium Orb"))
            {
                ItemType = ItemTypes.DeliriumOrbs;
            }
            else if (BaseName.StartsWith("Vial "))
            {
                ItemType = ItemTypes.Vials;
            }
            else if (BaseName.StartsWith("Maven's Invitation: "))
            {
                ItemType = ItemTypes.MavenInvitation;
            }
            else if (ClassName is "Support Skill Gem" or "Active Skill Gem")
            {
                ItemType = ItemTypes.SkillGem;
            }
            else
                switch (Rarity) // Unique information
                {
                    case ItemRarity.Unique or ItemRarity.Normal when ClassName == "Amulet" || ClassName == "Ring" || ClassName == "Belt":
                        ItemType = ItemTypes.UniqueAccessory;
                        break;
                    case ItemRarity.Unique or ItemRarity.Normal when itemEntity.HasComponent<Armour>() || ClassName == "Quiver":
                        ItemType = ItemTypes.UniqueArmour;
                        break;
                    case ItemRarity.Unique when itemEntity.HasComponent<Flask>():
                        ItemType = ItemTypes.UniqueFlask;
                        break;
                    case ItemRarity.Unique or ItemRarity.Normal when ClassName == "Jewel":
                        ItemType = ItemTypes.UniqueJewel;
                        break;
                    case ItemRarity.Unique when MapInfo.IsMap:
                        ItemType = ItemTypes.UniqueMap;
                        break;
                    case ItemRarity.Unique or ItemRarity.Normal when itemEntity.HasComponent<Weapon>():
                        ItemType = ItemTypes.UniqueWeapon;
                        break;
                }
        }
        catch (Exception exception)
        {
            if (Core.Settings.Debug)
                Core.LogError($"Ninja Pricer.CustomItem Error:{Environment.NewLine}{exception}");
        }

    }
}