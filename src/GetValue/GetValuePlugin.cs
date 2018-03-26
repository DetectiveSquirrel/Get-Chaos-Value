using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using GetValue.poe_ninja_api;
using GetValue.poe_ninja_api.Classes;
using ImGuiNET;
using PoeHUD.Models.Enums;
using PoeHUD.Plugins;
using PoeHUD.Poe.Components;
using PoeHUD.Poe.Elements;
using Map = PoeHUD.Poe.Components.Map;

namespace GetValue
{
    public partial class GetValuePlugin : BaseSettingsPlugin<GetValueSettings>
    {
        private const int NotFound = -1;
        private readonly Stopwatch _reloadStopWatch = Stopwatch.StartNew();
        private string _ninjaDirectory;
        public DateTime buildDate;
        public Currency.RootObject Currency;
        public DivinationCards.RootObject DivinationCards;
        public bool DownloadDone;
        public Essences.RootObject Essences;
        public Fragments.RootObject Fragments;
        public bool InitJsonDone;
        public string PluginVersion;
        public string PoeLeagueApiList = "http://api.pathofexile.com/leagues?type=main&compact=1";
        public Prophecies.RootObject Prophecies;
        public UniqueAccessories.RootObject UniqueAccessories;
        public UniqueArmours.RootObject UniqueArmours;
        public UniqueFlasks.RootObject UniqueFlasks;
        public UniqueJewels.RootObject UniqueJewels;
        public UniqueMaps.RootObject UniqueMaps;

        public UniqueWeapons.RootObject UniqueWeapons;

        //https://stackoverflow.com/questions/826777/how-to-have-an-auto-incrementing-version-number-visual-studio
        public Version version = Assembly.GetExecutingAssembly().GetName().Version;
        public WhiteMaps.RootObject WhiteMaps;

        public GetValuePlugin() => PluginName = "Poe.Ninja Pricer";

        public string CurrentLeague { get; private set; }


        public override void Initialise()
        {
            buildDate = new DateTime(2000, 1, 1).AddDays(version.Build).AddSeconds(version.Revision * 2);
            PluginVersion = $"{version}";
            if (Settings.VisibleStashValue.Value) LoadVisibleStashSettings();
            Settings.VisibleStashValue.OnValueChanged += LoadVisibleStashSettings;
            if (Settings.HighlightUniqueJunk.Value) LoadHighligherSettings();
            Settings.HighlightUniqueJunk.OnValueChanged += LoadHighligherSettings;
            Settings.ReloadButton.OnPressed += Load;
            _ninjaDirectory = PluginDirectory + "\\NinjaData\\";

            // Make folder if it doesnt exist
            var file = new FileInfo(_ninjaDirectory);
            file.Directory?.Create(); // If the directory already exists, this method does nothing.
            Load();
        }

        public override void DrawSettingsMenu()
        {
            ImGui.BulletText($"v{PluginVersion}");
            ImGui.BulletText($"Last Updated: {buildDate}");
            base.DrawSettingsMenu();
        }

        private void LoadVisibleStashSettings()
        {
            // If the Visible Stash Value is deactivated, then return.
            // We do this since this method is called OnValueChanged (could be 'turn on' or 'turn off).
            if (!Settings.VisibleStashValue.Value) return;

            // Visible Stash Settings
            var windowSize = GameController.Window.GetWindowRectangle().Size;
            Settings.StashValueX.Max = (int) windowSize.Width;
            Settings.StashValueY.Max = (int) windowSize.Height;

            // check if image exists, if it doesn't, download it.
            var fileName = $"{PluginDirectory}//images//Chaos_Orb_inventory_icon.png";
            if (File.Exists(fileName)) return;
            Directory.CreateDirectory($"{PluginDirectory}//images//");
            using (var client = new WebClient())
            {
                client.DownloadFile(new Uri("https://d1u5p3l4wpay3k.cloudfront.net/pathofexile_gamepedia/9/9c/Chaos_Orb_inventory_icon.png"), fileName);
            }
        }

        private void LoadHighligherSettings()
        {
            // If the Visible Inventory Value is deactivated, then return.
            // We do this since this method is called OnValueChanged (could be 'turn on' or 'turn off).
            if (!Settings.HighlightUniqueJunk.Value) return;

            // check if image exists, if it doesn't, download it.
            var fileName = $"{PluginDirectory}//images//Chaos_Orb_inventory_icon.png";
            if (File.Exists(fileName)) return;
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
            Api.SaveJson(_ninjaDirectory + "Leagues.json", Api.DownloadApi(PoeLeagueApiList));
            using (var r = new StreamReader(_ninjaDirectory + "Leagues.json"))
            {
                json = r.ReadToEnd();
            }

            var gatheredLeagues = Leagues.FromJson(json);
            var leagueList = (from league in gatheredLeagues where !league.Id.Contains("SSF") select league.Id).ToList();
            CurrentLeague = leagueList[0];
            Settings.LeagueList.SetListValues(leagueList);
        }

        public void Load()
        {
            DownloadDone = false;
            InitJsonDone = false;
            GatherLeagueNames();

            // display default league in setting
            if (Settings.LeagueList.Value == null) Settings.LeagueList.Value = CurrentLeague;
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

        public override void Render()
        {
            base.Render();
            if (DownloadDone && !InitJsonDone)
            {
                GatherLeagueNames();
                InitPoeNinjaData();
                InitJsonDone = true;
            }

            if (DownloadDone && InitJsonDone && Settings.AutoReload && _reloadStopWatch.ElapsedMilliseconds > 1000 * 60 * Settings.AutoReloadTimer.Value)
            {
                Load();
                _reloadStopWatch.Restart();
            }

            if (!DownloadDone || !InitJsonDone) return;
            try
            {
                Highlighter();
            }
            catch
            {
                // This is sadly needed at the moment, because of PoEHUD's failure to read classNames on some occasitions.
                LogMessage("Error in: Highlighter(), restart PoEHUD.", 5);
            }

            try
            {
                VisibleStashValue();
            }
            catch
            {
                LogMessage("Error in: VisibleStashValue(), restart PoEHUD.", 5);
            }
            try
            {
                PropheccyDisplay();
            }
            catch
            {
                LogMessage("Error in: PropheccyDisplay(), restart PoEHUD.", 5);
            }

            var lineCount = 0;
            var window = GameController.Window.GetWindowRectangle();
            var drawPos = window.TopLeft;
            window.Width = 400;
            window.Height = 15;
            var uiHover = GameController.Game.IngameState.UIHover;
            if (uiHover.AsObject<HoverItemIcon>().ToolTipType == ToolTipType.ItemInChat) return;
            var inventoryItemIcon = uiHover.AsObject<HoverItemIcon>();
            var tooltip = inventoryItemIcon.Tooltip;
            var poeEntity = inventoryItemIcon.Item;
            if (tooltip == null || poeEntity.Address == 0 || !poeEntity.IsValid) return;
            var item = inventoryItemIcon.Item;
            var baseItemType = GameController.Files.BaseItemTypes.Translate(item.Path);
            if (baseItemType == null) return;
            var className = GetClassName(baseItemType);
            var mods = item.GetComponent<Mods>();
            var path = item.Path;
            var stack = item.GetComponent<Stack>();
            var stackable = stack?.Info != null;
            var identified = mods.Identified;
            var uniqueItemName = mods.UniqueName;
            var baseItemName = baseItemType.BaseName;
            var isMap = item.HasComponent<Map>();
            var itemRarity = mods.ItemRarity;
            ShowChaosValue(window, drawPos, item, className, path, identified, uniqueItemName, baseItemName, isMap, itemRarity, lineCount, stackable);
        }

        private List<NormalInventoryItem> GetInventoryItems()
        {
            var inventory = GameController.Game.IngameState.IngameUi.InventoryPanel;
            return !inventory.IsVisible ? null : inventory[InventoryIndex.PlayerInventory].VisibleInventoryItems;
        }
    }
}