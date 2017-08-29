using System.IO;
using System.Net;

namespace GetValue.poe_ninja_api
{
    public class API
    {
        public static string DownloadAPI(string url)
        {
            using (WebClient web = new WebClient())
            {
                var json = web.DownloadString(url);
                return json;
            }
        }

        public static void SaveJSON(string filePath, string data)
        {
            using (StreamWriter file = File.CreateText(filePath))
            {
                file.WriteLine(data);
            }
        }

        public static string ReadJSON(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}
