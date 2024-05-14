using KitchenLib;
using KitchenLib.Logging;
using KitchenLib.Logging.Exceptions;
using KitchenMods;
using KitchenMysteryMenu.Customs.Appliances;
using KitchenMysteryMenu.Customs.Dishes;
using KitchenMysteryMenu.Customs.Dishes.Breakfast;
using KitchenMysteryMenu.Customs.Dishes.Burger;
using KitchenMysteryMenu.Customs.Dishes.Cakes;
using KitchenMysteryMenu.Customs.Dishes.Coffee;
using KitchenMysteryMenu.Customs.Dishes.Desserts;
using KitchenMysteryMenu.Customs.Dishes.Dumplings;
using KitchenMysteryMenu.Customs.Dishes.Fish;
using KitchenMysteryMenu.Customs.Dishes.HotDog;
using KitchenMysteryMenu.Customs.Dishes.Pies;
using KitchenMysteryMenu.Customs.Dishes.Pizza;
using KitchenMysteryMenu.Customs.Dishes.Salad;
using KitchenMysteryMenu.Customs.Dishes.Sides;
using KitchenMysteryMenu.Customs.Dishes.Spaghetti;
using KitchenMysteryMenu.Customs.Dishes.Starters;
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
            AddGameDataObject<MysteryIngredientProviderBoard>();
            AddGameDataObject<MysteryIngredientProviderBoard2>();
            AddGameDataObject<MysteryIngredientProviderCakes>();
            AddGameDataObject<MysteryIngredientProviderCakes2>();
            AddGameDataObject<MysteryIngredientProviderCakes3>();
            AddGameDataObject<MysteryIngredientProviderExtra2>();
            AddGameDataObject<MysteryIngredientProviderExtra3>();
            AddGameDataObject<MysteryIngredientProviderExtra4>();
            AddGameDataObject<MysteryIngredientProviderExtra5>();
            AddGameDataObject<MysteryIngredientProviderExtra6>();
            AddGameDataObject<MysteryIngredientProviderExtra7>();
            AddGameDataObject<MysteryIngredientProviderSides>();
            AddGameDataObject<MysteryTrayProviderCakes>();
        }

        private void AddIngredientGDOs()
        {
            AddGameDataObject<MysteryApple>();
            AddGameDataObject<MysteryCheese>();
            AddGameDataObject<MysteryChocolate>();
            AddGameDataObject<MysteryFlour>();
            AddGameDataObject<MysteryIceCreamStrawberry>();
            AddGameDataObject<MysteryIceCreamVanilla>();
            AddGameDataObject<MysteryMeat>();
            AddGameDataObject<MysteryMushroom>();
            AddGameDataObject<MysteryOnion>();
            AddGameDataObject<MysteryPotato>();
            AddGameDataObject<MysterySugar>();
            AddGameDataObject<MysterySoySauce>();
            AddGameDataObject<MysterySurfNTurf>();
            AddGameDataObject<MysteryTeapot>();
            AddGameDataObject<MysteryWine>();

            AddGameDataObject<MysteryCookieTray>();
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
            AddGameDataObject<MysteryMenuBoardsTreatsDish>();
            AddGameDataObject<MysteryMenuCoffeeCakesPiesDish>();
            AddGameDataObject<MysteryMenuCoffeeCakeVarietyDish>();
            AddGameDataObject<MysteryMenuCarnivoreVariationsDish>();
            AddGameDataObject<MysteryMenuCondimentsDish>();
            AddGameDataObject<MysteryMenuSaucesSoupsDish>();
            AddGameDataObject<MysteryMenuSidesDish>();
            AddGameDataObject<MysteryMenuToppingsDish>();
            AddGameDataObject<MysteryMenuVeggieVariationsDish>();

            // Mystery Starters
            AddGameDataObject<MysteryBreadBoardDish>();
            AddGameDataObject<MysteryBroccoliCheeseSoupDish>();
            AddGameDataObject<MysteryCarrotSoupDish>();
            AddGameDataObject<MysteryChristmasCrackersDish>();
            AddGameDataObject<MysteryMandarinStarterDish>();
            AddGameDataObject<MysteryMeatSoupDish>();
            AddGameDataObject<MysteryPumpkinSeedDish>();
            AddGameDataObject<MysteryPumpkinSoupDish>();
            AddGameDataObject<MysteryTomatoSoupDish>();

            // Mystery Sides
            AddGameDataObject<MysterySideBambooDish>();
            AddGameDataObject<MysterySideBroccoliDish>();
            AddGameDataObject<MysterySideChipsDish>();
            AddGameDataObject<MysterySideCornOnCobDish>();
            AddGameDataObject<MysterySideMashedPotatoDish>();
            AddGameDataObject<MysterySideOnionRingsDish>();
            AddGameDataObject<MysterySideRoastPotatoDish>();

            // Mystery Desserts
            AddGameDataObject<MysteryApplePieDish>();
            AddGameDataObject<MysteryCheeseBoardDish>();
            AddGameDataObject<MysteryCherryPieDish>();
            AddGameDataObject<MysteryIceCreamChocolateDish>();
            AddGameDataObject<MysteryIceCreamServingDish>();
            AddGameDataObject<MysteryIceCreamStrawberryDish>();
            AddGameDataObject<MysteryIceCreamVanillaDish>();
            AddGameDataObject<MysteryPumpkinPieDish>();

            // Mystery Breakfast Dishes
            AddGameDataObject<MysteryBreakfastBaseDish>();
            AddGameDataObject<MysteryBreakfastToppingBeansDish>();
            AddGameDataObject<MysteryBreakfastToppingEggsDish>();
            AddGameDataObject<MysteryBreakfastToppingMushroomDish>();
            AddGameDataObject<MysteryBreakfastToppingTomatoDish>();

            // Mystery Burger Dishes
            AddGameDataObject<MysteryBurgerBaseDish>();
            AddGameDataObject<MysteryBurgerToppingCheeseDish>();
            AddGameDataObject<MysteryBurgerToppingOnionDish>();
            AddGameDataObject<MysteryBurgerToppingTomatoDish>();

            // Mystery Cakes Dishes
            AddGameDataObject<MysteryCakeBatterRecipe>();
            AddGameDataObject<MysteryCakesCoffeeCookieDish>();
            AddGameDataObject<MysteryCakesCoffeeCupcakeDish>();
            AddGameDataObject<MysteryCakesCoffeeSpongeCakeDish>();
            AddGameDataObject<MysteryCakesChocolateCookieDish>();
            AddGameDataObject<MysteryCakesChocolateCupcakeDish>();
            AddGameDataObject<MysteryCakesChocolateSpongeCakeDish>();
            AddGameDataObject<MysteryCakesLemonCookieDish>();
            AddGameDataObject<MysteryCakesLemonCupcakeDish>();
            AddGameDataObject<MysteryCakesLemonSpongeCakeDish>();

            // Mystery Cakes Dishes
            AddGameDataObject<MysteryCoffeeBaseDish>();
            AddGameDataObject<MysteryCoffeeCakeStandDish>();
            AddGameDataObject<MysteryCoffeeExtraMilkDish>();
            AddGameDataObject<MysteryCoffeeExtraSugarDish>();
            AddGameDataObject<MysteryCoffeeIcedDish>();
            AddGameDataObject<MysteryCoffeeLatteDish>();
            AddGameDataObject<MysteryTeaDish>();

            // Mystery Dumplings Dishes
            AddGameDataObject<MysteryDumplingsBaseDish>();
            AddGameDataObject<MysteryDumplingsSoySauceDish>();
            AddGameDataObject<MysteryDumplingsSeaweedDish>();

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
            AddGameDataObject<MysterySaladToppingOliveDish>();
            AddGameDataObject<MysterySaladToppingOnionDish>();
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
            AddGameDataObject<MysterySteakToppingMushroomDish>();
            AddGameDataObject<MysterySteakToppingTomatoDish>();

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
            AddGameDataObject<MysteryTurkeyStuffingDish>();
        }
    }
}
