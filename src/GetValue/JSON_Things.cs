using System.IO;
using GetValue.poe_ninja_api;
using GetValue.poe_ninja_api.Classes;
using Newtonsoft.Json;
using PoeHUD.Plugins;

namespace GetValue
{
    public partial class GetValuePlugin
    {
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

            //using (var r = new StreamReader(_ninjaDirectory + "SkillGems.json"))
            //{
            //    var json  = r.ReadToEnd();
            //    WhiteMaps = JsonConvert.DeserializeObject<WhiteMaps.RootObject>(json);
            //}
        }

        private void DownloadPoeNinjaApi(string league)
        {
            Api.SaveJson(_ninjaDirectory + "Currency.json", Api.DownloadApi($"http://cdn.poe.ninja/api/Data/GetCurrencyOverview?league={league}"));
            Api.SaveJson(_ninjaDirectory + "DivinationCards.json", Api.DownloadApi($"http://cdn.poe.ninja/api/Data/GetDivinationCardsOverview?league={league}"));
            Api.SaveJson(_ninjaDirectory + "Essences.json", Api.DownloadApi($"http://cdn.poe.ninja/api/Data/GetEssenceOverview?league={league}"));
            Api.SaveJson(_ninjaDirectory + "Fragments.json", Api.DownloadApi($"http://cdn.poe.ninja/api/Data/GetFragmentOverview?league={league}"));
            Api.SaveJson(_ninjaDirectory + "Prophecies.json", Api.DownloadApi($"http://cdn.poe.ninja/api/Data/GetProphecyOverview?league={league}"));
            Api.SaveJson(_ninjaDirectory + "UniqueAccessories.json", Api.DownloadApi($"http://cdn.poe.ninja/api/Data/GetUniqueAccessoryOverview?league={league}"));
            Api.SaveJson(_ninjaDirectory + "UniqueArmours.json", Api.DownloadApi($"http://cdn.poe.ninja/api/Data/GetUniqueArmourOverview?league={league}"));
            Api.SaveJson(_ninjaDirectory + "UniqueFlasks.json", Api.DownloadApi($"http://cdn.poe.ninja/api/Data/GetUniqueFlaskOverview?league={league}"));
            Api.SaveJson(_ninjaDirectory + "UniqueJewels.json", Api.DownloadApi($"http://cdn.poe.ninja/api/Data/GetUniqueJewelOverview?league={league}"));
            Api.SaveJson(_ninjaDirectory + "UniqueMaps.json", Api.DownloadApi($"http://cdn.poe.ninja/api/Data/GetUniqueMapOverview?league={league}"));
            Api.SaveJson(_ninjaDirectory + "UniqueWeapons.json", Api.DownloadApi($"http://cdn.poe.ninja/api/Data/GetUniqueWeaponOverview?league={league}"));
            Api.SaveJson(_ninjaDirectory + "WhiteMaps.json", Api.DownloadApi($"http://cdn.poe.ninja/api/Data/GetMapOverview?league={league}"));
            //Api.SaveJson(_ninjaDirectory + "SkillGems.json",         Api.DownloadApi($"http://cdn.poe.ninja/api/Data/GetSkillGemsOverview?league={league}"));
        }
    }
}