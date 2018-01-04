using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GetValue.poe_ninja_api;
using GetValue.poe_ninja_api.Classes;
using Newtonsoft.Json;
using PoeHUD.Models;
using PoeHUD.Models.Enums;
using PoeHUD.Models.Interfaces;
using PoeHUD.Plugins;
using PoeHUD.Poe.Components;
using PoeHUD.Poe.Elements;
using PoeHUD.Poe.EntityComponents;
using SharpDX;
using SharpDX.Direct3D9;
using Map = PoeHUD.Poe.Components.Map;

namespace GetValue
{
    public class GetValuePlugin : BaseSettingsPlugin<GetValueSettings>
    {
        private const int NOT_FOUND = -1;
        private readonly Stopwatch _reloadStopWatch = Stopwatch.StartNew();
        private string _ninjaDirectory;
        public Currency.RootObject Currency;
        public DivinationCards.RootObject DivinationCards;
        public bool DownloadDone;
        public Essences.RootObject Essences;
        public Fragments.RootObject Fragments;
        public bool InitJsonDone;
        public string PoeLeagueApiList = "http://api.pathofexile.com/leagues?type=main&compact=1";
        public Prophecies.RootObject Prophecies;
        public UniqueAccessories.RootObject UniqueAccessories;
        public UniqueArmours.RootObject UniqueArmours;
        public UniqueFlasks.RootObject UniqueFlasks;
        public UniqueJewels.RootObject UniqueJewels;
        public UniqueMaps.RootObject UniqueMaps;
        public UniqueWeapons.RootObject UniqueWeapons;
        public WhiteMaps.RootObject WhiteMaps;

        public string CurrentLeague { get; private set; }

        public override void Initialise()
        {
            if (Settings.VisibleStashValue.Value)
            {
                LoadVisibleStashSettings();
            }
            Settings.VisibleStashValue.OnValueChanged += LoadVisibleStashSettings;

            Settings.ReloadButton.OnPressed += Load;
            _ninjaDirectory = PluginDirectory + "\\NinjaData\\";

            // Make folder if it doesnt exist
            var file = new FileInfo(_ninjaDirectory);
            file.Directory?.Create(); // If the directory already exists, this method does nothing.

            Load();
        }

        private void LoadVisibleStashSettings()
        {
            // Visible Stash Settings
            var windowSize = GameController.Window.GetWindowRectangle().Size;
            Settings.X.Max = (int)windowSize.Width;
            Settings.Y.Max = (int)windowSize.Height;

            // check if image exists, if it doesn't, download it.
            var fileName = $"{PluginDirectory}//images//Chaos_Orb_inventory_icon.png";
            if (File.Exists(fileName))
            {
                return;
            }

            Directory.CreateDirectory($"{PluginDirectory}//images//");

            using (var client = new WebClient())
            {
                client.DownloadFile(new Uri("https://d1u5p3l4wpay3k.cloudfront.net/pathofexile_gamepedia/9/9c/Chaos_Orb_inventory_icon.png"), fileName);
            }
        }

        private void GatherLeagueNames()
        {
            string json;
            // Download it
            Api.SaveJson(_ninjaDirectory + "Leagues.json",
                Api.DownloadApi(PoeLeagueApiList));

            using (var r = new StreamReader(_ninjaDirectory + "Leagues.json"))
            {
                json = r.ReadToEnd();
            }

            var gatheredLeagues = Leagues.FromJson(json);
            var leagueList = (from league in gatheredLeagues where !league.Id.Contains("SSF") select league.Id)
                .ToList();

            CurrentLeague = leagueList[0];
            Settings.LeagueList.SetListValues(leagueList);
        }

        private void InitPoeNinjaData()
        {
            using (var r = new StreamReader(_ninjaDirectory + "Currency.json"))
            {
                var json = r.ReadToEnd();
                Currency = JsonConvert.DeserializeObject<Currency.RootObject>(json);
            }
            using (var r = new StreamReader(_ninjaDirectory + "DivinationCards.json"))
            {
                var json = r.ReadToEnd();
                DivinationCards = JsonConvert.DeserializeObject<DivinationCards.RootObject>(json);
            }
            using (var r = new StreamReader(_ninjaDirectory + "Essences.json"))
            {
                var json = r.ReadToEnd();
                Essences = JsonConvert.DeserializeObject<Essences.RootObject>(json);
            }
            using (var r = new StreamReader(_ninjaDirectory + "Fragments.json"))
            {
                var json = r.ReadToEnd();
                Fragments = JsonConvert.DeserializeObject<Fragments.RootObject>(json);
            }
            using (var r = new StreamReader(_ninjaDirectory + "Prophecies.json"))
            {
                var json = r.ReadToEnd();
                Prophecies = JsonConvert.DeserializeObject<Prophecies.RootObject>(json);
            }
            using (var r = new StreamReader(_ninjaDirectory + "UniqueAccessories.json"))
            {
                var json = r.ReadToEnd();
                UniqueAccessories = JsonConvert.DeserializeObject<UniqueAccessories.RootObject>(json);
            }
            using (var r = new StreamReader(_ninjaDirectory + "UniqueArmours.json"))
            {
                var json = r.ReadToEnd();
                UniqueArmours = JsonConvert.DeserializeObject<UniqueArmours.RootObject>(json);
            }
            using (var r = new StreamReader(_ninjaDirectory + "UniqueFlasks.json"))
            {
                var json = r.ReadToEnd();
                UniqueFlasks = JsonConvert.DeserializeObject<UniqueFlasks.RootObject>(json);
            }
            using (var r = new StreamReader(_ninjaDirectory + "UniqueJewels.json"))
            {
                var json = r.ReadToEnd();
                UniqueJewels = JsonConvert.DeserializeObject<UniqueJewels.RootObject>(json);
            }
            using (var r = new StreamReader(_ninjaDirectory + "UniqueMaps.json"))
            {
                var json = r.ReadToEnd();
                UniqueMaps = JsonConvert.DeserializeObject<UniqueMaps.RootObject>(json);
            }
            using (var r = new StreamReader(_ninjaDirectory + "UniqueWeapons.json"))
            {
                var json = r.ReadToEnd();
                UniqueWeapons = JsonConvert.DeserializeObject<UniqueWeapons.RootObject>(json);
            }
            using (var r = new StreamReader(_ninjaDirectory + "WhiteMaps.json"))
            {
                var json = r.ReadToEnd();
                WhiteMaps = JsonConvert.DeserializeObject<WhiteMaps.RootObject>(json);
            }
        }

        public void Load()
        {
            DownloadDone = false;
            InitJsonDone = false;

            GatherLeagueNames();

            // display default league in setting
            if (Settings.LeagueList.Value == null)
            {
                Settings.LeagueList.Value = CurrentLeague;
            }
            // set wanted league
            CurrentLeague = Settings.LeagueList.Value;

            Task.Run(() =>
            {
                LogMessage("Gathering Data from Poe.Ninja.", 5);
                DownloadPoeNinjaApi(CurrentLeague);
                DownloadDone = true;
                LogMessage("Finished Gathering Data from Poe.Ninja.", 5);
            });
        }

        private void DownloadPoeNinjaApi(string league)
        {
            Api.SaveJson(_ninjaDirectory + "Currency.json",
                Api.DownloadApi($"http://cdn.poe.ninja/api/Data/GetCurrencyOverview?league={league}"));
            Api.SaveJson(_ninjaDirectory + "DivinationCards.json",
                Api.DownloadApi(
                    $"http://cdn.poe.ninja/api/Data/GetDivinationCardsOverview?league={league}"));
            Api.SaveJson(_ninjaDirectory + "Essences.json",
                Api.DownloadApi($"http://cdn.poe.ninja/api/Data/GetEssenceOverview?league={league}"));
            Api.SaveJson(_ninjaDirectory + "Fragments.json",
                Api.DownloadApi($"http://cdn.poe.ninja/api/Data/GetFragmentOverview?league={league}"));
            Api.SaveJson(_ninjaDirectory + "Prophecies.json",
                Api.DownloadApi($"http://cdn.poe.ninja/api/Data/GetProphecyOverview?league={league}"));
            Api.SaveJson(_ninjaDirectory + "UniqueAccessories.json",
                Api.DownloadApi(
                    $"http://cdn.poe.ninja/api/Data/GetUniqueAccessoryOverview?league={league}"));
            Api.SaveJson(_ninjaDirectory + "UniqueArmours.json",
                Api.DownloadApi(
                    $"http://cdn.poe.ninja/api/Data/GetUniqueArmourOverview?league={league}"));
            Api.SaveJson(_ninjaDirectory + "UniqueFlasks.json",
                Api.DownloadApi($"http://cdn.poe.ninja/api/Data/GetUniqueFlaskOverview?league={league}"));
            Api.SaveJson(_ninjaDirectory + "UniqueJewels.json",
                Api.DownloadApi($"http://cdn.poe.ninja/api/Data/GetUniqueJewelOverview?league={league}"));
            Api.SaveJson(_ninjaDirectory + "UniqueMaps.json",
                Api.DownloadApi($"http://cdn.poe.ninja/api/Data/GetUniqueMapOverview?league={league}"));
            Api.SaveJson(_ninjaDirectory + "UniqueWeapons.json",
                Api.DownloadApi(
                    $"http://cdn.poe.ninja/api/Data/GetUniqueWeaponOverview?league={league}"));
            Api.SaveJson(_ninjaDirectory + "WhiteMaps.json",
                Api.DownloadApi($"http://cdn.poe.ninja/api/Data/GetMapOverview?league={league}"));
        }

        public override void Render()
        {
            base.Render();

            if (DownloadDone && !InitJsonDone)
            {
                GatherLeagueNames();
                InitPoeNinjaData();
                InitJsonDone = true;
            }

            if (DownloadDone && InitJsonDone && Settings.AutoReload &&
                _reloadStopWatch.ElapsedMilliseconds > 1000 * 60 * Settings.AutoReloadTimer.Value)
            {
                Load();
                _reloadStopWatch.Restart();
            }

            if (!DownloadDone || !InitJsonDone)
            {
                return;
            }

            try
            {
                var stashPanel = GameController.Game.IngameState.ServerData.StashPanel;
            

            if (Settings.VisibleStashValue.Value && stashPanel.IsVisible)
            {
                var inventoryItems = stashPanel.VisibleStash.VisibleInventoryItems;
                double sum = 0;
                foreach (var normalInventoryItem in inventoryItems)
                {
                    
                        if (normalInventoryItem == null)
                        {
                            return;
                        }

                        var temp = GetChaosValue(normalInventoryItem);
                        if (temp == NOT_FOUND)
                        {
                            if (Settings.Debug.Value)
                            {
                                Graphics.DrawText("NOT FOUND", 12, normalInventoryItem.GetClientRect().Center,
                                    FontDrawFlags.Center);
                                Graphics.DrawFrame(normalInventoryItem.GetClientRect(), 2, Color.Aqua);
                            }
                            continue;
                        }

                        sum += temp;
                    
                }


                var color = Settings.ColorNode.Value;
                var pos = new Vector2(Settings.X.Value, Settings.Y.Value);

                var significantDigits = Math.Round((decimal) sum, Settings.SignificantDigits.Value);
                Graphics.DrawText(
                    DrawImage($"{PluginDirectory}//images//Chaos_Orb_inventory_icon.png",
                        new RectangleF(Settings.X.Value - Settings.FontSize.Value, Settings.Y.Value, Settings.FontSize.Value,
                            Settings.FontSize.Value))
                        ? $"{significantDigits}"
                        : $"{significantDigits} Chaos", Settings.FontSize.Value, pos, color);
            }
            }
            catch
            {
                // Divination card tab ugh.
            }

            var lineCount = 0;
            var window = GameController.Window.GetWindowRectangle();
            var drawPos = window.TopLeft;
            window.Width = 400;
            window.Height = 15;

            var uiHover = GameController.Game.IngameState.UIHover;
            var inventoryItemIcon = uiHover.AsObject<HoverItemIcon>();
            if (inventoryItemIcon == null)
            {
                return;
            }
            var tooltip = inventoryItemIcon.Tooltip;
            var poeEntity = inventoryItemIcon.Item;

            if (tooltip == null || poeEntity.Address == 0 || !poeEntity.IsValid)
            {
                return;
            }

            var item = inventoryItemIcon.Item;

            var baseItemType = GameController.Files.BaseItemTypes.Translate(item.Path);
            if (baseItemType == null)
            {
                return;
            }
            var className = GetClassName(baseItemType);
            item.GetComponent<Base>();
            var mods = item.GetComponent<Mods>();

            var path = item.Path;


            var stack = item.GetComponent<Stack>();
            var stackable = stack?.Info != null;

            var identified = mods.Identified;
            var uniqueItemName = mods.UniqueName;
            var baseItemName = baseItemType.BaseName;
            var isMap = item.HasComponent<Map>();
            var itemRarity = mods.ItemRarity;
            ShowChaosValue(window, drawPos, item, className, path, identified, uniqueItemName, baseItemName,
                isMap, itemRarity, lineCount, stackable);
        }

        private string GetClassName(BaseItemType baseItemType)
        {
            var className = GameController.Files.itemClasses.contents.TryGetValue(baseItemType.ClassName, out var tmp)
                ? tmp.ClassName
                : baseItemType.ClassName;
            return className;
        }


        private void ShowChaosValue(RectangleF window, Vector2 textPos, IEntity itemEntity, string className,
            string path,
            bool identified, string uniqueItemName, string baseItemName, bool isMap, ItemRarity itemRarity,
            int lineCount, bool stackable)
        {
            #region Normal Maps

            if (itemRarity != ItemRarity.Unique && isMap)
            {
                if (itemEntity == null)
                {
                    return;
                }
                if (WhiteMaps.Lines.Find(x => x.Name == baseItemName && x.Variant == "Atlas") == null)
                {
                    return;
                }

                var item = WhiteMaps.Lines.Find(x => x.Name == baseItemName && x.Variant == "Atlas");
                var text = $"Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";

                DrawText(ref textPos, ref lineCount, text);
                BackgroundBox(window, lineCount);
            }

            #endregion

            #region Unique Maps

            else if (itemRarity == ItemRarity.Unique && isMap)
            {
                if (itemEntity == null)
                {
                    return;
                }
                if (UniqueMaps.Lines.Find(x => x.BaseType == baseItemName) == null)
                {
                    return;
                }

                var item = UniqueMaps.Lines.Find(x => x.BaseType == baseItemName);
                var text = $"Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";

                DrawText(ref textPos, ref lineCount, text);
                BackgroundBox(window, lineCount);
            }

            #endregion

            #region Currency, but NOT Chaos Orbs, Shards, Essences, Wisdom Scrolls or Prohecies.

            else if (path.Contains("Currency") && baseItemName != "Chaos Orb" && !baseItemName.Contains("Shard") &&
                     !baseItemName.Contains("Essence") && !baseItemName.Contains("Remnant of") &&
                     !baseItemName.Contains("Wisdom") && baseItemName != "Prophecy")
            {
                if (itemEntity == null)
                {
                    return;
                }
                if (Currency.Lines.Find(x => x.CurrencyTypeName == baseItemName) == null)
                {
                    return;
                }

                var item = Currency.Lines.Find(x => x.CurrencyTypeName == baseItemName);
                var text =
                    $"Chaos: {item.ChaosEquivalent} || Change last 7 days: {item.ReceiveSparkLine.TotalChange}%";

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

            else if (path.Contains("Currency") && baseItemName.Contains("Shard") && !baseItemName.Contains("Essence") &&
                     !baseItemName.Contains("Remnant of") && !baseItemName.Contains("Wisdom") &&
                     baseItemName != "Prophecy")
            {
                if (itemEntity == null)
                {
                    return;
                }
                switch (baseItemName)
                {
                    case "Transmutation Shard":
                        ShowShardValues("Orb of Transmutation", textPos, lineCount, stackable, window,
                            itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Alteration Shard":
                        ShowShardValues("Orb of Alteration", textPos, lineCount, stackable, window,
                            itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Annulment Shard":
                        ShowShardValues("Orb of Annulment", textPos, lineCount, stackable, window,
                            itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Exalted Shard":
                        ShowShardValues("Exalted Orb", textPos, lineCount, stackable, window,
                            itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Mirror Shard":
                        ShowShardValues("Mirror of Kalandra", textPos, lineCount, stackable, window,
                            itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Regal Shard":
                        ShowShardValues("Regal Orb", textPos, lineCount, stackable, window,
                            itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Alchemy Shard":
                        ShowShardValues("Orb of Alchemy", textPos, lineCount, stackable, window,
                            itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Chaos Shard":
                        ShowShardValues("Chaos Orb", textPos, lineCount, stackable, window,
                            itemEntity.GetComponent<Stack>().Size);
                        break;
                    // Harb Orbs
                    case "Ancient Shard":
                        ShowShardValues("Ancient Orb", textPos, lineCount, stackable, window,
                            itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Engineer's Shard":
                        ShowShardValues("Engineer's Orb", textPos, lineCount, stackable, window,
                            itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Harbinger's Shard":
                        ShowShardValues("Harbinger's Orb", textPos, lineCount, stackable, window,
                            itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Horizon Shard":
                        ShowShardValues("Orb of Horizons", textPos, lineCount, stackable, window,
                            itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Binding Shard":
                        ShowShardValues("Orb of Binding", textPos, lineCount, stackable, window,
                            itemEntity.GetComponent<Stack>().Size);
                        break;
                }
            }

            #endregion

            #region Essences

            else if (baseItemName.Contains("Essence") || baseItemName.Contains("Remnant of"))
            {
                if (itemEntity == null)
                {
                    return;
                }
                if (Essences.Lines.Find(x => x.Name == baseItemName) == null)
                {
                    return;
                }

                var item = Essences.Lines.Find(x => x.Name == baseItemName);
                var text = $"Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";

                DrawText(ref textPos, ref lineCount, text);
                BackgroundBox(window, lineCount);
            }

            #endregion

            #region Divination Cards

            else if (className.Contains("Divination"))
            {
                if (itemEntity == null)
                {
                    return;
                }
                if (DivinationCards.Lines.Find(x => x.Name == baseItemName) == null)
                {
                    return;
                }

                var item = DivinationCards.Lines.Find(x => x.Name == baseItemName);
                var text = $"Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";

                DrawText(ref textPos, ref lineCount, text);
                BackgroundBox(window, lineCount);
            }

            #endregion

            #region Map Fragments and Offerings

            else if (className == "Map Fragments" || baseItemName == "Offering to the Goddess")
            {
                if (itemEntity == null)
                {
                    return;
                }
                if (Fragments.Lines.Find(x => x.CurrencyTypeName == baseItemName) == null)
                {
                    return;
                }

                var item = Fragments.Lines.Find(x => x.CurrencyTypeName == baseItemName);
                var text =
                    $"Chaos: {item.ChaosEquivalent} || Change last 7 days: {item.ReceiveSparkLine.TotalChange}%";

                DrawText(ref textPos, ref lineCount, text);
                BackgroundBox(window, lineCount);
            }

            #endregion

            else
            {
                switch (itemRarity)
                {
                    #region Amulets, Rings and Belts

                    case ItemRarity.Unique when
                    (className == "Amulets" || className == "Rings" || className == "Belts") && identified:
                        if (itemEntity == null)
                        {
                            return;
                        }
                        const string taliosSignCorrect = "Tasalio's Sign";
                        const string taliosSignIncorrect = "Tasalio’s Sign";

                        if (UniqueAccessories.Lines.Find(x => x.Name == uniqueItemName) == null)
                        {
                            return;
                        }
                        if (UniqueAccessories.Lines.Find(x => x.Name == taliosSignCorrect) == null)
                        {
                            return;
                        }

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

                    case ItemRarity.Unique when (itemEntity.HasComponent<Armour>() || className == "Quivers") &&
                                                identified:
                        const string victariosFlightCorrect = "Victario's Flight";
                        const string victariosFlightIncorrect = "Ondar's Flight";

                        if (uniqueItemName == victariosFlightIncorrect &&
                            UniqueArmours.Lines.Find(x => x.Name == victariosFlightCorrect) != null)
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
                                var text =
                                    $"Links: 0 || Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";

                                DrawText(ref textPos, ref lineCount, text);
                            }
                            if (UniqueArmours.Lines.Find(x => x.Name == uniqueItemName && x.Links == 5) != null)
                            {
                                var item = UniqueArmours.Lines.Find(x => x.Name == uniqueItemName && x.Links == 5);
                                var text =
                                    $"Links: 5 || Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";

                                DrawText(ref textPos, ref lineCount, text);
                            }
                            if (UniqueArmours.Lines.Find(x => x.Name == uniqueItemName && x.Links == 6) != null)
                            {
                                var item = UniqueArmours.Lines.Find(x => x.Name == uniqueItemName && x.Links == 6);
                                var text =
                                    $"Links: 6 || Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";

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
                            if (UniqueFlasks.Lines.Find(x =>
                                    x.Name == uniqueItemName && x.Variant == "Added Attacks") !=
                                null)
                            {
                                var item = UniqueFlasks.Lines.Find(x =>
                                    x.Name == uniqueItemName && x.Variant == "Added Attacks");
                                var text =
                                    $"{item.Variant} || Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";

                                DrawText(ref textPos, ref lineCount, text);
                            }
                            if (UniqueFlasks.Lines.Find(x => x.Name == uniqueItemName && x.Variant == "Penetration") !=
                                null)
                            {
                                var item = UniqueFlasks.Lines.Find(x =>
                                    x.Name == uniqueItemName && x.Variant == "Penetration");
                                var text =
                                    $"{item.Variant} || Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";

                                DrawText(ref textPos, ref lineCount, text);
                            }
                            if (UniqueFlasks.Lines.Find(x => x.Name == uniqueItemName && x.Variant == "Added Spells") !=
                                null)
                            {
                                var item = UniqueFlasks.Lines.Find(x =>
                                    x.Name == uniqueItemName && x.Variant == "Added Spells");
                                var text =
                                    $"{item.Variant} || Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";

                                DrawText(ref textPos, ref lineCount, text);
                            }
                            if (UniqueFlasks.Lines.Find(x => x.Name == uniqueItemName && x.Variant == "Conversion") !=
                                null)
                            {
                                var item = UniqueFlasks.Lines.Find(x =>
                                    x.Name == uniqueItemName && x.Variant == "Conversion");
                                var text =
                                    $"{item.Variant} || Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";

                                DrawText(ref textPos, ref lineCount, text);
                            }
                        }
                        else
                        {
                            if (UniqueFlasks.Lines.Find(x => x.Name == uniqueItemName) == null)
                            {
                                return;
                            }

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
                        else if (uniqueItemName == incorrectOne &&
                                 UniqueJewels.Lines.Find(x => x.Name == correctOne) != null)
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
                            var text =
                                $"Links: 0 || Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";

                            DrawText(ref textPos, ref lineCount, text);
                        }
                        if (UniqueWeapons.Lines.Find(x => x.Name == uniqueItemName && x.Links == 5) != null)
                        {
                            var item = UniqueWeapons.Lines.Find(x => x.Name == uniqueItemName && x.Links == 5);
                            var text =
                                $"Links: 5 || Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";

                            DrawText(ref textPos, ref lineCount, text);
                        }
                        if (UniqueWeapons.Lines.Find(x => x.Name == uniqueItemName && x.Links == 6) != null)
                        {
                            var item = UniqueWeapons.Lines.Find(x => x.Name == uniqueItemName && x.Links == 6);
                            var text =
                                $"Links: 6 || Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";

                            DrawText(ref textPos, ref lineCount, text);
                        }

                        BackgroundBox(window, lineCount);
                        break;

                    #endregion

                    default:
                        if (baseItemName.Contains("Breachstone"))
                        {
                            if (Fragments.Lines.Find(x => x.CurrencyTypeName == baseItemName) == null)
                            {
                                return;
                            }

                            var item = Fragments.Lines.Find(x => x.CurrencyTypeName == baseItemName);
                            var text =
                                $"Chaos: {item.ChaosEquivalent} || Change last 7 days: {item.ReceiveSparkLine.TotalChange}%";
                            DrawText(ref textPos, ref lineCount, text);
                            BackgroundBox(window, lineCount);
                        }
                        break;
                }
            }
        }

        private void DrawText(ref Vector2 textPos, ref int lineCount, string text)
        {
            Graphics.DrawText(text, 15, textPos);
            lineCount++;
            textPos.Y += 15;
        }

        private void BackgroundBox(RectangleF window, int lineCount)
        {
            window.Height *= lineCount;
            Graphics.DrawBox(window, new Color(0, 0, 0, 240));
        }

        private void ShowShardValues(string orbParent, Vector2 textPos, int lineCount, bool stackable,
            RectangleF window,
            int stackSize)
        {
            if (orbParent != "Chaos Orb")
            {
                if (Currency.Lines.Find(x => x.CurrencyTypeName == orbParent) == null)
                {
                    return;
                }

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

        /// <summary>
        /// This function is made by Github.com/Nymann
        /// </summary>
        /// <param name="normalInventoryItem"></param>
        /// <returns></returns>
        private double GetChaosValue(NormalInventoryItem normalInventoryItem)
        {
            var itemEntity = normalInventoryItem.Item;
            var itemRarity = itemEntity.GetComponent<Mods>().ItemRarity;
            var isMap = itemEntity.HasComponent<Map>();
            var baseType = GameController.Files.BaseItemTypes.Translate(itemEntity.Path);
            var baseItemName = baseType.BaseName;
            var classItemName = baseType.ClassName;
            var path = itemEntity.Path;
            var stack = itemEntity.GetComponent<Stack>();
            var stackable = stack?.Info != null;

            if (baseItemName.Equals("Scroll of Wisdom") || baseItemName.Equals("Scroll Fragment"))
            {
                return 0;
            }


            #region Normal Maps

            if (itemRarity != ItemRarity.Unique && isMap)
            {
                if (WhiteMaps.Lines.Find(x => x.Name == baseItemName && x.Variant == "Atlas") == null)
                {
                    return NOT_FOUND;
                }
            }

            #endregion

            #region Unique Maps

            else if (itemRarity == ItemRarity.Unique && isMap)
            {
                if (UniqueMaps.Lines.Find(x => x.BaseType == baseItemName) == null)
                {
                    return NOT_FOUND;
                }

                var item = UniqueMaps.Lines.Find(x => x.BaseType == baseItemName);
                return item.ChaosValue;
            }

            #endregion

            #region Currency, but NOT Shards, Essences, Wisdom Scrolls or Prohecies.

            else if (path.Contains("Currency") && !baseItemName.Contains("Shard") &&
                     !baseItemName.Contains("Essence") && !baseItemName.Contains("Remnant of") &&
                     !baseItemName.Contains("Wisdom") && baseItemName != "Prophecy")
            {
                if (baseItemName.Equals("Chaos Orb"))
                {
                    // If we can't get stack size which we always should be able to, just return 1.
                    return stack?.Size ?? 1;
                }

                if (Currency.Lines.Find(x => x.CurrencyTypeName == baseItemName) == null)
                {
                    return NOT_FOUND;
                }

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

            else if (path.Contains("Currency") && baseItemName.Contains("Shard") && !baseItemName.Contains("Essence") &&
                     !baseItemName.Contains("Remnant of") && !baseItemName.Contains("Wisdom") &&
                     baseItemName != "Prophecy")
            {
                var stackSize = stack?.Size ?? 1;
                return GetShardValues(baseItemName, stackSize);
            }

            #endregion

            #region Essences

            else if (baseItemName.Contains("Essence") || baseItemName.Contains("Remnant of"))
            {
                if (Essences.Lines.Find(x => x.Name == baseItemName) == null)
                {
                    return NOT_FOUND;
                }

                var item = Essences.Lines.Find(x => x.Name == baseItemName);
                if (stackable)
                {
                    var priceForTheStack = stack.Size * item.ChaosValue;
                    return priceForTheStack;
                }
                return item.ChaosValue;
            }

            #endregion

            #region Divination Cards

            else if (classItemName.Contains("Divination"))
            {
                if (DivinationCards.Lines.Find(x => x.Name == baseItemName) == null)
                {
                    return NOT_FOUND;
                }

                var item = DivinationCards.Lines.Find(x => x.Name == baseItemName);
                if (stackable)
                {
                    var priceForTheStack = stack.Size * item.ChaosValue;
                    return priceForTheStack;
                }

                return item.ChaosValue;
            }

            #endregion

            #region Map Fragments and Offerings

            else if (classItemName.Equals("Map Fragments") || baseItemName.Equals("Offering to the Goddess"))
            {
                if (Fragments.Lines.Find(x => x.CurrencyTypeName == baseItemName) == null)
                {
                    return NOT_FOUND;
                }

                var item = Fragments.Lines.Find(x => x.CurrencyTypeName == baseItemName);
                return item.ChaosEquivalent;
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

                    case ItemRarity.Unique when
                    (classItemName == "Amulets" || classItemName == "Rings" || classItemName == "Belts") && identified:
                        const string taliosSignCorrect = "Tasalio's Sign";
                        const string taliosSignIncorrect = "Tasalio’s Sign";

                        if (UniqueAccessories.Lines.Find(x => x.Name == uniqueItemName) == null)
                        {
                            return NOT_FOUND;
                        }
                        if (UniqueAccessories.Lines.Find(x => x.Name == taliosSignCorrect) == null)
                        {
                            return NOT_FOUND;
                        }

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

                    case ItemRarity.Unique when (itemEntity.HasComponent<Armour>() || classItemName == "Quivers") &&
                                                identified:
                        const string victariosFlightCorrect = "Victario's Flight";
                        const string victariosFlightIncorrect = "Ondar's Flight";

                        if (uniqueItemName == victariosFlightIncorrect &&
                            UniqueArmours.Lines.Find(x => x.Name == victariosFlightCorrect) != null)
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
                            if (UniqueFlasks.Lines.Find(x =>
                                    x.Name == uniqueItemName && x.Variant == "Added Attacks") !=
                                null)
                            {
                                var item = UniqueFlasks.Lines.Find(x =>
                                    x.Name == uniqueItemName && x.Variant == "Added Attacks");
                                return item.ChaosValue;
                            }
                            if (UniqueFlasks.Lines.Find(x => x.Name == uniqueItemName && x.Variant == "Penetration") !=
                                null)
                            {
                                var item = UniqueFlasks.Lines.Find(x =>
                                    x.Name == uniqueItemName && x.Variant == "Penetration");
                                return item.ChaosValue;
                            }
                            if (UniqueFlasks.Lines.Find(x => x.Name == uniqueItemName && x.Variant == "Added Spells") !=
                                null)
                            {
                                var item = UniqueFlasks.Lines.Find(x =>
                                    x.Name == uniqueItemName && x.Variant == "Added Spells");
                                return item.ChaosValue;
                            }
                            if (UniqueFlasks.Lines.Find(x => x.Name == uniqueItemName && x.Variant == "Conversion") !=
                                null)
                            {
                                var item = UniqueFlasks.Lines.Find(x =>
                                    x.Name == uniqueItemName && x.Variant == "Conversion");
                                return item.ChaosValue;
                            }
                        }
                        else
                        {
                            if (UniqueFlasks.Lines.Find(x => x.Name == uniqueItemName) == null)
                            {
                                return NOT_FOUND;
                            }

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
                        else if (uniqueItemName.Equals(incorrectOne) &&
                                 UniqueJewels.Lines.Find(x => x.Name == correctOne) != null)
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
                            if (Fragments.Lines.Find(x => x.CurrencyTypeName == baseItemName) == null)
                            {
                                return NOT_FOUND;
                            }

                            var item = Fragments.Lines.Find(x => x.CurrencyTypeName == baseItemName);
                            return item.ChaosEquivalent;
                        }
                        break;
                }
            }

            return NOT_FOUND;
        }


        /// <summary>
        /// This function is made by Github.com/Nymann
        /// </summary>
        /// <param name="baseItemName"></param>
        /// <param name="stackSize"></param>
        /// <returns></returns>
        private double GetShardValues(string baseItemName, int stackSize)
        {
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
                {"Binding Shard", "Orb of Binding"}
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

            if (item == null)
            {
                return NOT_FOUND;
            }

            var value = item.ChaosEquivalent / 20 * stackSize;

            return value;
        }

        /// <summary>
        /// Draws a plugin image to screen.
        /// </summary>
        /// <param name="path"></param>
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