using KitchenData;
using KitchenLib.References;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMysteryMenu.Utils
{
    public class MysteryDishUtils
    {
        private static HashSet<int> ReusableItems = new HashSet<int>()
        {
            ItemReferences.Plate,
            ItemReferences.Wok,
            ItemReferences.Water,
            ItemReferences.MixingBowlEmpty,
            ItemReferences.Pot,
            ItemReferences.CookieTray,
            ItemReferences.BrownieTray,
            ItemReferences.DoughnutTray,
            ItemReferences.BigCakeTin
        };

        public static bool IsLimitedContainer(int providedItem)
        {
            return ReusableItems.Contains(providedItem);
        }
    }
}
