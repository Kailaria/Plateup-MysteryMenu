using KitchenLib;
using KitchenLib.Logging;
using KitchenLib.Logging.Exceptions;
using KitchenMods;
using KitchenMasteryMenu.Customs.Appliances;
using KitchenMasteryMenu.Customs.Dishes;
using KitchenMasteryMenu.Customs.Dishes.Breakfast;
using KitchenMasteryMenu.Customs.Dishes.Burger;
using KitchenMasteryMenu.Customs.Dishes.Cakes;
using KitchenMasteryMenu.Customs.Dishes.Coffee;
using KitchenMasteryMenu.Customs.Dishes.Desserts;
using KitchenMasteryMenu.Customs.Dishes.Dumplings;
using KitchenMasteryMenu.Customs.Dishes.Fish;
using KitchenMasteryMenu.Customs.Dishes.HotDog;
using KitchenMasteryMenu.Customs.Dishes.Pies;
using KitchenMasteryMenu.Customs.Dishes.Pizza;
using KitchenMasteryMenu.Customs.Dishes.Salad;
using KitchenMasteryMenu.Customs.Dishes.Sandwiches;
using KitchenMasteryMenu.Customs.Dishes.Sides;
using KitchenMasteryMenu.Customs.Dishes.Spaghetti;
using KitchenMasteryMenu.Customs.Dishes.Starters;
using KitchenMasteryMenu.Customs.Dishes.Steaks;
using KitchenMasteryMenu.Customs.Dishes.StirFry;
using KitchenMasteryMenu.Customs.Dishes.Sundaes;
using KitchenMasteryMenu.Customs.Dishes.Tacos;
using KitchenMasteryMenu.Customs.Dishes.Turkey;
using KitchenMasteryMenu.Customs.Ingredients;
using KitchenMasteryMenu.Customs.ItemGroups;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace KitchenMasteryMenu
{
    public class Mod : BaseMod, IModSystem
    {
        public const string MOD_GUID = "kailaria.MasteryMenu";
        public const string MOD_NAME = "MasteryMenu";
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
            AddGameDataObject<MasteryIngredientProvider>();
            AddGameDataObject<MasteryIngredientProvider2>();
            AddGameDataObject<MasteryIngredientProviderExtra>();
            AddGameDataObject<MasteryIngredientProviderBoard>();
            AddGameDataObject<MasteryIngredientProviderBoard2>();
            AddGameDataObject<MasteryIngredientProviderCakes>();
            AddGameDataObject<MasteryIngredientProviderCakes2>();
            AddGameDataObject<MasteryIngredientProviderCakes3>();
            AddGameDataObject<MasteryIngredientProviderComplexities>();
            AddGameDataObject<MasteryIngredientProviderExtra2>();
            AddGameDataObject<MasteryIngredientProviderExtra3>();
            AddGameDataObject<MasteryIngredientProviderExtra4>();
            AddGameDataObject<MasteryIngredientProviderExtra5>();
            AddGameDataObject<MasteryIngredientProviderExtra6>();
            AddGameDataObject<MasteryIngredientProviderExtra7>();
            AddGameDataObject<MasteryIngredientProviderSides>();
            AddGameDataObject<MasteryTrayProviderCakes>();
            AddGameDataObject<MasteryTrayProviderComplexities>();
        }

        private void AddIngredientGDOs()
        {
            AddGameDataObject<MasteryApple>();
            AddGameDataObject<MasteryCheese>();
            AddGameDataObject<MasteryChocolate>();
            AddGameDataObject<MasteryFlour>();
            AddGameDataObject<MasteryIceCreamStrawberry>();
            AddGameDataObject<MasteryIceCreamVanilla>();
            AddGameDataObject<MasteryLasagnePastaSheet>();
            AddGameDataObject<MasteryMeat>();
            AddGameDataObject<MasteryMushroom>();
            AddGameDataObject<MasteryOnion>();
            AddGameDataObject<MasteryPotato>();
            AddGameDataObject<MasterySugar>();
            AddGameDataObject<MasterySoySauce>();
            AddGameDataObject<MasterySurfNTurf>();
            AddGameDataObject<MasteryTeapot>();
            AddGameDataObject<MasteryWine>();

            AddGameDataObject<MasteryCookieTray>();
            AddGameDataObject<MasteryDoughnutTray>();
        }

        private void AddItemGroupGDOs()
        {
            // For the HQ Kitchen to work with minimal extra code
            // TODO: replace this with a version of the algorithm for HQ Kitchen
            //AddGameDataObject<MasteryDough>();
            //AddGameDataObject<MasteryPieMeatRaw>();
            //AddGameDataObject<MasteryPieMeatRawBlindBaked>();
        }

        private void AddDishGDOs()
        {
            // Mastery Dish cards
            AddGameDataObject<MasteryMenuBaseMainsDish>();
            AddGameDataObject<MasteryMenuBoardsTreatsDish>();
            AddGameDataObject<MasteryMenuCoffeeCakesPiesDish>();
            AddGameDataObject<MasteryMenuCoffeeCakeVarietyDish>();
            AddGameDataObject<MasteryMenuCarnivoreVariationsDish>();
            AddGameDataObject<MasteryMenuCondimentsDish>();
            AddGameDataObject<MasteryMenuSaucesSoupsDish>();
            AddGameDataObject<MasteryMenuSidesDish>();
            AddGameDataObject<MasteryMenuSubstitutionsComplexityDish>();
            AddGameDataObject<MasteryMenuToppingsDish>();
            AddGameDataObject<MasteryMenuVeggieVariationsDish>();

            // Mastery Starters
            AddGameDataObject<MasteryBreadBoardDish>();
            AddGameDataObject<MasteryBroccoliCheeseSoupDish>();
            AddGameDataObject<MasteryCarrotSoupDish>();
            AddGameDataObject<MasteryChristmasCrackersDish>();
            AddGameDataObject<MasteryMandarinStarterDish>();
            AddGameDataObject<MasteryMeatSoupDish>();
            AddGameDataObject<MasteryPumpkinSeedDish>();
            AddGameDataObject<MasteryPumpkinSoupDish>();
            AddGameDataObject<MasteryTomatoSoupDish>();

            // Mastery Sides
            AddGameDataObject<MasterySideBambooDish>();
            AddGameDataObject<MasterySideBroccoliDish>();
            AddGameDataObject<MasterySideChipsDish>();
            AddGameDataObject<MasterySideCornOnCobDish>();
            AddGameDataObject<MasterySideMashedPotatoDish>();
            AddGameDataObject<MasterySideOnionRingsDish>();
            AddGameDataObject<MasterySideRoastPotatoDish>();

            // Mastery Desserts
            AddGameDataObject<MasteryApplePieDish>();
            AddGameDataObject<MasteryCheeseBoardDish>();
            AddGameDataObject<MasteryCherryPieDish>();
            AddGameDataObject<MasteryIceCreamChocolateDish>();
            AddGameDataObject<MasteryIceCreamServingDish>();
            AddGameDataObject<MasteryIceCreamStrawberryDish>();
            AddGameDataObject<MasteryIceCreamVanillaDish>();
            AddGameDataObject<MasteryPumpkinPieDish>();

            // Mastery Breakfast Dishes
            AddGameDataObject<MasteryBreakfastBaseDish>();
            AddGameDataObject<MasteryBreakfastToppingBeansDish>();
            AddGameDataObject<MasteryBreakfastToppingEggsDish>();
            AddGameDataObject<MasteryBreakfastToppingMushroomDish>();
            AddGameDataObject<MasteryBreakfastToppingTomatoDish>();

            // Mastery Burger Dishes
            AddGameDataObject<MasteryBurgerBaseDish>();
            AddGameDataObject<MasteryBurgerToppingCheeseDish>();
            AddGameDataObject<MasteryBurgerToppingOnionDish>();
            AddGameDataObject<MasteryBurgerToppingTomatoDish>();
            AddGameDataObject<MasteryBurgerFreshPattyMeatDish>();

            // Mastery Cakes Dishes
            AddGameDataObject<MasteryCakeBatterRecipe>();
            AddGameDataObject<MasteryCakesCoffeeCookieDish>();
            AddGameDataObject<MasteryCakesCoffeeCupcakeDish>();
            AddGameDataObject<MasteryCakesCoffeeDoughnutDish>();
            AddGameDataObject<MasteryCakesCoffeeSpongeCakeDish>();
            AddGameDataObject<MasteryCakesChocolateBrownieDish>();
            AddGameDataObject<MasteryCakesChocolateCookieDish>();
            AddGameDataObject<MasteryCakesChocolateCupcakeDish>();
            AddGameDataObject<MasteryCakesChocolateDoughnutDish>();
            AddGameDataObject<MasteryCakesChocolateSpongeCakeDish>();
            AddGameDataObject<MasteryCakesLemonCookieDish>();
            AddGameDataObject<MasteryCakesLemonCupcakeDish>();
            AddGameDataObject<MasteryCakesLemonDoughnutDish>();
            AddGameDataObject<MasteryCakesLemonSpongeCakeDish>();

            // Mastery Coffee Dishes
            AddGameDataObject<MasteryCoffeeBaseDish>();
            AddGameDataObject<MasteryCoffeeCakeStandDish>();
            AddGameDataObject<MasteryCoffeeExtraMilkDish>();
            AddGameDataObject<MasteryCoffeeExtraSugarDish>();
            AddGameDataObject<MasteryCoffeeIcedDish>();
            AddGameDataObject<MasteryCoffeeLatteDish>();
            AddGameDataObject<MasteryTeaDish>();

            // Mastery Dumplings Dishes
            AddGameDataObject<MasteryDumplingsBaseDish>();
            AddGameDataObject<MasteryDumplingsSoySauceDish>();
            AddGameDataObject<MasteryDumplingsSeaweedDish>();

            // Mastery Fish Dishes
            AddGameDataObject<MasteryFishBlueDish>();
            AddGameDataObject<MasteryFishPinkDish>();
            AddGameDataObject<MasteryFishCrabCakeDish>();
            AddGameDataObject<MasteryFishFilletDish>();
            AddGameDataObject<MasteryFishOysterDish>();
            AddGameDataObject<MasteryFishSpinyDish>();

            // Mastery Hot Dog Dishes
            AddGameDataObject<MasteryHotdogBaseDish>();
            AddGameDataObject<MasteryHotdogKetchupDish>();
            AddGameDataObject<MasteryHotdogMustardDish>();

            // Mastery Pies Dishes
            AddGameDataObject<MasteryPiesBaseDish>();
            AddGameDataObject<MasteryPiesMeatDish>();
            AddGameDataObject<MasteryPiesMushroomDish>();
            AddGameDataObject<MasteryPiesVegetableDish>();

            // Mastery Pizza Dishes
            AddGameDataObject<MasteryPizzaBaseDish>();
            AddGameDataObject<MasteryPizzaMushroomDish>();
            AddGameDataObject<MasteryPizzaOnionDish>();

            // Mastery Salad Dishes
            AddGameDataObject<MasterySaladBaseDish>();
            AddGameDataObject<MasterySaladTomatoDish>();
            AddGameDataObject<MasterySaladToppingOliveDish>();
            AddGameDataObject<MasterySaladToppingOnionDish>();
            AddGameDataObject<MasterySaladAppleDish>();
            AddGameDataObject<MasterySaladPotatoDish>();

            // Mastery Sandwich Dishes
            AddGameDataObject<MasterySandwichBaseDish>();
            AddGameDataObject<MasterySandwichClubDish>();

            AddGameDataObject<MasterySandwichBreadDish>();
            AddGameDataObject<MasterySandwichCheeseDish>();
            AddGameDataObject<MasterySandwichEggDish>();
            AddGameDataObject<MasterySandwichHamSliceDish>();
            AddGameDataObject<MasterySandwichLettuceDish>();
            AddGameDataObject<MasterySandwichMayoDish>();
            AddGameDataObject<MasterySandwichOliveDish>();
            AddGameDataObject<MasterySandwichPickleDish>();
            AddGameDataObject<MasterySandwichTomatoDish>();

            AddGameDataObject<MasterySandwichGiantDish>();
            AddGameDataObject<MasterySandwichGiantBreadDish>();
            AddGameDataObject<MasterySandwichGiantCheeseDish>();
            AddGameDataObject<MasterySandwichGiantEggDish>();
            AddGameDataObject<MasterySandwichGiantHamSliceDish>();
            AddGameDataObject<MasterySandwichGiantLettuceDish>();
            AddGameDataObject<MasterySandwichGiantMayoDish>();
            AddGameDataObject<MasterySandwichGiantOliveDish>();
            AddGameDataObject<MasterySandwichGiantPickleDish>();
            AddGameDataObject<MasterySandwichGiantTomatoDish>();

            AddGameDataObject<MasterySandwichToastDish>();
            AddGameDataObject<MasterySandwichToastSliceDish>();
            AddGameDataObject<MasterySandwichToastCheeseDish>();
            AddGameDataObject<MasterySandwichToastEggDish>();
            AddGameDataObject<MasterySandwichToastHamSliceDish>();
            AddGameDataObject<MasterySandwichToastLettuceDish>();
            AddGameDataObject<MasterySandwichToastMayoDish>();
            AddGameDataObject<MasterySandwichToastOliveDish>();
            AddGameDataObject<MasterySandwichToastPickleDish>();
            AddGameDataObject<MasterySandwichToastTomatoDish>();

            // Mastery Spaghetti Dishes
            AddGameDataObject<MasterySpaghettiBaseDish>();
            AddGameDataObject<MasterySpaghettiBologneseDish>();
            AddGameDataObject<MasterySpaghettiCheesyDish>();
            AddGameDataObject<MasterySpaghettiLasagneDish>();
            AddGameDataObject<MasterySpaghettiStarchyDish>();

            // Mastery Steak Dishes
            AddGameDataObject<MasterySteakBaseDish>();
            AddGameDataObject<MasterySteakBonedDish>();
            AddGameDataObject<MasterySteakThickDish>();
            AddGameDataObject<MasterySteakThinDish>();
            AddGameDataObject<MasterySteakSauceMushroomSauceDish>();
            AddGameDataObject<MasterySteakSauceRedWineJusDish>();
            AddGameDataObject<MasterySteakToppingMushroomDish>();
            AddGameDataObject<MasterySteakToppingTomatoDish>();

            // Mastery Stir Fry Dishes
            AddGameDataObject<MasteryStirFryBaseDish>();
            AddGameDataObject<MasteryStirFryRiceDish>();
            AddGameDataObject<MasteryStirFryBroccoliDish>();
            AddGameDataObject<MasteryStirFryCarrotDish>();
            AddGameDataObject<MasteryStirFryBambooDish>();
            AddGameDataObject<MasteryStirFryMushroomDish>();
            AddGameDataObject<MasteryStirFrySteakDish>();
            AddGameDataObject<MasteryStirFrySoySauceDish>();

            // Mastery Sundae Dishes
            AddGameDataObject<MasterySundaeBaseDish>();
            AddGameDataObject<MasterySundaeCherryDish>();
            AddGameDataObject<MasterySundaeChocolateIceCreamDish>();
            AddGameDataObject<MasterySundaeChocolateSyrupDish>();
            AddGameDataObject<MasterySundaeGlassDish>();
            AddGameDataObject<MasterySundaeNutsDish>();
            AddGameDataObject<MasterySundaeStrawberryIceCreamDish>();
            AddGameDataObject<MasterySundaeStrawberrySyrupDish>();
            AddGameDataObject<MasterySundaeVanillaIceCreamDish>();

            AddGameDataObject<MasterySundaeGiantDish>();
            AddGameDataObject<MasterySundaeGiantCherryDish>();
            AddGameDataObject<MasterySundaeGiantChocolateIceCreamDish>();
            AddGameDataObject<MasterySundaeGiantChocolateSyrupDish>();
            AddGameDataObject<MasterySundaeGiantGlassDish>();
            AddGameDataObject<MasterySundaeGiantNutsDish>();
            AddGameDataObject<MasterySundaeGiantStrawberryIceCreamDish>();
            AddGameDataObject<MasterySundaeGiantStrawberrySyrupDish>();
            AddGameDataObject<MasterySundaeGiantVanillaIceCreamDish>();

            AddGameDataObject<MasteryHomemadeChocolateIceCreamDish>();
            AddGameDataObject<MasteryHomemadeStrawberryIceCreamDish>();
            AddGameDataObject<MasteryHomemadeVanillaIceCreamDish>();

            // Mastery Tacos Dishes
            AddGameDataObject<MasteryTacosBaseDish>();
            AddGameDataObject<MasteryTacosToppingCheeseDish>();
            AddGameDataObject<MasteryTacosToppingLettuceDish>();
            AddGameDataObject<MasteryTacosToppingOnionDish>();
            AddGameDataObject<MasteryTacosToppingTomatoDish>();

            // Mastery Turkey Dishes
            AddGameDataObject<MasteryTurkeyBaseDish>();
            AddGameDataObject<MasteryNutRoastDish>();
            AddGameDataObject<MasteryTurkeyCranberrySauceDish>();
            AddGameDataObject<MasteryTurkeyGravyDish>();
            AddGameDataObject<MasteryTurkeyStuffingDish>();
        }
    }
}
