using KitchenLib;
using KitchenLib.Logging;
using KitchenLib.Logging.Exceptions;
using KitchenMods;
using KitchenMysteryMenu.Customs.Appliances;
using KitchenMysteryMenu.Customs.Dishes;
using KitchenMysteryMenu.Customs.Dishes.Breakfast;
using KitchenMysteryMenu.Customs.Dishes.Burger;
using KitchenMysteryMenu.Customs.Dishes.Dumplings;
using KitchenMysteryMenu.Customs.Dishes.Fish;
using KitchenMysteryMenu.Customs.Dishes.HotDog;
using KitchenMysteryMenu.Customs.Dishes.Pies;
using KitchenMysteryMenu.Customs.Dishes.Pizza;
using KitchenMysteryMenu.Customs.Dishes.Salad;
using KitchenMysteryMenu.Customs.Dishes.Spaghetti;
using KitchenMysteryMenu.Customs.Dishes.Steaks;
using KitchenMysteryMenu.Customs.Dishes.StirFry;
using KitchenMysteryMenu.Customs.Dishes.Turkey;
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
            // Mystery Breakfast Dishes
            AddGameDataObject<MysteryBreakfastBaseDish>();

            // Mystery Burger Dishes
            AddGameDataObject<MysteryBurgerBaseDish>();

            // Mystery Dumplings Dishes
            AddGameDataObject<MysteryDumplingsBaseDish>();

            // Mystery Fish Dishes
            AddGameDataObject<MysteryFishBlueDish>();
            AddGameDataObject<MysteryFishPinkDish>();
            AddGameDataObject<MysteryFishCrabCakeDish>();
            AddGameDataObject<MysteryFishFilletDish>();
            AddGameDataObject<MysteryFishOysterDish>();
            AddGameDataObject<MysteryFishSpinyDish>();

            // Mystery Hot Dog Dishes
            AddGameDataObject<MysteryHotdogBaseDish>();

            // Mystery Pies Dishes
            AddGameDataObject<MysteryPiesBaseDish>();
            AddGameDataObject<MysteryPiesMeatDish>();

            // Mystery Pizza Dishes
            AddGameDataObject<MysteryPizzaBaseDish>();

            // Mystery Salad Dishes
            AddGameDataObject<MysterySaladBaseDish>();
            AddGameDataObject<MysterySaladTomatoDish>();

            // Mystery Pizza Dishes
            AddGameDataObject<MysterySpaghettiBaseDish>();

            // Mystery Steak Dishes
            AddGameDataObject<MysterySteakBaseDish>();
            AddGameDataObject<MysterySteakBonedDish>();
            AddGameDataObject<MysterySteakThickDish>();
            AddGameDataObject<MysterySteakThinDish>();

            // Mystery Stir Fry Dishes
            AddGameDataObject<MysteryStirFryBaseDish>();
            AddGameDataObject<MysteryStirFryRiceDish>();
            AddGameDataObject<MysteryStirFryBroccoliDish>();
            AddGameDataObject<MysteryStirFryCarrotDish>();
            AddGameDataObject<MysteryStirFrySteakDish>();

            // Mystery Turkey Dishes
            AddGameDataObject<MysteryTurkeyBaseDish>();

            // Mystery Dish cards
            AddGameDataObject<MysteryMenuBaseMainsDish>();
            AddGameDataObject<MysteryMenuCarnivoreVariationsDish>();
        }
    }
}
