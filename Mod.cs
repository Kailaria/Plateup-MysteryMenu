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
        public const string MOD_GUID = "kailaria.mysterymenu";
        public const string MOD_NAME = "MysteryMenu";
        public const string MOD_VERSION = "0.0.1";
        public const string MOD_AUTHOR = "Kailaria";
        public const string MOD_GAMEVERSION = ">=1.1.9";

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
            AddGameDataObject<MysteryIngredientProviderExtra>();
            AddGameDataObject<MysteryIngredientProviderExtra2>();
            AddGameDataObject<MysteryIngredientProviderExtra3>();
            AddGameDataObject<MysteryIngredientProviderExtra4>();
            AddGameDataObject<MysteryIngredientProviderExtra5>();
            AddGameDataObject<MysteryIngredientProviderExtra6>();
        }

        private void AddIngredientGDOs()
        {
            // For the HQ Kitchen to work with minimal extra code
            AddGameDataObject<MysteryApple>();
            AddGameDataObject<MysteryFlour>();
            AddGameDataObject<MysteryMeat>();
            AddGameDataObject<MysteryMushroom>();
            AddGameDataObject<MysteryOnion>();
            AddGameDataObject<MysterySoySauce>();
            AddGameDataObject<MysterySurfNTurf>();
            AddGameDataObject<MysteryWine>();
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
            // Mystery Dish cards
            AddGameDataObject<MysteryMenuBaseMainsDish>();
            AddGameDataObject<MysteryMenuCarnivoreVariationsDish>();
            AddGameDataObject<MysteryMenuCondimentsDish>();
            AddGameDataObject<MysteryMenuSaucesDish>();
            AddGameDataObject<MysteryMenuVeggieVariationsDish>();

            // Mystery Breakfast Dishes
            AddGameDataObject<MysteryBreakfastBaseDish>();

            // Mystery Burger Dishes
            AddGameDataObject<MysteryBurgerBaseDish>();

            // Mystery Dumplings Dishes
            AddGameDataObject<MysteryDumplingsBaseDish>();
            AddGameDataObject<MysteryDumplingsSoySauceDish>();

            // Mystery Fish Dishes
            AddGameDataObject<MysteryFishBlueDish>();
            AddGameDataObject<MysteryFishPinkDish>();
            AddGameDataObject<MysteryFishCrabCakeDish>();
            AddGameDataObject<MysteryFishFilletDish>();
            AddGameDataObject<MysteryFishOysterDish>();
            AddGameDataObject<MysteryFishSpinyDish>();

            // Mystery Hot Dog Dishes
            AddGameDataObject<MysteryHotdogBaseDish>();
            AddGameDataObject<MysteryHotdogKetchupDish>();
            AddGameDataObject<MysteryHotdogMustardDish>();

            // Mystery Pies Dishes
            AddGameDataObject<MysteryPiesBaseDish>();
            AddGameDataObject<MysteryPiesMeatDish>();
            AddGameDataObject<MysteryPiesMushroomDish>();
            AddGameDataObject<MysteryPiesVegetableDish>();

            // Mystery Pizza Dishes
            AddGameDataObject<MysteryPizzaBaseDish>();
            AddGameDataObject<MysteryPizzaMushroomDish>();
            AddGameDataObject<MysteryPizzaOnionDish>();

            // Mystery Salad Dishes
            AddGameDataObject<MysterySaladBaseDish>();
            AddGameDataObject<MysterySaladTomatoDish>();
            AddGameDataObject<MysterySaladAppleDish>();
            AddGameDataObject<MysterySaladPotatoDish>();

            // Mystery Spaghetti Dishes
            AddGameDataObject<MysterySpaghettiBaseDish>();
            AddGameDataObject<MysterySpaghettiBologneseDish>();
            AddGameDataObject<MysterySpaghettiCheesyDish>();

            // Mystery Steak Dishes
            AddGameDataObject<MysterySteakBaseDish>();
            AddGameDataObject<MysterySteakBonedDish>();
            AddGameDataObject<MysterySteakThickDish>();
            AddGameDataObject<MysterySteakThinDish>();
            AddGameDataObject<MysterySteakSauceMushroomSauceDish>();
            AddGameDataObject<MysterySteakSauceRedWineJusDish>();

            // Mystery Stir Fry Dishes
            AddGameDataObject<MysteryStirFryBaseDish>();
            AddGameDataObject<MysteryStirFryRiceDish>();
            AddGameDataObject<MysteryStirFryBroccoliDish>();
            AddGameDataObject<MysteryStirFryCarrotDish>();
            AddGameDataObject<MysteryStirFryBambooDish>();
            AddGameDataObject<MysteryStirFryMushroomDish>();
            AddGameDataObject<MysteryStirFrySteakDish>();
            AddGameDataObject<MysteryStirFrySoySauceDish>();

            // Mystery Turkey Dishes
            AddGameDataObject<MysteryTurkeyBaseDish>();
            AddGameDataObject<MysteryNutRoastDish>();
            AddGameDataObject<MysteryTurkeyCranberrySauceDish>();
            AddGameDataObject<MysteryTurkeyGravyDish>();
        }
    }
}
