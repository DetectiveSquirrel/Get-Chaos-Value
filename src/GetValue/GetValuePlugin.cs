using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GetValue.poe_ninja_api;
using GetValue.poe_ninja_api.Classes;
using Newtonsoft.Json;
using PoeHUD.Models;
using PoeHUD.Models.Enums;
using PoeHUD.Plugins;
using PoeHUD.Poe;
using PoeHUD.Poe.Components;
using PoeHUD.Poe.Elements;
using PoeHUD.Poe.EntityComponents;
using SharpDX;
using Map = PoeHUD.Poe.Components.Map;

namespace GetValue
{
    public class GetValuePlugin : BaseSettingsPlugin<GetValueSettings>
    {
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
            Settings.ReloadButton.OnPressed += Load;
            _ninjaDirectory = PluginDirectory + "\\NinjaData\\";

            // Make folder if it doesnt exist
            var file = new FileInfo(_ninjaDirectory);
            file.Directory?.Create(); // If the directory already exists, this method does nothing.

            Load();
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
                Settings.LeagueList.Value = CurrentLeague;
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

            if (!DownloadDone || !InitJsonDone) return;
            var lineCount = 0;
            var window = GameController.Window.GetWindowRectangle();
            var drawPos = window.TopLeft;
            window.Width = 400;
            window.Height = 15;

            var uiHover = GameController.Game.IngameState.UIHover;
            var inventoryItemIcon = uiHover.AsObject<HoverItemIcon>();
            if (inventoryItemIcon == null)
                return;
            var tooltip = inventoryItemIcon.Tooltip;
            var poeEntity = inventoryItemIcon.Item;

            if (tooltip == null || poeEntity.Address == 0 || !poeEntity.IsValid) return;

            var item = inventoryItemIcon.Item;

            var baseItemType = GameController.Files.BaseItemTypes.Translate(item.Path);
            if (baseItemType == null)
                return;
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

        private void ShowChaosValue(RectangleF window, Vector2 textPos, Entity itemEntity, string className,
            string path,
            bool identified, string uniqueItemName, string baseItemName, bool isMap, ItemRarity itemRarity,
            int lineCount, bool stackable)
        {
            if (itemRarity != ItemRarity.Unique && isMap)
            {
                if (itemEntity == null) return;
                if (WhiteMaps.Lines.Find(x => x.Name == baseItemName && x.Variant == "Atlas") == null)
                    return;

                var item = WhiteMaps.Lines.Find(x => x.Name == baseItemName && x.Variant == "Atlas");
                var text = $"Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";

                DrawText(ref textPos, ref lineCount, text);
                BackgroundBox(window, lineCount);
            }

            else if (itemRarity == ItemRarity.Unique && isMap)
            {
                if (itemEntity == null) return;
                if (UniqueMaps.Lines.Find(x => x.BaseType == baseItemName) == null)
                    return;

                var item = UniqueMaps.Lines.Find(x => x.BaseType == baseItemName);
                var text = $"Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";

                DrawText(ref textPos, ref lineCount, text);
                BackgroundBox(window, lineCount);
            }

            else if (path.Contains("Currency") && baseItemName != "Chaos Orb" && !baseItemName.Contains("Shard") &&
                     !baseItemName.Contains("Essence") && !baseItemName.Contains("Remnant of") &&
                     !baseItemName.Contains("Wisdom") && baseItemName != "Prophecy")
            {
                if (itemEntity == null) return;
                if (Currency.Lines.Find(x => x.CurrencyTypeName == baseItemName) == null)
                    return;

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

            else if (path.Contains("Currency") && baseItemName.Contains("Shard") && !baseItemName.Contains("Essence") &&
                     !baseItemName.Contains("Remnant of") && !baseItemName.Contains("Wisdom") &&
                     baseItemName != "Prophecy")
            {
                if (itemEntity == null) return;
                switch (baseItemName)
                {
                    case "Transmutation Shard":
                        GetShardValues("Orb of Transmutation", textPos, lineCount, stackable, window,
                            itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Alteration Shard":
                        GetShardValues("Orb of Alteration", textPos, lineCount, stackable, window,
                            itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Annulment Shard":
                        GetShardValues("Orb of Annulment", textPos, lineCount, stackable, window,
                            itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Exalted Shard":
                        GetShardValues("Exalted Orb", textPos, lineCount, stackable, window,
                            itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Mirror Shard":
                        GetShardValues("Mirror of Kalandra", textPos, lineCount, stackable, window,
                            itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Regal Shard":
                        GetShardValues("Regal Orb", textPos, lineCount, stackable, window,
                            itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Alchemy Shard":
                        GetShardValues("Orb of Alchemy", textPos, lineCount, stackable, window,
                            itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Chaos Shard":
                        GetShardValues("Chaos Orb", textPos, lineCount, stackable, window,
                            itemEntity.GetComponent<Stack>().Size);
                        break;
                    // Harb Orbs
                    case "Ancient Shard":
                        GetShardValues("Ancient Orb", textPos, lineCount, stackable, window,
                            itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Engineer's Shard":
                        GetShardValues("Engineer's Orb", textPos, lineCount, stackable, window,
                            itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Harbinger's Shard":
                        GetShardValues("Harbinger's Orb", textPos, lineCount, stackable, window,
                            itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Horizon Shard":
                        GetShardValues("Orb of Horizons", textPos, lineCount, stackable, window,
                            itemEntity.GetComponent<Stack>().Size);
                        break;
                    case "Binding Shard":
                        GetShardValues("Orb of Binding", textPos, lineCount, stackable, window,
                            itemEntity.GetComponent<Stack>().Size);
                        break;
                }
            }

            else if (baseItemName.Contains("Essence") || baseItemName.Contains("Remnant of"))
            {
                if (itemEntity == null) return;
                if (Essences.Lines.Find(x => x.Name == baseItemName) == null)
                    return;

                var item = Essences.Lines.Find(x => x.Name == baseItemName);
                var text = $"Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";

                DrawText(ref textPos, ref lineCount, text);
                BackgroundBox(window, lineCount);
            }

            else if (className.Contains("Divination"))
            {
                if (itemEntity == null) return;
                if (DivinationCards.Lines.Find(x => x.Name == baseItemName) == null)
                    return;

                var item = DivinationCards.Lines.Find(x => x.Name == baseItemName);
                var text = $"Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";

                DrawText(ref textPos, ref lineCount, text);
                BackgroundBox(window, lineCount);
            }

            else if (className == "Map Fragments" || baseItemName == "Offering to the Goddess")
            {
                if (itemEntity == null) return;
                if (Fragments.Lines.Find(x => x.CurrencyTypeName == baseItemName) == null)
                    return;

                var item = Fragments.Lines.Find(x => x.CurrencyTypeName == baseItemName);
                var text =
                    $"Chaos: {item.ChaosEquivalent} || Change last 7 days: {item.ReceiveSparkLine.TotalChange}%";

                DrawText(ref textPos, ref lineCount, text);
                BackgroundBox(window, lineCount);
            }

            else
            {
                switch (itemRarity)
                {
                    case ItemRarity.Unique when (className == "Amulets" || className == "Rings" || className == "Belts") && identified:
                        if (itemEntity == null) return;
                        const string taliosSignCorrect = "Tasalio's Sign";
                        const string taliosSignIncorrect = "Tasalio’s Sign";

                        if (UniqueAccessories.Lines.Find(x => x.Name == uniqueItemName) == null)
                            return;
                        if (UniqueAccessories.Lines.Find(x => x.Name == taliosSignCorrect) == null)
                            return;

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
                                return;

                            var item = UniqueFlasks.Lines.Find(x => x.Name == uniqueItemName);
                            var text = $"Chaos: {item.ChaosValue} || Change last 7 days: {item.Sparkline.TotalChange}%";

                            DrawText(ref textPos, ref lineCount, text);
                        }

                        BackgroundBox(window, lineCount);
                        break;
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
                    default:
                        if (baseItemName.Contains("Breachstone"))
                        {
                            if (Fragments.Lines.Find(x => x.CurrencyTypeName == baseItemName) == null)
                                return;

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

        private void GetShardValues(string orbParent, Vector2 textPos, int lineCount, bool stackable, RectangleF window,
            int stackSize)
        {
            if (orbParent != "Chaos Orb")
            {
                if (Currency.Lines.Find(x => x.CurrencyTypeName == orbParent) == null)
                    return;

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
    }
}