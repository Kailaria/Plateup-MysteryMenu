using KitchenLib;
using KitchenLib.Logging;
using KitchenLib.Logging.Exceptions;
using KitchenMods;
using KitchenMysteryMenu.Customs.Appliances;
using KitchenMysteryMenu.Customs.Dishes;
using KitchenMysteryMenu.Customs.Dishes.Breakfast;
using KitchenMysteryMenu.Customs.Dishes.Pies;
using KitchenMysteryMenu.Customs.Dishes.Steaks;
using KitchenMysteryMenu.Customs.Ingredients;
using KitchenMysteryMenu.Customs.ItemGroups;
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
            //Bundle = mod.GetPacks<AssetBundleModPack>().SelectMany(e => e.AssetBundles).FirstOrDefault() ?? throw new MissingAssetBundleException(MOD_GUID);
            Logger = InitLogger();

            // Add new GDOs
            AddApplianceGDOs();
            AddIngredientGDOs();
            AddItemGroupGDOs();
            AddDishGDOs();
        }

        private void AddApplianceGDOs()
        {
            AddGameDataObject<MysteryIngredientProvider>();
            AddGameDataObject<MysteryIngredientProvider2>();
        }

        private void AddIngredientGDOs()
        {
            // For the HQ Kitchen to work with minimal extra code
            AddGameDataObject<MysteryMeat>();
            AddGameDataObject<MysteryFlour>();
        }

        private void AddItemGroupGDOs()
        {
            // For the HQ Kitchen to work with minimal extra code
            AddGameDataObject<MysteryDough>();
            AddGameDataObject<MysteryPieMeatRaw>();
            AddGameDataObject<MysteryPieMeatRawBlindBaked>();
        }

        private void AddDishGDOs()
        {
            AddGameDataObject<MysterySteakDish>();
            AddGameDataObject<MysteryBreakfastDish>();
            AddGameDataObject<MysteryPiesDish>();
            AddGameDataObject<MysteryMenuDish>();
        }
    }
}
