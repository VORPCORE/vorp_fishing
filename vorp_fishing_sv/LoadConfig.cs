using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vorp_fishing_sv
{
    class LoadConfig : BaseScript
    {
        public static JObject Config = new JObject();
        public static string ConfigString;
        public static Dictionary<string, string> Langs = new Dictionary<string, string>();
        public static string resourcePath = $"{API.GetResourcePath(API.GetCurrentResourceName())}";

        public static bool isConfigLoaded = false;

        public LoadConfig()
        {
            EventHandlers[$"{API.GetCurrentResourceName()}:getConfig"] += new Action<Player>(getConfig);

            LoadConfigAndLang();
        }

        private void LoadConfigAndLang()
        {
            if (File.Exists($"{resourcePath}/Config.json"))
            {
                ConfigString = File.ReadAllText($"{resourcePath}/Config.json", Encoding.UTF8);
                Config = JObject.Parse(ConfigString);
                if (File.Exists($"{resourcePath}/{Config["defaultlang"]}.json"))
                {
                    string langstring = File.ReadAllText($"{resourcePath}/{Config["defaultlang"]}.json", Encoding.UTF8);
                    Langs = JsonConvert.DeserializeObject<Dictionary<string, string>>(langstring);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{API.GetCurrentResourceName()}: Language {Config["defaultlang"]}.json loaded!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{API.GetCurrentResourceName()}: {Config["defaultlang"]}.json Not Found");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{API.GetCurrentResourceName()}: Config.json Not Found");
                Console.ForegroundColor = ConsoleColor.White;
            }
            isConfigLoaded = true;
        }
        

        private async void getConfig([FromSource] Player source)
        {
            source.TriggerEvent($"{API.GetCurrentResourceName()}:SendConfig", ConfigString, Langs);
           
        }
    }
}
