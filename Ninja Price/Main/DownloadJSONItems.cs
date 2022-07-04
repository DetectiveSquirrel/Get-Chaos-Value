using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ninja_Price.API.PoeNinja;
using Ninja_Price.API.PoeNinja.Classes;

namespace Ninja_Price.Main;

public partial class Main
{
    private const string CurrencyURL = "https://poe.ninja/api/data/currencyoverview?type=Currency&league=";
    private const string DivinationCards_URL = "https://poe.ninja/api/data/itemoverview?type=DivinationCard&league=";
    private const string Essences_URL = "https://poe.ninja/api/data/itemoverview?type=Essence&league=";
    private const string Fragments_URL = "https://poe.ninja/api/data/currencyoverview?type=Fragment&league=";
    private const string UniqueAccessories_URL = "https://poe.ninja/api/data/itemoverview?type=UniqueAccessory&league=";
    private const string UniqueArmours_URL = "https://poe.ninja/api/data/itemoverview?type=UniqueArmour&league=";
    private const string UniqueFlasks_URL = "https://poe.ninja/api/data/itemoverview?type=UniqueFlask&league=";
    private const string UniqueJewels_URL = "https://poe.ninja/api/data/itemoverview?type=UniqueJewel&league=";
    private const string UniqueMaps_URL = "https://poe.ninja/api/data/itemoverview?type=UniqueMap&league=";
    private const string UniqueWeapons_URL = "https://poe.ninja/api/data/itemoverview?type=UniqueWeapon&league=";
    private const string WhiteMaps_URL = "https://poe.ninja/api/data/itemoverview?type=Map&league=";
    private const string Resonators_URL = "https://poe.ninja/api/data/itemoverview?type=Resonator&league=";
    private const string Fossils_URL = "https://poe.ninja/api/data/itemoverview?type=Fossil&league=";
    private const string Scarabs_URL = "https://poe.ninja/api/data/itemoverview?type=Scarab&league=";
    private const string Incubators_URL = "https://poe.ninja/api/data/itemoverview?type=Incubator&league=";
    private const string Oil_URL = "https://poe.ninja/api/data/itemoverview?type=Oil&league=";
    private const string DeliriumOrb_URL = "https://poe.ninja/api/data/itemoverview?type=DeliriumOrb&league=";
    private const string Vial_URL = "https://poe.ninja/api/data/itemoverview?type=Vial&league=";
    private const string Invitation_URL = "https://poe.ninja/api/data/ItemOverview?type=Invitation&league=";
    private const string HelmetEnchants_URL = "https://poe.ninja/api/data/ItemOverview?type=HelmetEnchant&league=";
    private const string Artifacts_URL = "https://poe.ninja/api/data/ItemOverview?type=Artifact&league=";
    private const string SkillGems_URL = "https://poe.ninja/api/data/ItemOverview?type=SkillGem&league=";

    private class LeagueMetadata
    {
        public DateTime LastLoadTime { get; set; }
    }

    private void StartDataReload(string league, bool forceRefresh)
    {
        LogMessage($"Getting data for {league}", 5);

        if (Interlocked.CompareExchange(ref _updating, 1, 0) != 0)
        {
            LogMessage("Update is already in progress");
            return;
        }

        Task.Run(async () =>
        {
            try
            {
                LogMessage("Gathering Data from Poe.Ninja.", 5);

                var newData = new CollectiveApiData();
                var tryWebFirst = forceRefresh;
                var metadataPath = Path.Join(NinjaDirectory, league, "meta.json");
                if (!tryWebFirst && Settings.AutoReload)
                {
                    tryWebFirst = await IsLocalCacheStale(metadataPath);
                }

                await LoadData<Currency.RootObject>("Currency.json", CurrencyURL, league, tryWebFirst, t => newData.Currency = t);
                await LoadData<DivinationCards.RootObject>("DivinationCards.json", DivinationCards_URL, league, tryWebFirst, t => newData.DivinationCards = t);
                await LoadData<Essences.RootObject>("Essences.json", Essences_URL, league, tryWebFirst, t => newData.Essences = t);
                await LoadData<Fragments.RootObject>("Fragments.json", Fragments_URL, league, tryWebFirst, t => newData.Fragments = t);
                await LoadData<UniqueAccessories.RootObject>("UniqueAccessories.json", UniqueAccessories_URL, league, tryWebFirst, t => newData.UniqueAccessories = t);
                await LoadData<UniqueArmours.RootObject>("UniqueArmours.json", UniqueArmours_URL, league, tryWebFirst, t => newData.UniqueArmours = t);
                await LoadData<UniqueFlasks.RootObject>("UniqueFlasks.json", UniqueFlasks_URL, league, tryWebFirst, t => newData.UniqueFlasks = t);
                await LoadData<UniqueJewels.RootObject>("UniqueJewels.json", UniqueJewels_URL, league, tryWebFirst, t => newData.UniqueJewels = t);
                await LoadData<UniqueMaps.RootObject>("UniqueMaps.json", UniqueMaps_URL, league, tryWebFirst, t => newData.UniqueMaps = t);
                await LoadData<UniqueWeapons.RootObject>("UniqueWeapons.json", UniqueWeapons_URL, league, tryWebFirst, t => newData.UniqueWeapons = t);
                await LoadData<WhiteMaps.RootObject>("WhiteMaps.json", WhiteMaps_URL, league, tryWebFirst, t => newData.WhiteMaps = t);
                await LoadData<Resonators.RootObject>("Resonators.json", Resonators_URL, league, tryWebFirst, t => newData.Resonators = t);
                await LoadData<Fossils.RootObject>("Fossils.json", Fossils_URL, league, tryWebFirst, t => newData.Fossils = t);
                await LoadData<Oils.RootObject>("Oils.json", Oil_URL, league, tryWebFirst, t => newData.Oils = t);
                await LoadData<Incubators.RootObject>("Incubators.json", Incubators_URL, league, tryWebFirst, t => newData.Incubators = t);
                await LoadData<Scarab.RootObject>("Scarabs.json", Scarabs_URL, league, tryWebFirst, t => newData.Scarabs = t);
                await LoadData<DeliriumOrb.RootObject>("DeliriumOrbs.json", DeliriumOrb_URL, league, tryWebFirst, t => newData.DeliriumOrb = t);
                await LoadData<Vials.RootObject>("Vials.json", Vial_URL, league, tryWebFirst, t => newData.Vials = t);
                await LoadData<Invitations.RootObject>("Invitations.json", Invitation_URL, league, tryWebFirst, t => newData.Invitations = t);
                await LoadData<HelmetEnchants.RootObject>("HelmetEnchants.json", HelmetEnchants_URL, league, tryWebFirst, t => newData.HelmetEnchants = t);
                await LoadData<Artifacts.RootObject>("Artifacts.json", Artifacts_URL, league, tryWebFirst, t => newData.Artifacts = t);
                await LoadData<SkillGems.RootObject>("SkillGems.json", SkillGems_URL, league, tryWebFirst, t => newData.SkillGems = t);

                new FileInfo(metadataPath).Directory.Create();
                await File.WriteAllTextAsync(metadataPath, JsonConvert.SerializeObject(new LeagueMetadata { LastLoadTime = DateTime.UtcNow }));

                LogMessage("Finished Gathering Data from Poe.Ninja.", 5);
                CollectedData = newData;
                ExaltedValue = CollectedData.Currency.Lines.Find(x => x.CurrencyTypeName == "Exalted Orb")?.ChaosEquivalent;
                LogMessage("Updated CollectedData.", 5);
            }
            finally
            {
                Interlocked.Exchange(ref _updating, 0);
            }
        });
    }

    private async Task<bool> IsLocalCacheStale(string metadataPath)
    {
        if (!File.Exists(metadataPath))
        {
            return true;
        }

        try
        {
            var metadata = JsonConvert.DeserializeObject<LeagueMetadata>(await File.ReadAllTextAsync(metadataPath));
            return DateTime.UtcNow - metadata.LastLoadTime > TimeSpan.FromMinutes(Settings.ReloadTimer);
        }
        catch (Exception ex)
        {
            if (Settings.Debug)
            {
                LogError($"Metadata loading failed: {ex}");
            }

            return true;
        }
    }

    private async Task LoadData<T>(string fileName, string url, string league, bool tryWebFirst, Action<T> dataAction)
    {
        var backupFile = Path.Join(NinjaDirectory, league, fileName);
        if (tryWebFirst)
        {
            if (await LoadDataFromWeb(fileName, url, league, dataAction, backupFile))
            {
                return;
            }
        }

        if (await LoadDataFromBackup(fileName, dataAction, backupFile))
        {
            return;
        }

        if (!tryWebFirst)
        {
            await LoadDataFromWeb(fileName, url, league, dataAction, backupFile);
        }
    }

    private async Task<bool> LoadDataFromBackup<T>(string fileName, Action<T> dataAction, string backupFile)
    {
        if (File.Exists(backupFile))
        {
            try
            {
                var data = JsonConvert.DeserializeObject<T>(await File.ReadAllTextAsync(backupFile));
                dataAction(data);
                return true;
            }
            catch (Exception backupEx)
            {
                if (Settings.Debug)
                {
                    LogError($"{fileName} backup data load failed: {backupEx}");
                }
            }
        }
        else if (Settings.Debug)
        {
            LogError($"No backup for {fileName}");
        }

        return false;
    }

    private async Task<bool> LoadDataFromWeb<T>(string fileName, string url, string league, Action<T> dataAction, string backupFile)
    {
        try
        {
            if (Settings.Debug)
            {
                LogMessage($"Downloading {fileName}");
            }

            var data = JsonConvert.DeserializeObject<T>(await Api.DownloadFromUrl(url + league));
            if (Settings.Debug)
            {
                LogMessage($"{fileName} downloaded");
            }

            try
            {
                new FileInfo(backupFile).Directory.Create();
                await File.WriteAllTextAsync(backupFile, JsonConvert.SerializeObject(data, Formatting.Indented));
            }
            catch (Exception ex)
            {
                var errorPath = backupFile + ".error";
                new FileInfo(errorPath).Directory.Create();
                await File.WriteAllTextAsync(errorPath, ex.ToString());
                if (Settings.Debug)
                {
                    LogError($"{fileName} save failed: {ex}");
                }
            }

            dataAction(data);
            return true;
        }
        catch (Exception ex)
        {
            if (Settings.Debug)
            {
                LogError($"{fileName} fresh data download failed: {ex}");
            }

            return false;
        }
    }
}