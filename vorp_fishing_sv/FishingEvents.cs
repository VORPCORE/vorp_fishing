using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

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




            EventHandlers["vorp_fishing:FishToInventory"] += new Action<Player, string>(FishToInventory);
            EventHandlers["vorp_fishing:baitUsed"] += new Action<Player, string>(BaitUsed);
            /* TriggerEvent("vorpCore:registerUsableItem", "fishbait", new Action<dynamic>((data) =>
             {
                 PlayerList pl = new PlayerList();
                 Player p = pl[data.source];
                 p.TriggerEvent("vorp_fishing:UseBait", data.baitname);
             }));
            */
            RegisterUsableItems();


        }

        public async Task RegisterUsableItems()
        {
            await Delay(3000);
            Debug.WriteLine($"Vorp Fishing: Loading {LoadConfig.Config["UsableBaits"].Count().ToString()} items usables ");
            for (int i = 0; i < LoadConfig.Config["UsableBaits"].Count(); i++)
            {
                int index = i;
                TriggerEvent("vorpCore:registerUsableItem", LoadConfig.Config["UsableBaits"][i]["Name"].ToString(), new Action<dynamic>((data) =>
                {
                    PlayerList pl = new PlayerList();
                    Player p = pl[data.source];
                    p.TriggerEvent("vorp_fishing:UseBait", index, LoadConfig.Config["UsableBaits"][index]["Name"].ToString());
                    TriggerEvent("vorpCore:subItem", data.source, LoadConfig.Config["UsableBaits"][index]["Name"].ToString(), 1);
                }));

            }
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
    }
}
