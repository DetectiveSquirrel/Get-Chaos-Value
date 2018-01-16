using System.IO;
using System.Net;

namespace GetValue.poe_ninja_api
{
    public class Api
    {
        public static string DownloadApi(string url)
        {
            using (var web = new WebClient())
            {
                var json = web.DownloadString(url);
                return json;
            }
        }

        public static void SaveJson(string filePath, string data)
        {
            using (var file = File.CreateText(filePath))
            {
                file.WriteLine(data);
            }
        }

        public static string ReadJson(string filePath) => File.ReadAllText(filePath);
    }
}