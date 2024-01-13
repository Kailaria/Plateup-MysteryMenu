using KitchenLib;
using KitchenLib.Logging;
using KitchenLib.Logging.Exceptions;
using KitchenMods;
using KitchenMysteryMenu.Customs.Appliances;
using KitchenMysteryMenu.Customs.Dishes;
using KitchenMysteryMenu.Customs.Dishes.Steaks;
using KitchenMysteryMenu.Customs.Ingredients;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace KitchenMysteryMenu
{
    public class Mod : BaseMod, IModSystem
    {
        public const string MOD_GUID = "com.kailaria.mysterymenu";
        public const string MOD_NAME = "MysteryMenu";
        public const string MOD_VERSION = "0.1.0";
        public const string MOD_AUTHOR = "Kailaria";
        public const string MOD_GAMEVERSION = ">=1.1.8";

        public static AssetBundle Bundle;
        public static KitchenLogger Logger;

        public Mod() : base(MOD_GUID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_GAMEVERSION, Assembly.GetExecutingAssembly()) { }

        protected override void OnInitialise()
        {
            Logger.LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");
            // Modify existing GDOs
        }

        protected override void OnUpdate()
        {
        }

        protected override void OnPostActivate(KitchenMods.Mod mod)
        {
            // Add new GDOs
            //Bundle = mod.GetPacks<AssetBundleModPack>().SelectMany(e => e.AssetBundles).FirstOrDefault() ?? throw new MissingAssetBundleException(MOD_GUID);
            Logger = InitLogger();
            AddGameDataObject<MysteryIngredientProvider>();
            AddGameDataObject<MysteryMeat>();
            AddGameDataObject<MysterySteakDish>();
            AddGameDataObject<MysteryMenuDish>();
        }
    }
}
