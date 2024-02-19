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
    }
}
