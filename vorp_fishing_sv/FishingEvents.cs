using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using System.Drawing;

namespace vorp_fishing_sv
{
    public class FishingEvents : BaseScript
    {
        public static dynamic VorpCore;

        public FishingEvents()
        {
            TriggerEvent("getCore", new Action<dynamic>((core) =>
            {
                VorpCore = core;
            }));



            EventHandlers["vorp_fishing:FishKept"] += new Action<Player, string, float, string>(KeepFishNotification);
            EventHandlers["vorp_fishing:FishThrown"] += new Action<Player, string, float, string>(ThrowFishNotification);
            EventHandlers["vorp_fishing:FishToInventory"] += new Action<Player, string>(FishToInventory);
            EventHandlers["vorp_fishing:baitUsed"] += new Action<Player, string>(BaitUsed);
            TriggerEvent("vorpCore:registerUsableItem", "p_baitBread01x", new Action<dynamic>((data) =>
            {
                PlayerList pl = new PlayerList();
                Player p = pl[data.source];
                p.TriggerEvent("vorp_fishing:UseBait", "p_baitBread01x");
            }));

            TriggerEvent("vorpCore:registerUsableItem", "p_baitCorn01x", new Action<dynamic>((data) =>
            {
                PlayerList pl = new PlayerList();
                Player p = pl[data.source];
                p.TriggerEvent("vorp_fishing:UseBait", "p_baitCorn01x");
            }));

            TriggerEvent("vorpCore:registerUsableItem", "p_baitCheese01x", new Action<dynamic>((data) =>
            {
                PlayerList pl = new PlayerList();
                Player p = pl[data.source];
                p.TriggerEvent("vorp_fishing:UseBait", "p_baitCheese01x");
            }));

            TriggerEvent("vorpCore:registerUsableItem", "p_finishedragonfly01x", new Action<dynamic>((data) =>
            {
                PlayerList pl = new PlayerList();
                Player p = pl[data.source];
                p.TriggerEvent("vorp_fishing:UseBait", "p_finishedragonfly01x");
            }));

            TriggerEvent("vorpCore:registerUsableItem", "p_baitCricket01x", new Action<dynamic>((data) =>
            {
                PlayerList pl = new PlayerList();
                Player p = pl[data.source];
                p.TriggerEvent("vorp_fishing:UseBait", "p_baitCricket01x");
            }));

            TriggerEvent("vorpCore:registerUsableItem", "p_FinisdFishlure01x", new Action<dynamic>((data) =>
            {
                PlayerList pl = new PlayerList();
                Player p = pl[data.source];
                p.TriggerEvent("vorp_fishing:UseBait", "p_FinisdFishlure01x");
            }));

            TriggerEvent("vorpCore:registerUsableItem", "p_finishdcrawd01x", new Action<dynamic>((data) =>
            {
                PlayerList pl = new PlayerList();
                Player p = pl[data.source];
                p.TriggerEvent("vorp_fishing:UseBait", "p_finishdcrawd01x");
            }));

        }

        private struct RequestDataInternal
        {
            public string url;
            public string method;
            public string data;
            public dynamic headers;
        }


        private static void DiscordWebhook(string url, string data)
        {
            Dictionary<string, string> header = new Dictionary<string, string>() {
                { "Content-Type", "application/json" }
            };



            RequestDataInternal requestData = new RequestDataInternal();
            requestData.url = url;
            requestData.method = "POST";
            requestData.data = data;
            requestData.headers = header;
            string json = JsonConvert.SerializeObject(requestData);
            API.PerformHttpRequestInternal(json, Encoding.UTF8.GetByteCount(json));

        }

        private void BaitUsed([FromSource] Player player, string baitname)
        {
            int _source = int.Parse(player.Handle);
            TriggerEvent("vorpCore:subItem", _source, baitname, 1);
        }

        public void FishToInventory([FromSource]Player source, string modelName)
        {
            Debug.WriteLine("Model Name:" + modelName);
            int _source = int.Parse(source.Handle);
            source.TriggerEvent("vorp:TipRight", LoadConfig.Langs["CaughtFish"], 2000);
            TriggerEvent("vorpCore:addItem", _source, modelName, 1);
        }

        //
        //      CURRENTLY NOT IN USE
        //
        public void KeepFishNotification([FromSource] Player player, string FishName, float weight, string FishDescription)
        {

            int _source = int.Parse(player.Handle);
            string sid = ("steam:" + player.Identifiers["steam"]);

            DiscordMessage message = new DiscordMessage();
            message.Content = "A fish was caught!";
            message.TTS = false; 
            message.Username = "Webhook username";
            //message.AvatarUrl = "http://url-of-image";

            //embeds
            DiscordEmbed embed = new DiscordEmbed();
            embed.Title = sid + " caught a fish!";
            embed.Description = "The fish was kept!  😁";
            //embed.Url = "Embed Url";
            embed.Timestamp = DateTime.Now;
            embed.Color = Color.FromArgb(0,255,0); //alpha will be ignored, you can use any RGB color
            embed.Footer = new EmbedFooter() { Text = "Alentexas RP" };
            embed.Image = new EmbedMedia() { Url = "http://www.fws.gov/fisheries/freshwater-fish-of-america/images/originals/east_warm/Bluegilll_DuaneRavenArt.png", Width = 150, Height = 150 }; //valid for thumb and video
            //embed.Provider = new EmbedProvider() { Name = "Provider Name", Url = "Provider Url" };
            embed.Author = new EmbedAuthor() { Name = "VORP Fishing", Url = "Author Url", IconUrl = "http://www.fws.gov/fisheries/freshwater-fish-of-america/images/originals/east_warm/Bluegilll_DuaneRavenArt.png" };

            //fields
            embed.Fields = new List<EmbedField>();
            embed.Fields.Add(new EmbedField() { Name = "🦈 Fish Name: ", Value = FishName, InLine = true });
            embed.Fields.Add(new EmbedField() { Name = "⚖️ Weight: ", Value = weight.ToString(), InLine = true });
            embed.Fields.Add(new EmbedField() { Name = "🔍 Fish Info: ", Value = FishDescription, InLine = false });

            //set embed
            message.Embeds = new List<DiscordEmbed>();
            message.Embeds.Add(embed);

            Debug.WriteLine(JsonConvert.SerializeObject(message));

            DiscordWebhook(LoadConfig.DiscordWebhook, JsonConvert.SerializeObject(message));


        }


        //
        //      CURRENTLY NOT IN USE
        //

        public void ThrowFishNotification([FromSource] Player player, string FishName, float weight, string FishDescription)
        {

            int _source = int.Parse(player.Handle);
            string sid = ("steam:" + player.Identifiers["steam"]);

            DiscordMessage message = new DiscordMessage();
            message.Content = "A fish was caught!";
            message.TTS = false; //read message to everyone on the channel
            message.Username = "Webhook username";
            //message.AvatarUrl = "http://url-of-image";

            //embeds
            DiscordEmbed embed = new DiscordEmbed();
            embed.Title = sid + " caught a fish!";
            embed.Description = "The fish was kept!  😁";
            //embed.Url = "Embed Url";
            embed.Timestamp = DateTime.Now;
            embed.Color = Color.FromArgb(0, 255, 0); //alpha will be ignored, you can use any RGB color
            embed.Footer = new EmbedFooter() { Text = "Alentexas RP" };
            embed.Image = new EmbedMedia() { Url = "http://www.fws.gov/fisheries/freshwater-fish-of-america/images/originals/east_warm/Bluegilll_DuaneRavenArt.png", Width = 150, Height = 150 }; //valid for thumb and video
            //embed.Provider = new EmbedProvider() { Name = "Provider Name", Url = "Provider Url" };
            embed.Author = new EmbedAuthor() { Name = "VORP Fishing", Url = "Author Url", IconUrl = "http://www.fws.gov/fisheries/freshwater-fish-of-america/images/originals/east_warm/Bluegilll_DuaneRavenArt.png" };

            //fields
            embed.Fields = new List<EmbedField>();
            embed.Fields.Add(new EmbedField() { Name = "🦈 Fish Name: ", Value = FishName, InLine = true });
            embed.Fields.Add(new EmbedField() { Name = "⚖️ Weight: ", Value = weight.ToString(), InLine = true });
            embed.Fields.Add(new EmbedField() { Name = "🔍 Fish Info: ", Value = FishDescription, InLine = false });

            //set embed
            message.Embeds = new List<DiscordEmbed>();
            message.Embeds.Add(embed);

            DiscordWebhook(LoadConfig.DiscordWebhook, JsonConvert.SerializeObject(message));



        }

       
    }
}
