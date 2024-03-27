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
