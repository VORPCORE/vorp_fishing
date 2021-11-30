﻿using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vorp_fishing
{
    public class GetConfig : BaseScript
    {
        public static JObject Config = new JObject();
        public static JArray CharactArray = new JArray();
        public static Dictionary<string, string> Langs = new Dictionary<string, string>();
        public static bool IsLoaded = false;
        public string text;

        public GetConfig()
        {
            EventHandlers[$"{API.GetCurrentResourceName()}:SendConfig"] += new Action<string, ExpandoObject>(LoadDefaultConfig);
            TriggerServerEvent($"{API.GetCurrentResourceName()}:getConfig");
        }

        private void LoadDefaultConfig(string dc, ExpandoObject dl)
        {
            Config = JObject.Parse(dc);

            foreach (KeyValuePair<string, object> l in dl)
            {
                Langs[l.Key] = l.Value.ToString();
            }

            Fishing.SetupPrompts();

            IsLoaded = true;

        }

        

    }
}
