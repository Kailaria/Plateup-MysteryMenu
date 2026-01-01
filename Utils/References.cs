using KitchenData;
using KitchenLib.Utils;
using KitchenMysteryMenu.Customs.Dishes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMysteryMenu.Utils
{
    internal static class References
    {
        public static readonly GameDataObject MysteryMenuBaseDish = GDOUtils.GetCastedGDO<Dish, MysteryMenuBaseMainsDish>();
        public static readonly DynamicMenuType DynamicMenuTypeMystery = (DynamicMenuType)VariousUtils.GetID($"{Mod.MOD_GUID}:{((Dish)MysteryMenuBaseDish).Name}");
        public static readonly int MaxIngredientCountForMinimumRecipe = 5;

        public static string DishCardDoNotAddFlavorText = "(This is an \"alsoAddsRecipes\" card. <b>DO NOT ADD</b> with Cards Manager)";

        // Sprites, colors, and colorized text
        public static string SpriteCake = "<sprite name=\"cake\">";//"<nobr><space=-0.2em><sprite name=\"cake\" tint=1>Cake</nobr>";
        public static string SpriteCakeTint1 = "<sprite name=\"cake\" tint=1>";
        public static string SpriteFillCoffee = "<sprite name=\"fill_coffee\">";
        public static string SpriteFillCoffeeTint1 = "<sprite name=\"fill_coffee\" tint=1>";

        public static string ColorCakeHex = "#F376D4"; // OFFICIAL FROM DATA
        public static string ColorHotDrinkHex = "#B8802F"; // Specific to this mod

        public static string PinkTintCakeText = "$cake$";
        public static string PinkTintCakesText = "$cakes$";
        public static string ColorTextCakeBatter = MysteryDishUtils.ColorizeSpriteTextToCake("Cake Batter");
        public static string ColorTextCakeFlavour = MysteryDishUtils.ColorizeSpriteTextToCake("Cake Flavour");
        public static string ColorTextCakeFlavours = MysteryDishUtils.ColorizeSpriteTextToCake("Cake Flavours");
        public static string ColorTextCakeForm = MysteryDishUtils.ColorizeSpriteTextToCake("Cake Form");
        public static string ColorTextCakeForms = MysteryDishUtils.ColorizeSpriteTextToCake("Cake Forms");
        public static string ColorTextHotDrink = MysteryDishUtils.ColorizeSpriteTextToHotDrink("Hot Drink");
        public static string ColorTextHotDrinks = MysteryDishUtils.ColorizeSpriteTextToHotDrink("Hot Drinks");

        // Temporary until KitchenLib updates w/ Sandos & Sundaes
        // Dishes
        public static int SandwichBaseDish = -1272159363;
        public static int SandwichCheeseDish = -469306490;
        public static int SandwichEggDish = 525953646;
        public static int SandwichMayoDish = -778718372;
        public static int SandwichToastDish = -72176411;
        public static int SandwichToppersDish = 368792675;
        public static int SandwichGiantDish = -1795285445;
        public static int SandwichTurkeyClubDish = 641008296;

        public static int SundaeBaseDish = 934171642;
        public static int SundaeSyrupsDish = 431260200;
        public static int SundaeToppingsDish = 1879652468;
        public static int SundaeGiantDish = -690833761;
        
        public static int SundaeHomemadeVariantDish = -1451591918;
        public static int SpaghettiStarchyVariantDish = -1974675533;

        // Menu Items
        public static int SandwichPiecemeal = 359143701;
        public static int SandwichGiantPiecemeal = -527624731;
        public static int SandwichToastPiecemeal = 1453228675;
        public static int SandwichClubPiecemeal = -1370653249;
        public static int SundaePiecemeal = 1695786399;
        public static int SundaeGiantPiecemeal = -1990180123;

        // Ingredients
        public static int HamSliced = 164425076;
        public static int Pickle = 1384381531;
        public static int SundaeGlass = 1116040881;
        public static int Strawberry = 578808284;
        public static int ChocolcateSyrup = 2105393112; // bottle
        public static int StrawberrySyrup = -764053970; // bottle
        public static int ChocolateSyrupServing = 118697218;
        public static int StrawberrySyrupServing = -1927341553;

        // Processes
        public static int ProcessFreeze = -1853370850;

        // Statuses
        public static int StatusBlockSinkBins = 8388643;

        // END TEMP BLOCK
    }
}
