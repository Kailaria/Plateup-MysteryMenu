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

        public static string DishCardDoNotAddFlavorText = "(alsoAddsRecipes card, do not add with Cards Manager)";

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

        // Temporary until KitchenLib updates w/ Spaghetti
        public static int SpaghettiBaseDish = 1764920765;
        public static int SpaghettiBologneseDish = -1501485763;
        public static int SpaghettiCheesyDish = 1651927267;
        public static int LasagneDish = 803049136;

        public static int SpaghettiPomodoroPlated = 1900532137;
        public static int SpaghettiBolognesePlated = -1711635749;
        public static int SpaghettiCheesyPlated = -383718493;
        public static int LasagnePlated = 82891941;

        public static int SpaghettiRaw = -823534126;
        public static int Mince = -2047874552;
        public static int Butter = 1605072344;
        public static int LasagnePastaSheet = 1521319349;
        public static int LasagneTray = -2080052245;
    }
}
