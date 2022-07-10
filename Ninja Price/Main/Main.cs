using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExileCore;
using Newtonsoft.Json;
using Ninja_Price.API.PoeNinja;
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

        GatherLeagueNames();
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
    }

    private void GatherLeagueNames()
    {
        List<string> leagueList;
        try
        {
            var leagueListFromUrl = Api.DownloadFromUrl(PoeLeagueApiList).Result;
            var leagueData = JsonConvert.DeserializeObject<List<Leagues>>(leagueListFromUrl);
            leagueList = leagueData.Where(league => !league.Id.Contains("SSF")).Select(league => league.Id).ToList();
        }
        catch (Exception ex)
        {
            LogError($"Failed to download the league list: {ex}");
            leagueList = new List<string> { GameController.IngameState.ServerData.League };
        }

        if (!leagueList.Contains(Settings.LeagueList.Value))
        {
            Settings.LeagueList.Value = leagueList[0];
        }

        Settings.LeagueList.SetListValues(leagueList);
    }
}