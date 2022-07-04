using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExileCore;
using Newtonsoft.Json;
using Ninja_Price.API.PoeNinja.Classes;

namespace Ninja_Price.Main;

public partial class Main : BaseSettingsPlugin<Settings.Settings>
{
    private string NinjaDirectory;
    private CollectiveApiData CollectedData;
    private const string PoeLeagueApiList = "http://api.pathofexile.com/leagues?type=main&compact=1";
    private int _updating;

    public override bool Initialise()
    {
        Name = "Ninja Price";
        NinjaDirectory = Path.Join(DirectoryFullName, "NinjaData");
        Directory.CreateDirectory(NinjaDirectory);

        UpdateLeagueList();
        StartDataReload(Settings.LeagueList.Value, false);

        // Enable Events
        Settings.ReloadButton.OnPressed += () => StartDataReload(Settings.LeagueList.Value, true);

        CustomItem.InitCustomItem(this);

        return true;
    }

    public override void AreaChange(AreaInstance area)
    {
        if (GameController.Files.UniqueItemDescriptions.EntriesList.Count == 0)
        {
            GameController.Files.LoadFiles();
        }

        if (Settings.SyncCurrentLeague)
        {
            var playerLeague = PlayerLeague;
            if (playerLeague != null)
            {
                if (!Settings.LeagueList.Values.Contains(playerLeague))
                {
                    Settings.LeagueList.Values.Add(playerLeague);
                }

                if (Settings.LeagueList.Value != playerLeague)
                {
                    Settings.LeagueList.Value = playerLeague;
                    StartDataReload(Settings.LeagueList.Value, false);
                }
            }
        }
    }

    private void UpdateLeagueList()
    {
        var leagueList = new HashSet<string>();
        var playerLeague = PlayerLeague;
        if (playerLeague != null)
        {
            leagueList.Add(playerLeague);
        }

        try
        {
            var leagueListFromUrl = Utils.DownloadFromUrl(PoeLeagueApiList).Result;
            var leagueData = JsonConvert.DeserializeObject<List<Leagues>>(leagueListFromUrl);
            leagueList.UnionWith(leagueData.Where(league => !league.Id.Contains("SSF")).Select(league => league.Id));
        }
        catch (Exception ex)
        {
            LogError($"Failed to download the league list: {ex}");
        }

        leagueList.Add("Standard");
        leagueList.Add("Hardcore");

        if (!leagueList.Contains(Settings.LeagueList.Value))
        {
            Settings.LeagueList.Value = leagueList.MaxBy(x => x == playerLeague);
        }

        Settings.LeagueList.SetListValues(leagueList.ToList());
    }

    private string PlayerLeague
    {
        get
        {
            var playerLeague = GameController.IngameState.ServerData.League;
            if (string.IsNullOrWhiteSpace(playerLeague))
            {
                playerLeague = null;
            }
            else
            {
                if (playerLeague.StartsWith("SSF "))
                {
                    playerLeague = playerLeague["SSF ".Length..];
                }
            }

            return playerLeague;
        }
    }
}